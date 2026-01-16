import { Events, Client, GuildMember } from "discord.js";
import {reactionRoleMap} from "../../stores/reactionRoleMap";

export default {
    name: Events.MessageReactionAdd,
    async execute(reaction: any, user: any, client: Client) {

        // 1. Ignore bots
        if (user.bot) return;

        // 2. Handle Partials (Fetch message if it was sent before bot started)
        if (reaction.partial) {
            try {
                await reaction.fetch();
            } catch (error) {
                console.error('Something went wrong when fetching the message:', error);
                return;
            }
        }

        // 3. Construct Key to look up in our store
        const messageId = reaction.message.id;
        const emojiKey = reaction.emoji.id ? reaction.emoji.id : reaction.emoji.name;
        const targetKey = `${messageId}_${emojiKey}`;

        // 4. Check if this reaction is linked to a role
        //const roleId = reactionRoleMap.get(uniqueKey);
        //if (!roleId) return;

        console.log(`🔎 [DEBUG] Trying to find key: "${targetKey}"`);
        console.log(`📊 [DEBUG] Map Size: ${reactionRoleMap.size}`);
        console.log(`📂 [DEBUG] Available Keys:`, [...reactionRoleMap.keys()]);

        const roleId = reactionRoleMap.get(targetKey);

        if (!roleId) {
            console.log("❌ Key not found in Map!");
            return;
        } else {
            console.log("✅ Key found! Role ID:", roleId);
        }

        // 5. Get the member and Role
        const guild = reaction.message.guild;
        if (!guild) return;

        try {
            const member = await guild.members.fetch(user.id);
            const role = await guild.roles.fetch(roleId);

            if (member && role) {
                await member.roles.add(role);
            }
        } catch (error) {
            console.error("Failed to add reaction role:", error);
        }
    }
}