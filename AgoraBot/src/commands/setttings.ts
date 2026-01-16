import {SlashCommandBuilder, ChatInputCommandInteraction, MessageFlags} from "discord.js";
import {settingsStore} from "../stores/settingsStore";

export default {
    // 2. Define the Command Structure
    data: new SlashCommandBuilder()
        .setName('settings')
        .setDescription('Manage your bot preferences')

        // Subcommand: /settings show
        .addSubcommand(subcommand =>
            subcommand
                .setName('show')
                .setDescription('View your current settings')
        )

        // Subcommand: /settings set [setting] [value]
        .addSubcommand(subcommand =>
            subcommand
                .setName('set')
                .setDescription('Update a specific setting')
                // Argument 1: The key (XXX)
                .addStringOption(option =>
                    option.setName('setting')
                        .setDescription('The setting you want to change')
                        .setRequired(true)
                        .addChoices(
                            // Restrict user input to specific valid settings
                            { name: 'Giveaway Channel Id', value: 'giveawayChannelId' },
                            { name: 'Giveaway Duration', value: 'giveawayDuration' },
                            { name: 'Giveaway Color', value: 'giveawayColor' },
                            { name: 'Join Role Id', value: 'joinRoleId' },
                        )
                )
                // Argument 2: The value (YYY)
                .addStringOption(option =>
                    option.setName('value')
                        .setDescription('The new value for the setting')
                        .setRequired(true)
                )
        ),

    // 3. Execution Logic
    async execute(interaction: ChatInputCommandInteraction) {
        // specific types like 'ChatInputCommandInteraction' provide better autocomplete than 'any'

        const subcommand = interaction.options.getSubcommand();

        if (subcommand === 'show') {
            const settingsList = Array.from(settingsStore)
                .map(([key, val]) => `• **${key}**: ${val}`)
                .join('\n');

            await interaction.reply({
                content: `Here are your current settings:\n${settingsList}`,
                flags: MessageFlags.Ephemeral,
            });

        } else if (subcommand === 'set') {
            // Logic for: /settings set [setting] [value]
            const key = interaction.options.getString('setting', true);
            const value = interaction.options.getString('value', true);

            // Update the mock DB
            settingsStore.set(key, value);

            await interaction.reply({
                content: `✅ Successfully updated **${key}** to: \`${value}\``,
                flags: MessageFlags.Ephemeral,
            });
        }
    },
};