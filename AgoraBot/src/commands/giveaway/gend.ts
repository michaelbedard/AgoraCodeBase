import {
    ActionRowBuilder,
    ButtonBuilder, ButtonStyle,
    ChannelType, Client,
    EmbedBuilder,
    MessageFlags,
    SlashCommandBuilder
} from "discord.js";
import {Giveaway, giveaways, updateGiveaway} from "../../stores/giveawayStore";

export async function endGiveaway(client: Client, giveaway: Giveaway) {
    if (giveaway.status === "ended") {
        return { success: false, message: "⚠️ This giveaway has already ended." };
    }

    // --- 1. Pick Winners ---
    const winnerCount = giveaway.numberOfWinners || 1;
    let winners: string[] = [];
    let winnerText: string;

    if (!giveaway.participants || giveaway.participants.length === 0) {
        winnerText = "No participants joined!";
    } else {
        if (giveaway.participants.length <= winnerCount) {
            winners = giveaway.participants;
        } else {
            const pool = [...giveaway.participants];
            for (let i = 0; i < winnerCount; i++) {
                const randomIndex = Math.floor(Math.random() * pool.length);
                winners.push(pool[randomIndex]);
                pool.splice(randomIndex, 1);
            }
        }
        winnerText = winners.map(id => `<@${id}>`).join(", ") + " 🎉";
    }

    // --- 2. Update the Store ---
    giveaway.status = "ended";
    giveaway.winners = winners;
    await updateGiveaway(giveaway.id, giveaway);

    // --- 3. Fetch and Edit the Original Message ---
    try {
        const channel = await client.channels.fetch(giveaway.channelId);
        if (!channel || !channel.isTextBased() || channel.type === ChannelType.GroupDM) {
            throw new Error("Channel not found or is not a text channel.");
        }

        const message = await channel.messages.fetch(giveaway.id);

        // helper: Format the roles if they exist
        const rolesLine = (giveaway.allowedRoles && giveaway.allowedRoles.length > 0)
            ? `\n**Req. Roles:** ${giveaway.allowedRoles.map(r => `<@&${r}>`).join(", ")}`
            : "";

        // --- 4. Build Ended Embed ---
        const endedEmbed = EmbedBuilder.from(message.embeds[0])
            .setTitle(`🎉 ${giveaway.title} — Ended`)
            .setDescription(
                `**Hosted by:** <@${giveaway.host}>\n` + // Kept your clickable host fix
                `**${winners.length > 1 ? "Winners" : "Winner"}:** ${winnerText}` +
                rolesLine // <--- Adds the roles back to the ended message
            )
            .setColor(0xFF0000)
            .setTimestamp();

        // --- 5. Build Disabled Button ---
        const disabledRow = new ActionRowBuilder<ButtonBuilder>().addComponents(
            new ButtonBuilder()
                .setCustomId(`join_${giveaway.id}`)
                .setLabel("🎉 Join Giveaway")
                .setStyle(ButtonStyle.Success)
                .setDisabled(true)
        );

        // --- 6. Edit the Message (SILENT UPDATE) ---
        await message.edit({
            embeds: [endedEmbed],
            components: [disabledRow]
        });

        // --- 7. Send the NEW Message (THE PING) ---
        if (winners.length > 0) {
            // Create an embed for the details to avoid pinging the host
            const winnerEmbed = new EmbedBuilder()
                .setColor(0xFFD700) // Gold color
                .setTitle(`🏆 ${giveaway.title}`)
                .setDescription(
                    `**Hosted by:** <@${giveaway.host}>\n` +
                    `-# ➠ [Jump to Giveaway](${message.url})` // Clickable link to original message
                );

            await channel.send({
                content: `Congratulations ${winnerText}!`, // Only the winners are pinged here
                embeds: [winnerEmbed],
                reply: {
                    messageReference: message.id, // Visually links to the original message
                    failIfNotExists: false
                }
            });
        }

        return {
            success: true,
            message: `Giveaway **${giveaway.title}** ended successfully.hh`,
        };

    } catch (error) {
        console.error(`Failed to end giveaway message ${giveaway.id}:`, error);
        return {
            success: false,
            message: "Giveaway ended in store, but failed to update the message.",
        };
    }
}

export default {
    data: new SlashCommandBuilder()
        .setName("gend")
        .setDescription("End a giveaway and pick a winner")
        .addStringOption((option) =>
            option
                .setName("id")
                .setDescription("The ID of the giveaway to end")
                .setRequired(true)
        ),

    async execute(interaction: any) {
        const id = interaction.options.getString("id");
        const giveaway = giveaways.get(id);

        if (!giveaway) {
            await interaction.reply({
                content: "❌ Giveaway not found.",
                flags: MessageFlags.Ephemeral,
            });
            return;
        }

        const result = await endGiveaway(interaction.client, giveaway);
        await interaction.reply({
            content: result.message,
            flags: MessageFlags.Ephemeral,
        });
    },
};