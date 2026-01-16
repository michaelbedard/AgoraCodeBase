import {
    ActionRowBuilder,
    ButtonBuilder,
    ButtonStyle, ChannelType,
    Client, EmbedBuilder,
    Events, GuildMember,
    Interaction, MessageFlags,
    PermissionFlagsBits, TextChannel
} from "discord.js";
import {ticketMessages} from "../../stores/ticketButtonStore";

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: Client) {
        if (!interaction.isButton() || !interaction.guild) return;
        if (!interaction.customId.startsWith('createTicketButton_')) return;

        await interaction.deferReply({ flags: MessageFlags.Ephemeral });

        // CustomId format is: 'createTicketButton_[id]'
        const parts = interaction.customId.split('_');
        const id = parts[1];

        // Retrieve custom message information
        let customMessageData = ticketMessages.get(id);

        // Fallback data if storage fails
        if (!customMessageData) {
            customMessageData = {
                title: `Ticket for ${interaction.member!.user.username}`,
                description: 'A staff member will be with you shortly. Please explain your issue.',
                color: "#0099ff",
                footer: null,
                banner: null,
                thumbnail: null,
                pingRoles: [],
            };
        }

        const member = interaction.member;
        const guild = interaction.guild;

        const permissionOverwrites = [
            {
                id: guild.id, // @everyone
                deny: [PermissionFlagsBits.ViewChannel],
            },
            {
                id: member.id, // The user
                allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.SendMessages, PermissionFlagsBits.ReadMessageHistory, PermissionFlagsBits.AttachFiles],
            },
            {
                id: client.user?.id, // The Bot
                allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.SendMessages, PermissionFlagsBits.ManageChannels],
            }
        ];

        // **Conditionally add the admin/mod role's permission overwrite**
        const adminRole = guild.roles.cache.find((role: any) => role.name.toLowerCase() === 'admin' || role.name.toLowerCase() === 'mod')
        if (adminRole) {
            permissionOverwrites.push({
                id: adminRole.id, // Admins/Mods
                allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.SendMessages, PermissionFlagsBits.ReadMessageHistory, PermissionFlagsBits.ManageMessages],
            });
        }

        try {
            // Create the private channel
            const channel = await guild.channels.create({
                name: `ticket-${member.user.username}`,
                type: ChannelType.GuildText,
                permissionOverwrites: permissionOverwrites,
            });

            // Build the embed
            const ticketEmbed = new EmbedBuilder()
                .setTitle(customMessageData.title)
                .setDescription(customMessageData.description)
                .setTimestamp();

            if (customMessageData.footer) {
                ticketEmbed.setFooter({ text: customMessageData.footer });
            }

            // Banner Image (Large, bottom)
            if (customMessageData.banner) {
                try {
                    new URL(customMessageData.banner);
                    ticketEmbed.setImage(customMessageData.banner);
                } catch (error) {
                    await interaction.followUp({ content: "Warning: The banner image URL was invalid and was ignored.", flags: MessageFlags.Ephemeral });
                }
            }

            // Thumbnail Image (Small, top-right)
            if (customMessageData.thumbnail) {
                try {
                    new URL(customMessageData.thumbnail);
                    ticketEmbed.setThumbnail(customMessageData.thumbnail);
                } catch (error) {
                    await interaction.followUp({ content: "Warning: The thumbnail image URL was invalid and was ignored.", flags: MessageFlags.Ephemeral });
                }
            }

            if (customMessageData.color && /^#[0-9A-F]{6}$/i.test(customMessageData.color)) {
                ticketEmbed.setColor(customMessageData.color as any);
            } else if (customMessageData.color) {
                await interaction.followUp({ content: "Warning: The color was not a valid 6-digit hex code (e.g., #0099ff) and was ignored.", flags: MessageFlags.Ephemeral });
            }

            const mentionString = customMessageData.pingRoles.map(id => `<@&${id}>`).join(" ");

            // --- Create the Close Button ---
            const closeButton = new ButtonBuilder()
                .setCustomId('close_ticket')
                .setLabel('Close Ticket')
                .setStyle(ButtonStyle.Danger) // Red button
                .setEmoji('🔒');

            // --- Create the Action Row to hold the button ---
            const row = new ActionRowBuilder().addComponents(closeButton);

            await channel.send({
                content: `Welcome ${member}! ${mentionString} will be with you shortly.`,
                embeds: [ticketEmbed],
                components: [row as any]
            });

            // Reply to the user who clicked the button
            await interaction.editReply({
                content: `✅ Your ticket has been created! Please go to ${channel}`
            });

        } catch (error) {
            console.error("Failed to create ticket channel:", error);
            await interaction.editReply({ content: 'There was an error creating your ticket. Please contact an administrator.' });
        }
    }
};