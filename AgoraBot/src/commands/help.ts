import {MessageFlags, SlashCommandBuilder} from "discord.js";

export default {
    data: new SlashCommandBuilder()
        .setName('help')
        .setDescription('XXX'),

    async execute(interaction: any) {

        const reply = "Welcome to Agora!\nXXX"

        await interaction.reply({
            content: reply,
            flags: MessageFlags.Ephemeral,
        })
    },
};
