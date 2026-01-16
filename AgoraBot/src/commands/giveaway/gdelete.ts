import {ChannelType, Client, MessageFlags, SlashCommandBuilder} from "discord.js";
import {Giveaway, giveaways, deleteGiveaway as deleteGiveawayData } from "../../stores/giveawayStore";

export async function deleteGiveaway(client: Client, giveaway: Giveaway) {
    try {
        // --- 1. Fetch and Delete the Original Message ---
        const channel = await client.channels.fetch(giveaway.channelId);

        if (channel && channel.isTextBased() && channel.type !== ChannelType.GroupDM) {
            const message = await channel.messages.fetch(giveaway.id);
            await message.delete();
        }

    } catch (error: any) {
        console.warn(`Failed to delete giveaway message ${giveaway.id}:`, error.message);
        // We can ignore a "10008: Unknown Message" error,
        // it just means the message was already deleted.
        if (error.code !== 10008) {
            return {
                success: false,
                message: "❌ Failed to delete the message from Discord. It might be gone already.",
            };
        }
    }

    // --- 2. Update the Store ---
    // This should use giveaway.id, not just id
    await deleteGiveawayData(giveaway.id);

    // --- 3. Return a success message ---
    return {
        success: true,
        message: `🗑️ Giveaway **${giveaway.title}** has been deleted.`,
    };
}

export default {
    data: new SlashCommandBuilder()
        .setName("gdelete")
        .setDescription("Delete an active giveaway (no winner)")
        .addStringOption((option) =>
            option
                .setName("id")
                .setDescription("The ID of the giveaway to delete")
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

        const result = await deleteGiveaway(interaction.client, giveaway);

        // Reply with the result
        await interaction.reply({
            content: result.message,
            flags: result.success ? undefined : MessageFlags.Ephemeral,
        });
    },
};
