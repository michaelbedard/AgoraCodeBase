import { Events, GuildMember, Client } from "discord.js";
import {settingsStore} from "../stores/settingsStore";
import {RecurrentMessage} from "../stores/recurrentMessageStore";
import {displayEmbeddedMessage} from "../utils/displayEmbeddedMessage";

export default {
    name: Events.GuildMemberAdd,
    async execute(member: GuildMember, client: Client) {

        const channelId = settingsStore.get("joinChannelId");

        // verification
        if (!channelId) return;

        const channel = await client.channels.fetch(channelId).catch(() => null);
        if (!channel || !channel.isTextBased()) return;

        // ==========================================
        // PART A: Auto-Role Assignment
        // ==========================================
        const roleId = settingsStore.get("joinRoleId");

        // Only run if a Role ID is actually set
        if (roleId && roleId.trim() !== "") {
            try {
                // 1. Fetch the role to ensure it exists
                const role = await member.guild.roles.fetch(roleId);

                if (role) {
                    // 2. Add the role
                    await member.roles.add(role);
                    console.log(`✅ Assigned role ${role.name} to ${member.user.tag}`);
                } else {
                    console.warn(`⚠️ Join Role ID is set (${roleId}) but role was not found.`);
                }
            } catch (error: any) {
                console.error(`❌ Failed to assign join role:`, error);

                // Detailed check for common error
                if (error.code === 50013) {
                    console.error("⚠️ PERMISSION ERROR: The Bot's role must be higher than the 'Join Role' in Server Settings!");
                }
            }
        }

        // ==========================================
        // PART B: Welcome Message (Your existing code)
        // ==========================================
        console.log(`👤 User ${member.user.tag} joined. Sending message...`);

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