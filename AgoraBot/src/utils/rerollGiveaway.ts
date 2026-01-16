import { Client, ChannelType, EmbedBuilder } from "discord.js";
import {Giveaway, updateGiveaway} from "../stores/giveawayStore";

// Define return type for better handling in commands
interface RerollResult {
    success: boolean;
    message: string;
    requiresSelection?: boolean; // New flag
    currentWinnerIds?: string[]; // To populate dropdown
}

export async function rerollGiveaway(client: Client, giveaway: Giveaway, targetWinnerId?: string): Promise<RerollResult> {
    // --- 1. Validation Checks ---
    if (giveaway.status !== "ended") {
        return { success: false, message: "⚠️ This giveaway is still active! You can only reroll after it has ended." };
    }

    if (!giveaway.participants || giveaway.participants.length === 0) {
        return { success: false, message: "⚠️ No participants found for this giveaway." };
    }

    const winnerCount = giveaway.numberOfWinners || 1;
    const currentWinners = giveaway.winners || [];

    // --- 2. Check for Multiple Winners / Selection Requirement ---
    // If we have multiple winners and the user hasn't chosen one to kick yet:
    if (currentWinners.length > 1 && !targetWinnerId) {
        return {
            success: false,
            message: "Select a winner to reroll.",
            requiresSelection: true,
            currentWinnerIds: currentWinners
        };
    }

    // --- 3. Determine Logic for New Winners ---

    let winnersToKeep: string[] = [];
    let winnersToReplaceCount = 0;

    if (currentWinners.length === 1) {
        // Single winner mode: We replace the only winner
        winnersToKeep = [];
        winnersToReplaceCount = 1;
        // Implicitly, the target is the only winner
        targetWinnerId = currentWinners[0];
    } else {
        // Multi winner mode (Target provided): We keep everyone except the target
        winnersToKeep = currentWinners.filter(id => id !== targetWinnerId);
        winnersToReplaceCount = winnerCount - winnersToKeep.length;
    }

    // Pool A: People who did NOT win last time AND are not the person being rerolled
    // We filter out:
    // 1. People currently staying as winners (winnersToKeep)
    // 2. The person we are explicitly rerolling (targetWinnerId) - they shouldn't win immediately again
    const freshCandidates = giveaway.participants.filter(p =>
        !winnersToKeep.includes(p) && p !== targetWinnerId
    );

    // Pool B: Previous winners (if we run out of fresh people)
    // We treat the person being rerolled as a 'previous winner' valid for backup,
    // but typically we want fresh blood first.
    const previousWinners = [targetWinnerId].filter((id): id is string => !!id);

    const newAdditions: string[] = [];

    while (newAdditions.length < winnersToReplaceCount) {
        let selectedId: string;

        if (freshCandidates.length > 0) {
            const randomIndex = Math.floor(Math.random() * freshCandidates.length);
            selectedId = freshCandidates[randomIndex];
            freshCandidates.splice(randomIndex, 1);
        } else if (previousWinners.length > 0) {
            const randomIndex = Math.floor(Math.random() * previousWinners.length);
            selectedId = previousWinners[randomIndex];
            previousWinners.splice(randomIndex, 1);
        } else {
            // No one left to pick (everyone participated is already a winner)
            break;
        }
        newAdditions.push(selectedId);
    }

    // If we couldn't find a replacement (e.g., specific user reroll but no other participants), abort
    if (newAdditions.length === 0) {
        return { success: false, message: "⚠️ Not enough other participants to pick a new winner!" };
    }

    // Combine kept winners with new winners
    const finalWinnerList = [...winnersToKeep, ...newAdditions];

    // --- 4. Update Store ---
    giveaway.winners = finalWinnerList;
    await updateGiveaway(giveaway.id, giveaway);

    // Format text for display
    const winnerText = finalWinnerList.map(id => `<@${id}>`).join(", ") + " 🎉";

    // --- 5. Edit the Original Message ---
    try {
        const channel = await client.channels.fetch(giveaway.channelId);
        if (channel && channel.isTextBased() && channel.type !== ChannelType.GroupDM) {
            const message = await channel.messages.fetch(giveaway.id);
            const originalEmbed = message.embeds[0];

            const rerolledEmbed = EmbedBuilder.from(originalEmbed)
                .setDescription(
                    `**Hosted by:** ${giveaway.host}\n` +
                    `**Winners:** ${winnerText}`
                );

            await message.edit({
                content: `🎉 **Reroll Complete!** Congratulations ${winnerText}!`,
                embeds: [rerolledEmbed],
                allowedMentions: { parse: ['users'] }
            });
        }
    } catch (error: any) {
        console.error("Failed to edit giveaway message after reroll:", error.message);
    }

    // --- 6. Return Success ---
    return {
        success: true,
        message: `🎉 The winner has been rerolled! New winner: <@${newAdditions[0]}> (Winners: ${winnerText})`,
    };
}