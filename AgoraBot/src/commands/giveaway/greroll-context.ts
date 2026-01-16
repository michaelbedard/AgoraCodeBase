import {
    ContextMenuCommandBuilder,
    ApplicationCommandType,
    MessageFlags,
    ActionRowBuilder,
    StringSelectMenuBuilder,
    StringSelectMenuOptionBuilder,
    ComponentType
} from "discord.js";
import { giveaways } from "../../stores/giveawayStore";
import {rerollGiveaway} from "../../utils/rerollGiveaway";

export default {
    data: new ContextMenuCommandBuilder()
        .setName("Reroll Giveaway")
        .setType(ApplicationCommandType.Message),

    async execute(interaction: any) {
        const message = interaction.targetMessage;
        const giveaway = Array.from(giveaways.values()).find(g => g.id === message.id);

        if (!giveaway) {
            return await interaction.reply({
                content: "❌ No giveaway found with that ID.",
                flags: MessageFlags.Ephemeral,
            });
        }

        const result = await rerollGiveaway(interaction.client, giveaway);

        if (!result.success && result.requiresSelection && result.currentWinnerIds) {

            const options = await Promise.all(result.currentWinnerIds.map(async (winnerId) => {
                const user = await interaction.client.users.fetch(winnerId).catch(() => null);
                return new StringSelectMenuOptionBuilder()
                    .setLabel(user ? user.username : `Unknown User (${winnerId})`)
                    .setValue(winnerId)
                    .setDescription(`Reroll this winner`);
            }));

            const selectMenu = new StringSelectMenuBuilder()
                .setCustomId('reroll_winner_select')
                .setPlaceholder('Select which winner to reroll...')
                .addOptions(options);

            const row = new ActionRowBuilder<StringSelectMenuBuilder>().addComponents(selectMenu);

            const response = await interaction.reply({
                content: "⚠️ **Multiple winners detected.** Who do you want to reroll?",
                components: [row],
                flags: MessageFlags.Ephemeral,
            });

            try {
                const confirmation = await response.awaitMessageComponent({
                    filter: (i: any) => i.user.id === interaction.user.id,
                    componentType: ComponentType.StringSelect,
                    time: 60_000,
                });

                const targetId = confirmation.values[0];
                const finalResult = await rerollGiveaway(interaction.client, giveaway, targetId);

                await confirmation.update({
                    content: finalResult.message,
                    components: [],
                });

            } catch (e) {
                await interaction.editReply({
                    content: "❌ Reroll timed out. No changes made.",
                    components: [],
                });
            }
            return;
        }

        await interaction.reply({
            content: result.message,
            flags: result.success ? undefined : MessageFlags.Ephemeral,
        });
    },
};