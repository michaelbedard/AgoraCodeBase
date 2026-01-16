import {
    Client,
    Events,
    MessageFlags,
    ButtonBuilder,
    ButtonStyle,
    ActionRowBuilder,
    TextChannel
} from "discord.js";
import {createTicketPanel} from "../../stores/ticketButtonStore";
import {sendSettingsMenu} from "../../utils/sendSettingsMenu";

// --- DRAFT STORE ---
const draftStore = new Map<string, any>();

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: Client) {

        // ====================================================
        // 1. HANDLE MODAL SUBMIT (Content Entry)
        // ====================================================
        if (interaction.isModalSubmit() && interaction.customId.startsWith('addTicketButtonForm_')) {
            const parts = interaction.customId.split('_');
            const targetMessageId = parts[1];
            const buttonLabel = parts[2];
            const buttonEmoji = parts[3];
            const buttonStyleString = parts[4];

            const title = interaction.fields.getTextInputValue("messageTitle");
            const description = interaction.fields.getTextInputValue("messageDescription");
            const footer = interaction.fields.getTextInputValue("messageFooter");
            const banner = interaction.fields.getTextInputValue("messageBanner");
            const thumbnail = interaction.fields.getTextInputValue("messageThumbnail");

            draftStore.set(interaction.user.id, {
                targetMessageId,
                buttonLabel,
                buttonEmoji,
                buttonStyleString,
                title,
                description,
                footer,
                banner,
                thumbnail,
                color: "#3498db", // Default Blue
                pingRoles: []
            })

            await sendSettingsMenu(interaction, "ticket", draftStore.get(interaction.user.id), false, {
                showChannel: false,
                showRoles: true,
                showFrequency: false,
                showColor: true
            });
            return;
        }

        // ====================================================
        // 2. HANDLE SETTINGS (Dropdowns)
        // ====================================================
        if (interaction.isAnySelectMenu() && interaction.customId.startsWith('ticket_setup_')) {
            const draft = draftStore.get(interaction.user.id);
            if (!draft) return interaction.reply({ content: "❌ Session expired.", flags: MessageFlags.Ephemeral });

            // Update Draft based on selection
            switch (interaction.customId) {
                case 'ticket_setup_roles':
                    draft.pingRoles = interaction.values;
                    break;
                case 'ticket_setup_color':
                    draft.color = interaction.values[0];
                    break;
            }

            draftStore.set(interaction.user.id, draft);
            await sendSettingsMenu(interaction, "ticket", draft, true, {
                showChannel: false,
                showRoles: true,
                showFrequency: false,
                showColor: true
            });
            return;
        }

        // ====================================================
        // 3. HANDLE SEND BUTTON (Finalize)
        // ====================================================
        if (interaction.isButton() && interaction.customId === "ticket_send") {
            const draft = draftStore.get(interaction.user.id);
            if (!draft) return interaction.reply({content: "❌ Session expired.", flags: MessageFlags.Ephemeral});

            await interaction.deferReply({flags: MessageFlags.Ephemeral});

            let buttonStyle = ButtonStyle.Primary;
            switch (draft.buttonStyleString.toLowerCase()) {
                case 'secondary':
                    buttonStyle = ButtonStyle.Secondary;
                    break;
                case 'success':
                    buttonStyle = ButtonStyle.Success;
                    break;
                case 'danger':
                    buttonStyle = ButtonStyle.Danger;
                    break;
                default:
                    buttonStyle = ButtonStyle.Primary; // Default to Primary
            }

            const targetMessage = await (interaction.channel as TextChannel).messages.fetch(draft.targetMessageId);
            if (!targetMessage) {
                return interaction.editReply({content: "Error: Message not found in current channel.  Make sure you are in the same channel as the message."});
            }

            // store message corresponding to this button
            const id = crypto.randomUUID();
            await createTicketPanel({
                id: id,
                title: draft.title,
                description: draft.description,
                footer: draft.footer,
                banner: draft.banner,
                thumbnail: draft.thumbnail,
                color: draft.color,
                pingRoles: draft.pingRoles
            })

            // Build the button
            const createTicketButton = new ButtonBuilder()
                .setCustomId(`createTicketButton_${id}`)
                .setLabel(draft.buttonLabel)
                .setStyle(buttonStyle);

            if (draft.buttonEmoji && draft.buttonEmoji !== 'null' && draft.buttonEmoji !== 'undefined') {
                createTicketButton.setEmoji(draft.buttonEmoji);
            }

            // Get existing components or create a new row
            let components = targetMessage.components.map((row: any) => ActionRowBuilder.from(row));
            let lastRow = components[components.length - 1];

            if (!lastRow || lastRow.components.length === 5) {
                // Create a new row if the last one is full (max 5 components)
                lastRow = new ActionRowBuilder<ButtonBuilder>();
                components.push(lastRow);
            }

            lastRow.addComponents(createTicketButton);

            // EDIT the target message to add the button
            await targetMessage.edit({
                components: components as ActionRowBuilder<ButtonBuilder>[]
            });

            await interaction.editReply({
                content: `✅ Success! The ticket creation button (**${draft.buttonLabel}**) has been added to the message.`
            });
        }
    }
};