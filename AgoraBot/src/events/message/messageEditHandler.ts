import {
    Client,
    EmbedBuilder,
    Events,
    MessageFlags,
    resolveColor
} from "discord.js";
import {
    recurrentMessageStore,
    updateRecurrentMessage,
    deleteRecurrentMessage,
    RecurrentMessage
} from "../../stores/recurrentMessageStore";
import { sendSettingsMenu } from "../../utils/sendSettingsMenu";
import { parseDuration } from "../../utils/parseduration";

// --- DRAFT STORE FOR EDITS ---
// Key: UserId, Value: Draft Object + Original Message ID
const editDraftStore = new Map<string, any>();

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: Client) {

        // ====================================================
        // 1. HANDLE MODAL SUBMIT (Content Entry from /medit)
        // ====================================================
        if (interaction.isModalSubmit()) {

            // A. Handle DB-Linked Edit (embeddedMessageEditForm_MESSAGEID)
            if (interaction.customId.startsWith('embeddedMessageEditForm_')) {
                const messageId = interaction.customId.split('_')[1];
                const originalMessage = recurrentMessageStore.get(messageId);

                if (!originalMessage) {
                    return await interaction.reply({
                        content: `❌ Original message with ID ${messageId} not found in database.`,
                        flags: MessageFlags.Ephemeral
                    });
                }

                // Create Draft from Inputs + Original Settings
                const draft = {
                    originalId: messageId,
                    title: interaction.fields.getTextInputValue("messageTitle"),
                    description: interaction.fields.getTextInputValue("messageDescription"),
                    footer: interaction.fields.getTextInputValue("messageFooter"),
                    banner: interaction.fields.getTextInputValue("messageBanner"),
                    thumbnail: interaction.fields.getTextInputValue("messageThumbnail"),

                    // Inherit these from original unless changed via menu later
                    color: originalMessage.color,
                    channelId: originalMessage.channelId,
                    pingRoles: originalMessage.ping || [],
                    frequency: originalMessage.frequency
                };

                editDraftStore.set(interaction.user.id, draft);
                await sendSettingsMenu(interaction, "medit", draft);
                return;
            }

            // B. Handle Direct Edit (directMessageEdit_CHANNELID_MESSAGEID)
            if (interaction.customId.startsWith('directMessageEdit_')) {
                const parts = interaction.customId.split('_');
                const channelId = parts[1];
                const messageId = parts[2];

                // 1. FETCH THE MESSAGE to get existing roles/color
                // We need this because the Modal doesn't contain Role info
                let existingRoles: string[] = [];
                let existingColor = "#3498db"; // Default fallback

                try {
                    const channel = await client.channels.fetch(channelId);
                    if (channel && channel.isTextBased()) {
                        const msg = await channel.messages.fetch(messageId);

                        // Extract Roles (Mentions)
                        existingRoles = Array.from(msg.mentions.roles.keys());

                        // Extract Color (from Embed)
                        if (msg.embeds[0] && msg.embeds[0].color) {
                            existingColor = `#${msg.embeds[0].color.toString(16).padStart(6, '0')}`;
                        }
                    }
                } catch (e) {
                    console.error("Could not fetch original message details for draft:", e);
                }

                // Create Draft from Inputs (Defaulting settings for one-time)
                const draft = {
                    originalId: messageId,
                    title: interaction.fields.getTextInputValue("messageTitle"),
                    description: interaction.fields.getTextInputValue("messageDescription"),
                    footer: interaction.fields.getTextInputValue("messageFooter"),
                    banner: interaction.fields.getTextInputValue("messageBanner"),
                    thumbnail: interaction.fields.getTextInputValue("messageThumbnail"),

                    color: existingColor,
                    channelId: channelId,
                    pingRoles: existingRoles,
                    frequency: "0s" // Default to one-time
                };

                editDraftStore.set(interaction.user.id, draft);

                // Show menu.
                // Note: Changing channel/frequency on a direct edit effectively converts it to a recurrent message or moves it.
                await sendSettingsMenu(interaction, "medit", draft, false, {
                    showChannel: false, showRoles: true, showFrequency: false, showColor: true
                });
                return;
            }
        }

        // ====================================================
        // 2. HANDLE SETTINGS (Dropdowns for Edit Menu)
        // ====================================================
        if (interaction.isAnySelectMenu() && interaction.customId.startsWith('medit_setup_')) {
            const draft = editDraftStore.get(interaction.user.id);
            if (!draft) return interaction.reply({ content: "❌ Session expired. Please run /medit again.", flags: MessageFlags.Ephemeral });

            switch (interaction.customId) {
                case 'medit_setup_channel':
                    draft.channelId = interaction.values[0];
                    break;
                case 'medit_setup_roles':
                    draft.pingRoles = interaction.values;
                    break;
                case 'medit_setup_freq':
                    draft.frequency = interaction.values[0];
                    break;
                case 'medit_setup_color':
                    draft.color = interaction.values[0];
                    break;
            }

            editDraftStore.set(interaction.user.id, draft);

            const frequencyMs = parseDuration(draft.frequency) || 0;
            if (frequencyMs > 0) {
                await sendSettingsMenu(interaction, "medit", draft, true);
            } else {
                await sendSettingsMenu(interaction, "medit", draft, true, {
                    showChannel: false, showRoles: true, showFrequency: false, showColor: true
                });
            }
            return;
        }

        // ====================================================
        // 3. HANDLE UPDATE BUTTON (Finalize Edit)
        // ====================================================
        if (interaction.isButton() && interaction.customId === "medit_send") {
            const draft = editDraftStore.get(interaction.user.id);
            if (!draft) return interaction.reply({ content: "❌ Session expired.", flags: MessageFlags.Ephemeral });

            const frequencyMs = parseDuration(draft.frequency) || 0;

            // --- SCENARIO A: It was Recurrent, and stays/is updated as Recurrent ---
            if (frequencyMs > 0) {

                const messageData: RecurrentMessage = {
                    id: draft.originalId, // Keep ID
                    title: draft.title,
                    description: draft.description,
                    footer: draft.footer,
                    banner: draft.banner,
                    thumbnail: draft.thumbnail,
                    color: draft.color,
                    frequency: draft.frequency,
                    frequencyMs: frequencyMs,
                    channelId: draft.channelId,
                    ping: draft.pingRoles,
                    lastSent: Date.now() // Reset timer on edit
                };

                await updateRecurrentMessage(draft.originalId, messageData);

                await interaction.reply({
                    content: `✅ **Recurrent Message Updated!**\nChannel: <#${draft.channelId}>\nFrequency: \`${draft.frequency}\``,
                    flags: MessageFlags.Ephemeral
                });

            }

            // --- SCENARIO B: It is One-Time (0s) ---
            else {
                // If it WAS recurrent before, user effectively "cancelled" the schedule
                if (draft.isRecurrent) {
                    await deleteRecurrentMessage(draft.originalId);
                }

                // Edit the Discord Message Directly
                try {
                    const channel = await client.channels.fetch(draft.channelId);
                    if (channel && channel.isTextBased()) {
                        const discordMessage = await channel.messages.fetch(draft.originalId);

                        const newEmbed = new EmbedBuilder()
                            .setTitle(draft.title)
                            .setDescription(draft.description)
                            .setFooter(draft.footer ? { text: draft.footer } : null);

                        if (draft.banner) newEmbed.setImage(draft.banner);
                        if (draft.thumbnail) newEmbed.setThumbnail(draft.thumbnail);

                        let finalColor = "#3498db";
                        try {
                            if (draft.color) {
                                resolveColor(draft.color as any);
                                finalColor = draft.color;
                            }
                        } catch (e) {}
                        newEmbed.setColor(finalColor as any);

                        const pingContent = draft.pingRoles && draft.pingRoles.length > 0
                            ? draft.pingRoles.map((id: string) => `<@&${id}>`).join(' ')
                            : ""; // Sending empty string removes previous pings

                        await discordMessage.edit({
                            content: pingContent,
                            embeds: [newEmbed]
                        });

                        await interaction.reply({
                            content: `✅ **Message Edited!** (Not scheduled)`,
                            flags: MessageFlags.Ephemeral
                        });
                    }
                } catch (error) {
                    console.error("Failed to edit Discord message:", error);
                    await interaction.reply({
                        content: `⚠️ Database updated (if applicable), but could not edit the message on Discord. It may have been deleted or moved.`,
                        flags: MessageFlags.Ephemeral
                    });
                }
            }

            // Cleanup
            editDraftStore.delete(interaction.user.id);
        }
    }
}