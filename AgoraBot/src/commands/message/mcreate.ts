import {
    ModalBuilder,
    SlashCommandBuilder,
    TextInputStyle,
    TextInputBuilder,
    ActionRowBuilder
} from "discord.js";

export const data = new SlashCommandBuilder()
    .setName('mcreate')
    .setDescription('Open the message creator wizard');

export async function execute(interaction: any) {
    const modal = new ModalBuilder()
        .setCustomId(`mcreate_modal`)
        .setTitle(`📝 Create Embedded Message`);

    // --- Row 1: Title ---
    const titleInput = new TextInputBuilder()
        .setCustomId("messageTitle")
        .setLabel("Title")
        .setPlaceholder("Ex: Server Maintenance")
        .setStyle(TextInputStyle.Short)
        .setRequired(true);

    // --- Row 2: Description ---
    const descInput = new TextInputBuilder()
        .setCustomId("messageDescription")
        .setLabel("Description")
        .setPlaceholder("Write your content here...")
        .setStyle(TextInputStyle.Paragraph)
        .setRequired(true);

    // --- Row 3: Footer ---
    const footerInput = new TextInputBuilder()
        .setCustomId("messageFooter")
        .setLabel("Footer (Optional)")
        .setPlaceholder("Ex: Management Team")
        .setStyle(TextInputStyle.Short)
        .setRequired(false);

    // --- Row 4: Banner Image ---
    const bannerInput = new TextInputBuilder()
        .setCustomId("messageBanner")
        .setLabel("Banner Image URL (Optional)")
        .setPlaceholder("https://... (Big Image at bottom)")
        .setStyle(TextInputStyle.Short)
        .setRequired(false);

    // --- Row 5: Thumbnail Image ---
    const thumbInput = new TextInputBuilder()
        .setCustomId("messageThumbnail")
        .setLabel("Thumbnail URL (Optional)")
        .setPlaceholder("https://... (Small Image at top-right)")
        .setStyle(TextInputStyle.Short)
        .setRequired(false);

    modal.addComponents(
        new ActionRowBuilder<TextInputBuilder>().addComponents(titleInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(descInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(footerInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(bannerInput),
        new ActionRowBuilder<TextInputBuilder>().addComponents(thumbInput)
    );

    await interaction.showModal(modal);
}