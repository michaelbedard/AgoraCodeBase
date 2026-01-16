import {
    SlashCommandBuilder,
    TextChannel,
    MessageFlags,
    PermissionFlagsBits,
    Role,
    parseEmoji
} from "discord.js";
import {addReactionRole, reactionRoleMap} from "../../../stores/reactionRoleMap";

export const data = new SlashCommandBuilder()
    .setName('maddreactionrole')
    .setDescription('Setup a reaction role on a specific message')
    .setDefaultMemberPermissions(PermissionFlagsBits.ManageRoles)
    .addStringOption(option =>
        option.setName('message_id')
            .setDescription('Id of the message to react to')
            .setRequired(true)
    )
    .addRoleOption(option =>
        option.setName('role')
            .setDescription('The role to assign')
            .setRequired(true)
    )
    .addStringOption(option =>
        option.setName('emoji')
            .setDescription('The emoji users should react with')
            .setRequired(true)
    );

export async function execute(interaction: any) {
    const messageId = interaction.options.getString('message_id');
    const role = interaction.options.getRole('role') as Role;
    const emojiInput = interaction.options.getString('emoji');

    // 1. Fetch the message
    let targetMessage;
    try {
        targetMessage = await (interaction.channel as TextChannel).messages.fetch(messageId);
    } catch (e) {
        return interaction.reply({
            content: "❌ Message not found. Ensure you are in the same channel.",
            flags: MessageFlags.Ephemeral,
        });
    }

    // 2. Validate Emoji and React
    // We try to react. If it fails, the emoji is invalid or bot lacks permissions.
    try {
        await targetMessage.react(emojiInput);
    } catch (error) {
        return interaction.reply({
            content: "❌ I cannot use that emoji. Ensure it is a standard emoji or a custom emoji from THIS server.",
            flags: MessageFlags.Ephemeral,
        });
    }

    // 3. Calculate the unique key for storage
    // We need to know if it's a Custom Emoji (ID) or Standard (Name)
    const parsed = parseEmoji(emojiInput);
    const emojiKey = parsed?.id ? parsed.id : parsed?.name;

    if (!emojiKey) {
        return interaction.reply({ content: "❌ Could not parse emoji.", flags: MessageFlags.Ephemeral });
    }

    // 4. Save to our Store
    await addReactionRole(messageId, emojiKey, role.id)

    await interaction.reply({
        content: `✅ **Reaction Role Setup!**\nMessage: [Link](${targetMessage.url})\nEmoji: ${emojiInput}\nRole: ${role.name}\n\n`,
        flags: MessageFlags.Ephemeral,
    });
}