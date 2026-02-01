import { SlashCommandBuilder, EmbedBuilder } from "discord.js";
import {games} from "../../stores/GameStore";

export default {
    data: new SlashCommandBuilder()
        .setName('game')
        .setDescription('Board game reference system')
        // SUBCOMMAND 1: INFO (The Box Art)
        .addSubcommand(subcommand =>
            subcommand
                .setName('info')
                .setDescription('View player count, complexity, and details')
                .addStringOption(option =>
                    option.setName('title')
                        .setDescription('Which game?')
                        .setRequired(true)
                        .addChoices(...games.map(g => ({ name: g.title, value: g.id })))
                )
        )
        // SUBCOMMAND 2: RULES (The Manual)
        .addSubcommand(subcommand =>
            subcommand
                .setName('rules')
                .setDescription('Read the specific rules for a game')
                .addStringOption(option =>
                    option.setName('title')
                        .setDescription('Which game?')
                        .setRequired(true)
                        .addChoices(...games.map(g => ({ name: g.title, value: g.id })))
                )
        ),

    async execute(interaction: any) {
        // 1. Get the Game ID and the Subcommand used
        const gameId = interaction.options.getString('title');
        const subcommand = interaction.options.getSubcommand();
        const game = games.get(gameId);

        if (!game) {
            return interaction.reply({ content: '❌ Game not found.', ephemeral: true });
        }

        // --- MODE A: INFO ---
        if (subcommand === 'info') {
            const infoEmbed = new EmbedBuilder()
                .setColor(0xE74C3C)
                .setTitle(`🎲 ${game.title}`)
                .setDescription(game.description)
                .setThumbnail(game.thumbnail)
                .addFields(
                    { name: '👥 Players', value: `${game.minPlayers}-${game.maxPlayers}`, inline: true },
                    { name: '⏱️ Playtime', value: game.playtime, inline: true },
                    { name: '🧠 Complexity', value: game.complexity, inline: true },
                );

            return interaction.reply({ embeds: [infoEmbed] });
        }

        // --- MODE B: RULES ---
        if (subcommand === 'rules') {
            const rulesEmbed = new EmbedBuilder()
                .setColor(0xFFFFFF)
                .setTitle(`📜 Rules: ${game.title}`)
                .setDescription(game.rules)
                .setFooter({ text: 'have fun!' });

            return interaction.reply({
                embeds: [rulesEmbed],
                ephemeral: true
            });
        }
    },
};