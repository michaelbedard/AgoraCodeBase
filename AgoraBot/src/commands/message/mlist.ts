import {MessageFlags, SlashCommandBuilder} from "discord.js";
import {recurrentMessageStore} from "../../stores/recurrentMessageStore";

export default {
    data: new SlashCommandBuilder()
        .setName("mlist")
        .setDescription("List all recurrent messages"),

    async execute(interaction: any) {
        if (recurrentMessageStore.size === 0) {
            await interaction.reply({
                content: "No active recurrent message right now!",
                flags: MessageFlags.Ephemeral,
            });
            return;
        }

        const list = Array.from(recurrentMessageStore.values())
            .map((m, i) => {
                const channelName = `<#${m.channelId}>`;
                return `**${i + 1}.** 🔄 **${m.title}**\n> **Channel:** ${channelName}\n> **Frequency:** ${m.frequencyMs > 0 ? m.frequency : "N/A"} ID: \`${m.id}\``
            })
            .join("\n\n");

        await interaction.reply({
            content: `📋 **Active Messages:**\n\n${list}`,
            flags: MessageFlags.Ephemeral,
        });
    },
};
