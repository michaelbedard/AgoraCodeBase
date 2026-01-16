import {
    SlashCommandBuilder,
    MessageFlags,
    ActionRowBuilder,
    StringSelectMenuBuilder,
    ComponentType,
    StringSelectMenuOptionBuilder
} from "discord.js";
import { giveaways } from "../../stores/giveawayStore";
import {rerollGiveaway} from "../../utils/rerollGiveaway";

export default {
    data: new SlashCommandBuilder()
        .setName("greroll")
        .setDescription("Finds new winners for a giveaway that has ended")
        .addStringOption(option =>
            option
                .setName("id")
                .setDescription("The ID of the giveaway to reroll")
                .setRequired(true)
        ),

    async execute(interaction: any) {
        const id = interaction.options.getString("id");
        const giveaway = giveaways.get(id);

        if (!giveaway) {
            return await interaction.reply({
                content: "❌ No giveaway found with that ID.",
                flags: MessageFlags.Ephemeral,
            });
        }

        // 1. Attempt Initial Reroll check
        const result = await rerollGiveaway(interaction.client, giveaway);

        // 2. Handle Case: Multiple Winners (Needs Selection)
        if (!result.success && result.requiresSelection && result.currentWinnerIds) {

            // Fetch users to get usernames for the dropdown labels
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

            // Create a collector for the dropdown
            try {
                const confirmation = await response.awaitMessageComponent({
                    filter: (i: any) => i.user.id === interaction.user.id,
                    componentType: ComponentType.StringSelect,
                    time: 60_000, // 60 seconds to choose
                });

                const targetId = confirmation.values[0];

                // Execute the reroll with the specific target
                const finalResult = await rerollGiveaway(interaction.client, giveaway, targetId);

                await confirmation.update({
                    content: finalResult.message,
                    components: [], // Remove dropdown
                });

            } catch (e) {
                await interaction.editReply({
                    content: "❌ Reroll timed out. No changes made.",
                    components: [],
                });
            }
            return;
        }

        // 3. Handle Standard Case (Single winner or validation error)
        await interaction.reply({
            content: result.message,
            flags: result.success ? undefined : MessageFlags.Ephemeral,
        });
    },
};