import {ButtonInteraction, CommandInteraction, MessageFlags, SlashCommandBuilder} from "discord.js";
import {getRecurrentMessageStore, RecurrentMessage} from "../../stores/recurrentMessageStore";
import {buildEditModal} from "../../utils/buildMessageModal";

/**
 * HELPER A: Handle editing a message that exists in your Database.
 */
export async function handleRecurrentEdit(interaction: CommandInteraction | ButtonInteraction, storedMessage: RecurrentMessage) {
    console.log(`found in store: ${storedMessage.id}`);

    const modal = buildEditModal(
        `embeddedMessageEditForm_${storedMessage.id}`,
        `Edit DB Message`,
        {
            title: storedMessage.title,
            description: storedMessage.description,
            footer: storedMessage.footer || "",
            banner: storedMessage.banner || "",
            thumbnail: storedMessage.thumbnail || ""
        }
    );

    await interaction.showModal(modal);
}

/**
 * HELPER B: Handle editing a raw Discord message directly (not in DB).
 */
export async function handleDirectEdit(interaction: any, messageId: string) {
    const channel = interaction.channel;

    // Safety check: can only fetch messages in text-based channels
    if (!channel || !channel.isTextBased()) {
        return await interaction.reply({
            content: "❌ Message not found in database, and cannot fetch raw messages in this channel type.",
            flags: MessageFlags.Ephemeral,
        });
    }

    try {
        const targetMessage = await channel.messages.fetch(messageId);
        const embed = targetMessage.embeds[0];

        if (!embed) {
            return await interaction.reply({
                content: "❌ Found the message, but it has no Embed to edit.",
                flags: MessageFlags.Ephemeral,
            });
        }

        // We pass CHANNEL_ID and MESSAGE_ID so the handler knows where to look
        const modal = buildEditModal(
            `directMessageEdit_${channel.id}_${messageId}`,
            `Edit Direct Message`,
            {
                title: embed.title || "",
                description: embed.description || "",
                footer: embed.footer?.text || "",
                banner: embed.image?.url || "",
                thumbnail: embed.thumbnail?.url || ""
            }
        );

        await interaction.showModal(modal);

    } catch (error) {
        // If fetch fails, it means it's truly gone
        return await interaction.reply({
            content: "❌ Message not found in Database OR in this channel.",
            flags: MessageFlags.Ephemeral,
        });
    }
}

export default {
    data: new SlashCommandBuilder()
        .setName("medit")
        .setDescription("Edit a message (Database or Direct)")
        .addStringOption((option) =>
            option
                .setName("id")
                .setDescription("The ID of the message to edit")
                .setRequired(true)
        ),

    async execute(interaction: any) {
        const messageId = interaction.options.getString("id");

        // Use the safe getter we created earlier
        const store = getRecurrentMessageStore();
        const storedMessage = store ? store.get(messageId) : undefined;

        if (storedMessage) {
            // PATH A: It is in the database
            await handleRecurrentEdit(interaction, storedMessage);
        } else {
            // PATH B: It is NOT in the database (try direct fetch)
            await handleDirectEdit(interaction, messageId);
        }
    },
};