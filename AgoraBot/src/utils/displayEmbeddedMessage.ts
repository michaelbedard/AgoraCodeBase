import { Client, EmbedBuilder, ColorResolvable, ChannelType } from "discord.js";
import { RecurrentMessage } from "../stores/recurrentMessageStore";

export async function displayEmbeddedMessage(
    client: Client,
    message: RecurrentMessage,
) : Promise<string> {

    // 1. Build the Embed
    const embed = new EmbedBuilder()
        .setTitle(message.title)
        .setDescription(message.description)
        .setColor(message.color as ColorResolvable || "#0099ff");

    if (message.footer) {
        embed.setFooter({ text: message.footer });
    }

    // Banner Image (Large, bottom)
    if (message.banner) {
        embed.setImage(message.banner);
    }

    // Thumbnail Image (Small, top-right)
    // We check if the property exists on the message object
    if (message.thumbnail) {
        embed.setThumbnail(message.thumbnail);
    }

    // 2. Format the Pings
    // Maps ['123', '456'] -> "<@&123> <@&456>"
    const mentionString = message.ping.map(id => `<@&${id}>`).join(" ");

    try {
        const channel = await client.channels.fetch(message.channelId);

        // Check if the channel exists and is a text channel
        if (channel && channel.isTextBased() && channel.type !== ChannelType.GroupDM) {

            // 3. Send Message with Content (Pings) AND Embeds
            const sentMessage = await channel.send({
                content: mentionString.length > 0 ? mentionString : undefined,
                embeds: [embed]
            });

            return sentMessage.id;
        } else {
            console.warn(`Could not send message: Channel ${message.channelId} is not a text channel or was not found.`);
            return "";
        }
    } catch (error) {
        console.error(`Failed to send message to channel ${message.channelId}:`, error);
        return "";
    }
}