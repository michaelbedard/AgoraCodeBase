import {
    ActionRowBuilder,
    ModalBuilder,
    TextInputBuilder,
    TextInputStyle,
} from "discord.js";

/**
 * Shared helper to build the Modal UI.
 * Used by both Recurrent and Direct edit functions to ensure consistent UI.
 */
export function buildEditModal(customId: string, modalTitle: string, initialData: {
    title: string,
    description: string,
    footer: string,
    banner: string,
    thumbnail: string
}) {
    const modal = new ModalBuilder()
        .setCustomId(customId)
        .setTitle(modalTitle);

    const titleInput = new TextInputBuilder()
        .setCustomId("messageTitle")
        .setLabel("Title")
        .setStyle(TextInputStyle.Short)
        .setValue(initialData.title)
        .setRequired(true);

    const descriptionInput = new TextInputBuilder()
        .setCustomId("messageDescription")
        .setLabel("Description")
        .setStyle(TextInputStyle.Paragraph)
        .setValue(initialData.description)
        .setRequired(true);

    const footerInput = new TextInputBuilder()
        .setCustomId("messageFooter")
        .setLabel("Footer (optional)")
        .setStyle(TextInputStyle.Short)
        .setValue(initialData.footer)
        .setRequired(false);

    const bannerInput = new TextInputBuilder()
        .setCustomId("messageBanner")
        .setLabel("Banner Image URL (Optional)")
        .setPlaceholder("https://... (Big Image at bottom)")
        .setStyle(TextInputStyle.Short)
        .setValue(initialData.banner)
        .setRequired(false);

    const thumbInput = new TextInputBuilder()
        .setCustomId("messageThumbnail")
        .setLabel("Thumbnail URL (Optional)")
        .setPlaceholder("https://... (Small Image at top-right)")
        .setStyle(TextInputStyle.Short)
        .setValue(initialData.thumbnail)
        .setRequired(false);

    modal.addComponents(
        new ActionRowBuilder<TextInputBuilder>().addComponents(titleInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(descriptionInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(footerInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(bannerInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(thumbInput)
    );

    return modal;
}