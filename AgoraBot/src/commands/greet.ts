import {SlashCommandBuilder} from "discord.js";

export default {
    data: new SlashCommandBuilder()
        .setName('greet')
        .setDescription('XXX'),

    async execute(interaction: any) {
        const sent = await interaction.reply({
            content: 'Waking up...',
            withResponse: true,
        });

        const possibleReplies = [
            "The box is open and the board is set! Welcome! 🎲",
            "Player found! Dealing you in... 👋",
            "It's always your turn in my book! Hello! 🃏",
            "Setup phase complete. Great to see you! ♟️",
            "Rolling out the red carpet... and the dice! 🎲",
            "Rolled a Natural 20 on friendship! Hello! 🐉",
            "Critical Success! You've arrived! ✨",
            "The odds are ever in your favor. Hi there! 🎲",
            "You rolled a 6! Advancing token to 'Hello'! 🏃",
        ];

        console.log("REPLYING");

        await interaction.editReply(
            possibleReplies[Math.floor(Math.random() * possibleReplies.length)]
        );
    },
};
