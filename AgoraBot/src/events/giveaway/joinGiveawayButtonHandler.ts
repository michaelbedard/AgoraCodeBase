import { Events, MessageFlags, GuildMember } from "discord.js";
import {giveaways, updateGiveaway} from "../../stores/giveawayStore";

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: any) {
        if (!interaction.isButton()) return;

        const [action, id] = interaction.customId.split("_");
        if (action !== "join") return;

        const giveaway = giveaways.get(id);

        // 1. Check if giveaway exists
        if (!giveaway) {
            return interaction.reply({
                content: "❌ This giveaway no longer exists.",
                flags: MessageFlags.Ephemeral
            });
        }

        // 2. Check if user already joined
        if (giveaway.participants.includes(interaction.user.id)) {
            return interaction.reply({
                content: "⚠️ You’ve already joined this giveaway!",
                flags: MessageFlags.Ephemeral
            });
        }

        // 3. CHECK REQUIRED ROLES
        // We ensure 'interaction.member' is treated as a GuildMember to access roles
        const member = interaction.member as GuildMember;

        if (giveaway.allowedRoles && giveaway.allowedRoles.length > 0) {
            // Check if the user has AT LEAST one of the allowed roles
            const hasAllowedRole = giveaway.allowedRoles.some(roleId =>
                member.roles.cache.has(roleId)
            );

            if (!hasAllowedRole) {
                // Format the missing roles nicely so the user knows what they need
                const roleMentions = giveaway.allowedRoles.map(r => `<@&${r}>`).join(", ");

                return interaction.reply({
                    content: `🔒 **Access Denied**\nYou need one of the following roles to join this giveaway:\n${roleMentions}`,
                    flags: MessageFlags.Ephemeral
                });
            }
        }

        // 4. Success - Add to database
        giveaway.participants.push(interaction.user.id);
        await updateGiveaway(giveaway.id, giveaway);

        await interaction.reply({
            content: `🎉 You joined the giveaway for **${giveaway.title}**!`,
            flags: MessageFlags.Ephemeral
        });
    },
};