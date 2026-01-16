import {
    ActionRowBuilder,
    ButtonBuilder,
    ButtonStyle,
    Client,
    Events, GuildMember,
    Interaction,
    PermissionFlagsBits, TextChannel
} from "discord.js";

export default {
    name: Events.InteractionCreate,
    async execute(interaction: Interaction, client: Client) {
        if (!interaction.isButton() || !interaction.guild) return;

        const channel = interaction.channel as TextChannel;
        const member = interaction.member as GuildMember;

        if (interaction.customId === 'close_ticket') {

            if (!member || !channel) return;

            // **Permission Check (Optional but Recommended)**
            // Ensure only Admins/Mods or the original ticket creator can close the ticket
            if (!member.permissions.has(PermissionFlagsBits.ManageChannels) && member.user.id !== channel.name.split('-')[1]) {
                return interaction.reply({
                    content: 'You do not have permission to close this ticket.',
                    ephemeral: true
                });
            }

            // Disable the button to prevent double-clicks/errors
            const disabledRow = new ActionRowBuilder().addComponents(
                new ButtonBuilder()
                    .setCustomId('closing')
                    .setLabel('Closing...')
                    .setStyle(ButtonStyle.Secondary)
                    .setDisabled(true)
            );

            await interaction.update({
                content: 'This ticket is closing...',
                components: [disabledRow as any]
            });

            try {
                await channel.delete('Ticket closed by user/staff.');
            } catch (error) {
                console.error('Failed to delete ticket channel:', error);
            }
        }
    }
};