import { ContextMenuCommandBuilder, ApplicationCommandType, MessageFlags } from "discord.js";
import { giveaways } from "../../stores/giveawayStore";
import {deleteGiveaway} from "./gdelete"; // 👈 import from same folder

export default {
    data: new ContextMenuCommandBuilder()
        .setName("Delete Giveaway")
        .setType(ApplicationCommandType.Message),

    async execute(interaction: any) {
        const message = interaction.targetMessage;
        const giveaway = Array.from(giveaways.values()).find(g => g.id === message.id);

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
