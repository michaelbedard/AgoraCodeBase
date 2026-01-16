import { Events, GuildMember, Client, GuildTextBasedChannel } from "discord.js";
import {displayEmbeddedMessage} from "../utils/displayEmbeddedMessage";
import {RecurrentMessage, recurrentMessageStore} from "../stores/recurrentMessageStore";
import {settingsStore} from "../stores/settingsStore";

export default {
    name: Events.GuildMemberRemove,
    async execute(member: GuildMember, client: Client) {

        const channelId = settingsStore.get("joinChannelId");

        // verification
        if (!channelId) return;

        const channel = await client.channels.fetch(channelId).catch(() => null);
        if (!channel || !channel.isTextBased()) return;

        console.log(`👤 User ${member.user.tag} left. Sending message...`);

        const message = {
            id: member.user.id,
            title: "",
            description: "",
            footer: "",
            banner: "",
            thumbnail: "",
            ping: [],
            color: "",
            channelId,
            frequency: "",
            frequencyMs: 0,
            lastSent: 0
        } as RecurrentMessage

        await displayEmbeddedMessage(client, message);
    }
};