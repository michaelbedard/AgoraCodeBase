import {
    SlashCommandBuilder,
    MessageFlags,
    ChannelType,
    PermissionFlagsBits,
    EmbedBuilder,
    ActionRowBuilder,
    ButtonBuilder,
    ButtonStyle,
    VoiceChannel,
    InviteTargetType, GuildMember
} from "discord.js";
import {games} from "../../stores/GameStore";

export default {
    data: new SlashCommandBuilder()
        .setName("play")
        .setDescription("Start a voice activity (game) and create a temporary channel")
        .addStringOption(option =>
            option
                .setName("game")
                .setDescription("Which game do you want to play?")
                .setRequired(true)
                .addChoices(...games.map(g => ({ name: g.title, value: g.id })))
        ),

    async execute(interaction: any) {
        const gameId = interaction.options.getString('game');
        const game = games.get(gameId);

        if (!game) {
            return interaction.reply({ content: '❌ Game not found.', ephemeral: true });
        }

        // Use the channel where the command was sent
        const lobbyChannel = interaction.channel;

        if (!lobbyChannel || !lobbyChannel.isTextBased()) {
            return await interaction.reply({
                content: "❌ This command must be used in a text-based channel.",
                flags: MessageFlags.Ephemeral,
            });
        }

        await interaction.deferReply({ flags: MessageFlags.Ephemeral });

        try {
            // 2. Create the Temporary Voice Channel
            const gameChannel = await interaction.guild.channels.create({
                name: `🎮・${game.title}`,
                type: ChannelType.GuildVoice,
                parent: "1457015111939260436",
                permissionOverwrites: [
                    {
                        id: interaction.guild.id,
                        allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.Connect],
                    },
                ],
            }) as VoiceChannel;

            // 3. Create the Activity Invite (The "Magic Link")
            const invite = await gameChannel.createInvite({
                targetApplication: "1456389795264729150",
                targetType: InviteTargetType.EmbeddedApplication,
                maxAge: 3600, // 1 hour
                maxUses: 0,   // Unlimited
            });

            // 4. Send Public Announcement to the Lobby
            const lobbyEmbed = new EmbedBuilder()
                .setTitle(`🎮 New ${game.title} Game!`)
                .setDescription(`<@${interaction.user.id}> has started a game of **${game.title}**!`)
                .setColor("#5865F2")
                .setThumbnail(interaction.user.displayAvatarURL());

            const joinButton = new ButtonBuilder()
                .setLabel(`Join ${game.title}`)
                .setStyle(ButtonStyle.Link)
                .setURL(invite.url); // Clicking this joins Voice + Starts Game

            const row = new ActionRowBuilder<ButtonBuilder>().addComponents(joinButton);

            await lobbyChannel.send({
                embeds: [lobbyEmbed],
                components: [row]
            });

            // 5. Reply to the Creator (Private Button)
            await interaction.editReply({
                content: `✅ **Game Created!**\nClick the button below to join your new game room.`,
                components: [row]
            });

        } catch (error: any) {
            console.error(error);
            await interaction.editReply({
                content: "❌ Something went wrong creating the game channel.",
            });
        }
    },
};