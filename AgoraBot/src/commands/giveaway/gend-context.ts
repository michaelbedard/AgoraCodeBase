import { ContextMenuCommandBuilder, ApplicationCommandType, MessageFlags } from "discord.js";
import { giveaways } from "../../stores/giveawayStore";
import { endGiveaway } from "./gend"; // 👈 import from same folder

export default {
    data: new ContextMenuCommandBuilder()
        .setName("End Giveaway")
        .setType(ApplicationCommandType.Message),

    async execute(interaction: any) {
        const message = interaction.targetMessage;
        const giveaway = Array.from(giveaways.values()).find(g => g.id === message.id);

        if (!giveaway) {
            await interaction.reply({
                content: "❌ This message isn’t linked to a giveaway.",
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
