import {
    SlashCommandBuilder,
    ButtonStyle
} from "discord.js";
import {buildEditModal} from "../../../utils/buildMessageModal";

const COLOR_CHOICES = [
    { name: 'Primary (Blue)', value: ButtonStyle.Primary.toString() },
    { name: 'Secondary (Gray)', value: ButtonStyle.Secondary.toString() },
    { name: 'Success (Green)', value: ButtonStyle.Success.toString() },
    { name: 'Danger (Red)', value: ButtonStyle.Danger.toString() },
];

export const data = new SlashCommandBuilder()
    .setName('maddticketbutton')
    .setDescription('Add a button that creates a ticket to an existing message')
    .addStringOption(option =>
        option.setName('message_id')
            .setDescription('Id of the message to reply to')
            .setRequired(true)
    )
    .addStringOption(option =>
        option.setName('label')
            .setDescription('Label displayed on the button')
            .setRequired(true)
    )
    .addStringOption(option =>
        option.setName('emoji')
            .setDescription('An emoji to display on the button (e.g., 🎫 or a custom ID)')
            .setRequired(false)
    )
    .addStringOption(option =>
        option.setName('style')
            .setDescription('Style (Color) of the button')
            .setRequired(false)
            .addChoices(...COLOR_CHOICES)
    );

export async function execute(interaction: any) {

    const messageId = interaction.options.getString('message_id');
    const buttonLabel = interaction.options.getString('label');
    const buttonEmoji = interaction.options.getString('emoji');
    const buttonStyle = interaction.options.getString('style') || "Primary";

    const modal = buildEditModal(
        `addTicketButtonForm_${messageId}_${buttonLabel}_${buttonEmoji}_${buttonStyle}`,
        `Create message for ticket salon`,
        {
            title: "",
            description: "",
            footer: "",
            banner: "",
            thumbnail: ""
        }
    );

    // Show modal to user
    await interaction.showModal(modal);
}