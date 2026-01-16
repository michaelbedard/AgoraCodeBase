import {MessageFlags, SlashCommandBuilder} from "discord.js";
import {giveaways} from "../../stores/giveawayStore";

export default {
    data: new SlashCommandBuilder()
        .setName("glist")
        .setDescription("List all running giveaways"),

    async execute(interaction: any) {
        if (giveaways.size === 0) {
            await interaction.reply({
                content: "No active giveaways right now!",
                flags: MessageFlags.Ephemeral,
            });
            return;
        }

        const list = Array.from(giveaways.values())
            .map((g, i) =>
                `**${i + 1}.** 🎁 **${g.title}**\n> **Host:** ${g.host}\n> **Duration:** ${g.duration} ID: \`${g.id}\``
            )
            .join("\n\n");

        await interaction.reply({
            content: `🎉 **Active Giveaways:**\n\n${list}`,
            flags: MessageFlags.Ephemeral,
        });
    },
};
