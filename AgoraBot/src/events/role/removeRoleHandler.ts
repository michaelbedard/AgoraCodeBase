import { Events, Client } from "discord.js";
import {reactionRoleMap} from "../../stores/reactionRoleMap";

export default {
    name: Events.MessageReactionRemove,
    async execute(reaction: any, user: any, client: Client) {
        if (user.bot) return;

        if (reaction.partial) {
            try {
                await reaction.fetch();
            } catch (error) {
                console.error('Something went wrong when fetching the message:', error);
                return;
            }
        }

        const messageId = reaction.message.id;
        const emojiKey = reaction.emoji.id ? reaction.emoji.id : reaction.emoji.name;
        const uniqueKey = `${messageId}_${emojiKey}`;

        const roleId = reactionRoleMap.get(uniqueKey);
        if (!roleId) return;

        const guild = reaction.message.guild;
        if (!guild) return;

        try {
            const member = await guild.members.fetch(user.id);
            if (member) {
                await member.roles.remove(roleId);
            }
        } catch (error) {
            console.error("Failed to remove reaction role:", error);
        }
    }
}