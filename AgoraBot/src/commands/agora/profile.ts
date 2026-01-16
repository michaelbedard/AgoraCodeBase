import { SlashCommandBuilder, EmbedBuilder } from "discord.js";
import {getOrCreateUser} from "../../stores/UserStore";

export default {
    data: new SlashCommandBuilder()
        .setName('profile')
        .setDescription('View your player stats and reputation')
        .addUserOption(option =>
            option.setName('target')
                .setDescription('The user whose profile you want to view')
                .setRequired(false)
        ),

    async execute(interaction: any) {
        const targetUser = interaction.options.getUser('target') || interaction.user;
        const userData = await getOrCreateUser(targetUser.id, targetUser.username);

        // Calculate Win Rate (Avoid division by zero)
        const winRate = userData.stats.gamesPlayed > 0
            ? ((userData.stats.wins / userData.stats.gamesPlayed) * 100).toFixed(1)
            : "0.0";

        // Build the "Character Sheet" Embed
        const profileEmbed = new EmbedBuilder()
            .setColor(0x3498DB) // Blue implies "Information/UI"
            .setTitle(`👤 Player Profile: ${userData.username}`)
            .setThumbnail(targetUser.displayAvatarURL())
            .addFields(
                {
                    name: '📅 Member Since',
                    value: `<t:${Math.floor(userData.joinedAt / 1000)}:D>`, // Discord Timestamp formatting
                    inline: false
                },
                {
                    name: '🏆 Career Stats',
                    value: `**Wins:** ${userData.stats.wins}\n**Losses:** ${userData.stats.losses}\n**Win Rate:** ${winRate}%`,
                    inline: true
                },
                {
                    name: '💰 Inventory',
                    value: `**Coins:** ${userData.coins}`,
                    inline: true
                },
            )
            .setFooter({ text: `User ID: ${userData.id}` });

        await interaction.reply({
            embeds: [profileEmbed]
        });
    },
};