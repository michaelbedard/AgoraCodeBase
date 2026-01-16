import {
    ModalBuilder,
    SlashCommandBuilder,
    TextInputStyle,
    TextInputBuilder,
    ActionRowBuilder,
    ChannelType, ButtonStyle, TextChannel, ButtonBuilder, MessageFlags
} from "discord.js";

const COLOR_CHOICES = [
    { name: 'Primary (Blue)', value: ButtonStyle.Primary.toString() },
    { name: 'Secondary (Gray)', value: ButtonStyle.Secondary.toString() },
    { name: 'Success (Green)', value: ButtonStyle.Success.toString() },
    { name: 'Danger (Red)', value: ButtonStyle.Danger.toString() },
];

export const data = new SlashCommandBuilder()
    .setName('maddgiveawaybutton')
    .setDescription('Add a button that creates a giveaway')
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

    const targetMessage = await (interaction.channel as TextChannel).messages.fetch(messageId);
    if (!targetMessage) {
        return interaction.reply({
            content: "Error: Message not found in current channel.  Make sure you are in the same channel as the message.",
            flags: MessageFlags.Ephemeral,
        });
    }

    // Build the button
    const createGiveawayButton = new ButtonBuilder()
        .setCustomId(`createGiveawayButton`)
        .setLabel(buttonLabel)
        .setStyle(buttonStyle);

    if (buttonEmoji && buttonEmoji !== 'null' && buttonEmoji !== 'undefined') {
        createGiveawayButton.setEmoji(buttonEmoji);
    }

    // Get existing components or create a new row
    let components = targetMessage.components.map((row: any) => ActionRowBuilder.from(row));
    let lastRow = components[components.length - 1];

    if (!lastRow || lastRow.components.length === 5) {
        // Create a new row if the last one is full (max 5 components)
        lastRow = new ActionRowBuilder<ButtonBuilder>();
        components.push(lastRow);
    }

    lastRow.addComponents(createGiveawayButton);

    // EDIT the target message to add the button
    await targetMessage.edit({
        components: components as ActionRowBuilder<ButtonBuilder>[]
    });

    await interaction.reply({
        content: `✅ Success! The giveaway creation button (**${buttonLabel}**) has been added to the message.`,
        flags: MessageFlags.Ephemeral,
    });
}