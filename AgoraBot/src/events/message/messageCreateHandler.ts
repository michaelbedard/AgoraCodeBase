import {
    Events,
    MessageFlags,
} from "discord.js";
import { parseDuration } from "../../utils/parseduration";
import {createRecurrentMessage, RecurrentMessage} from "../../stores/recurrentMessageStore";
import { displayEmbeddedMessage } from "../../utils/displayEmbeddedMessage";
import {sendSettingsMenu} from "../../utils/sendSettingsMenu";

// --- DRAFT STORE ---
const draftStore = new Map<string, any>();

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: any) {

        // ====================================================
        // 1. HANDLE MODAL SUBMIT (Content Entry)
        // ====================================================
        if (interaction.isModalSubmit() && interaction.customId === "mcreate_modal") {
            const title = interaction.fields.getTextInputValue("messageTitle");
            const description = interaction.fields.getTextInputValue("messageDescription");
            const footer = interaction.fields.getTextInputValue("messageFooter");
            const banner = interaction.fields.getTextInputValue("messageBanner");
            const thumbnail = interaction.fields.getTextInputValue("messageThumbnail");

            // Save Initial Draft
            draftStore.set(interaction.user.id, {
                title, description, footer, banner, thumbnail,
                color: "#3498db", // Default Blue
                frequency: "0s",  // Default One-time
                channelId: interaction.channelId,
                pingRoles: []     // Default No pings
            });

            await sendSettingsMenu(interaction, "mcreate", draftStore.get(interaction.user.id));
            return;
        }

        // ====================================================
        // 2. HANDLE SETTINGS (Dropdowns)
        // ====================================================
        if (interaction.isAnySelectMenu() && interaction.customId.startsWith('mcreate_setup_')) {
            const draft = draftStore.get(interaction.user.id);
            if (!draft) return interaction.reply({ content: "❌ Session expired.", flags: MessageFlags.Ephemeral });

            // Update Draft based on selection
            switch (interaction.customId) {
                case 'mcreate_setup_channel':
                    draft.channelId = interaction.values[0];
                    break;
                case 'mcreate_setup_roles':
                    draft.pingRoles = interaction.values;
                    break;
                case 'mcreate_setup_freq':
                    draft.frequency = interaction.values[0];
                    break;
                case 'mcreate_setup_color':
                    draft.color = interaction.values[0];
                    break;
            }

            draftStore.set(interaction.user.id, draft);
            await sendSettingsMenu(interaction, "mcreate", draft, true);
            return;
        }

        // ====================================================
        // 3. HANDLE SEND BUTTON (Finalize)
        // ====================================================
        if (interaction.isButton() && interaction.customId === "mcreate_send") {
            const draft = draftStore.get(interaction.user.id);
            if (!draft) return interaction.reply({ content: "❌ Session expired.", flags: MessageFlags.Ephemeral });

            const frequencyMs = parseDuration(draft.frequency) || 0;

            // 1. Construct the final message object
            const message: RecurrentMessage = {
                id: "", // generated later
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
                lastSent: Date.now()
            };

            // 2. Send the message immediately
            const sentId = await displayEmbeddedMessage(
                client,
                message,
            );

            message.id = sentId;

            // 3. Save to store if recurring
            if (frequencyMs > 0) {
                await createRecurrentMessage(message);
                await interaction.reply({
                    content: `✅ **Recurring Message Scheduled!**\nChannel: <#${draft.channelId}>\nFrequency: \`${draft.frequency}\``,
                    flags: MessageFlags.Ephemeral
                });
            } else {
                await interaction.reply({
                    content: `✅ **Message Sent!** Check <#${draft.channelId}>`,
                    flags: MessageFlags.Ephemeral
                });
            }

            // Clear draft
            draftStore.delete(interaction.user.id);
        }
    }
};