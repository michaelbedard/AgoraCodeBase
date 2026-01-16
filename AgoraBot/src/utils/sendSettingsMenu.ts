import {
    EmbedBuilder,
    ActionRowBuilder,
    ButtonBuilder,
    ButtonStyle,
    StringSelectMenuBuilder,
    ChannelSelectMenuBuilder,
    RoleSelectMenuBuilder,
    ChannelType,
    MessageFlags
} from "discord.js";

// Interface for the toggles
interface MenuOptions {
    showChannel?: boolean;
    showRoles?: boolean;
    showFrequency?: boolean;
    showColor?: boolean;
}

export async function sendSettingsMenu(
    interaction: any,
    type: string,
    draft: any,
    isUpdate = false,
    { showChannel = true, showRoles = true, showFrequency = true, showColor = true }: MenuOptions = {}
) {
    // 1. Build Preview Embed
    const previewEmbed = new EmbedBuilder()
        .setTitle(draft.title)
        .setDescription(draft.description)
        .setColor(draft.color)
        .setFooter(draft.footer ? { text: draft.footer } : null);

    if (draft.banner) previewEmbed.setImage(draft.banner);
    if (draft.thumbnail) previewEmbed.setThumbnail(draft.thumbnail);

    // 2. Build Dynamic Description for Status Embed
    let statusDescription = "";

    if (showChannel) {
        statusDescription += `**Channel:** <#${draft.channelId}>\n`;
    }
    if (showFrequency) {
        let freqLabel = draft.frequency;

        switch (draft.frequency) {
            case "0s": freqLabel = "One-time (None)"; break;
            case "on_enter": freqLabel = "On Member Join"; break;
            case "on_exit": freqLabel = "On Member Leave"; break;
            default: freqLabel = draft.frequency; // Falls back to 10m, 1h, etc.
        }

        statusDescription += `**Frequency:** ${freqLabel}\n`;
    }
    if (showRoles) {
        const pingText = draft.pingRoles.length > 0
            ? draft.pingRoles.map((r: string) => `<@&${r}>`).join(" ")
            : "None";
        statusDescription += `**Pings:** ${pingText}\n`;
    }

    // Fallback if everything is disabled
    if (statusDescription === "") statusDescription = "Review the preview above and click Send.";

    // 3. Build Status Embed
    const statusEmbed = new EmbedBuilder()
        .setTitle("🛠️ Configure Message Settings")
        .setDescription(statusDescription)
        .setColor(0x2b2d31); // Dark/Invisible

    // 4. Build Components Array Dynamically
    const rows: ActionRowBuilder<any>[] = [];

    // --- Row: Channel ---
    if (showChannel) {
        const channelSelect = new ChannelSelectMenuBuilder()
            .setCustomId(`${type}_setup_channel`)
            .setPlaceholder('📢 Where should this go?')
            .setChannelTypes(ChannelType.GuildText, ChannelType.GuildAnnouncement);

        rows.push(new ActionRowBuilder().addComponents(channelSelect));
    }

    // --- Row: Roles (Pings) ---
    if (showRoles) {
        const roleSelect = new RoleSelectMenuBuilder()
            .setCustomId(`${type}_setup_roles`)
            .setPlaceholder('🔔 Who to ping? (Optional)')
            .setMinValues(0)
            .setMaxValues(10);

        rows.push(new ActionRowBuilder().addComponents(roleSelect));
    }

    // --- Row: Frequency ---
    if (showFrequency) {
        const freqSelect = new StringSelectMenuBuilder()
            .setCustomId(`${type}_setup_freq`)
            .setPlaceholder('⏰ How often?')
            .addOptions(
                { label: 'One-time only', value: '0s', emoji: '1️⃣' },
                { label: 'Every 5 Seconds', value: '5s', emoji: '⏱️' },
                { label: 'Every 10 Minutes', value: '10m', emoji: '⏱️' },
                { label: 'Every Hour', value: '1h', emoji: '🕐' },
                { label: 'Every 6 Hours', value: '6h', emoji: '🕕' },
                { label: 'Daily (24h)', value: '1d', emoji: '📅' },
                { label: 'Weekly (7d)', value: '7d', emoji: '📆' },
            );

        rows.push(new ActionRowBuilder().addComponents(freqSelect));
    }

    // --- Row: Color ---
    if (showColor) {
        const colorSelect = new StringSelectMenuBuilder()
            .setCustomId(`${type}_setup_color`)
            .setPlaceholder('🎨 Choose Color')
            .addOptions(
                { label: 'Blue', value: '#3498db', emoji: '🔵' },
                { label: 'Red', value: '#e74c3c', emoji: '🔴' },
                { label: 'Green', value: '#2ecc71', emoji: '🟢' },
                { label: 'Gold', value: '#f1c40f', emoji: '🟡' },
                { label: 'Purple', value: '#9b59b6', emoji: '🟣' },
                { label: 'White', value: '#ffffff', emoji: '⚪' },
            );

        rows.push(new ActionRowBuilder().addComponents(colorSelect));
    }

    // --- Row: Send Button (Always Included) ---
    // const label = (showFrequency && draft.frequency !== '0s') ? 'Schedule Message' : 'Send Message';
    const label = "Complete";

    const sendBtn = new ButtonBuilder()
        .setCustomId(`${type}_send`)
        .setLabel(label)
        .setStyle(ButtonStyle.Success);

    rows.push(new ActionRowBuilder().addComponents(sendBtn));

    // 5. Send Response
    const payload = {
        content: "Here is a preview of your message:",
        embeds: [previewEmbed, statusEmbed],
        components: rows,
        flags: MessageFlags.Ephemeral
    };

    if (isUpdate) {
        await interaction.update(payload);
    } else {
        await interaction.reply(payload);
    }
}