import {
    Client,
    Events,
    ButtonBuilder,
    ButtonStyle,
    ActionRowBuilder,
    TextChannel
} from "discord.js";
import {ticketMessages} from "../../stores/ticketButtonStore";

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: Client) {
        if (!interaction.isModalSubmit()) return;
        if (!interaction.customId.startsWith('createGiveawayButton')) return;

        // await interaction.deferReply({flags: MessageFlags.Ephemeral});

        const parts = interaction.customId.split('_');
        const messageId = parts[1];
        const buttonLabel = parts[2];
        const buttonEmoji = parts[3];
        const buttonStyleString = parts[4];

        let buttonStyle = ButtonStyle.Primary;
        switch (buttonStyleString.toLowerCase()) {
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

        const targetMessage = await (interaction.channel as TextChannel).messages.fetch(messageId);
        if (!targetMessage) {
            return interaction.editReply({content: "Error: Message not found in current channel.  Make sure you are in the same channel as the message."});
        }

        // Build the button
        const createTicketButton = new ButtonBuilder()
            .setCustomId("createGiveawayButton")
            .setLabel(buttonLabel)
            .setStyle(buttonStyle);

        if (buttonEmoji && buttonEmoji !== 'null' && buttonEmoji !== 'undefined') {
            createTicketButton.setEmoji(buttonEmoji);
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

        await interaction.reply({
            content: `✅ Success! The ticket creation button (**${buttonLabel}**) has been added to the message.`
        });
    }
};