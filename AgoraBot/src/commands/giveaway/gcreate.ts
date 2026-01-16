import {ModalBuilder, SlashCommandBuilder, TextInputStyle, TextInputBuilder, ActionRowBuilder} from "discord.js";

export const data = new SlashCommandBuilder()
    .setName('gcreate')
    .setDescription('Creates a giveaway');

export async function execute(interaction: any) {

    await showGiveawayCreationModal(interaction);
}

export async function showGiveawayCreationModal(interaction: any) {
    const modal = new ModalBuilder()
        .setCustomId("giveawayCreateForm")
        .setTitle("Create a Giveaway 🎉");

    // Create form fields
    const titleInput = new TextInputBuilder()
        .setCustomId("giveawayTitle")
        .setLabel("Giveaway Title")
        .setPlaceholder("Ex: Free Steam Game Key")
        .setStyle(TextInputStyle.Short)
        .setRequired(true);

    const entriesInput = new TextInputBuilder()
        .setCustomId("giveawayEntries")
        .setLabel("Number of winners")
        .setStyle(TextInputStyle.Short)
        .setValue("1")
        .setRequired(true);

    // Add inputs to the modal (each input needs its own ActionRow)
    const firstRow = new ActionRowBuilder<TextInputBuilder>().addComponents(titleInput);
    const secondRow = new ActionRowBuilder<TextInputBuilder>().addComponents(entriesInput);

    modal.addComponents(firstRow, secondRow);

    // Show modal to user
    await interaction.showModal(modal);
}