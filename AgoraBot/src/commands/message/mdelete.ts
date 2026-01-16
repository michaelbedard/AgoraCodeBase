import { MessageFlags, SlashCommandBuilder } from "discord.js";
import {deleteRecurrentMessage, recurrentMessageStore} from "../../stores/recurrentMessageStore";

export default {
    data: new SlashCommandBuilder()
        .setName("mdelete")
        .setDescription("Delete a recurrent message")
        .addStringOption((option) =>
            option
                .setName("id")
                .setDescription("The ID of the message to delete")
                .setRequired(true)
        ),

    async execute(interaction: any) {
        const id = interaction.options.getString("id");
        const message = recurrentMessageStore.get(id);

        if (!message) {
            await interaction.reply({
                content: "❌ Recurrent message not found.",
                flags: MessageFlags.Ephemeral,
            });
            return;
        }

        await deleteRecurrentMessage(id);

        await interaction.reply({
            content: `🗑️ Recurrent message **${message.title}** has been deleted.`,
            flags: MessageFlags.Ephemeral,
        });
    },
};
