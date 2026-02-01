import { Collection } from "discord.js";

export interface Game {
    id: string;
    title: string;
    description: string;
    minPlayers: number;
    maxPlayers: number;
    playtime: string;
    complexity: string;
    rules: string;
    thumbnail: string;
}

const globalAny: any = global;

if (!globalAny.games) {
    globalAny.games = new Collection<string, Game>();
}

export const games = globalAny.games as Collection<string, Game>;

// Populate the Collection
const gameList: Game[] = [
    {
        id: "uno",
        title: "Uno",
        description: "The classic card game of matching colors and numbers. Watch out for Draw 4s!",
        minPlayers: 2,
        maxPlayers: 4,
        playtime: "15-30 min",
        complexity: "Low",
        rules: "**Objective:** Be the first player to empty your hand.\n\n**On Your Turn:**\n• Match the top card by **Color** or **Number**.\n• If you can't play, you must **Draw**.\n• Use **Skip, Reverse, & +2** to block opponents.\n\n**Additional Details:**\n• **No Stacking:** You cannot play a +2 on top of another +2.\n• **No Wild Finish:** Your final card cannot be a Wild (Black) card.",
        thumbnail: "https://i.imgur.com/v1k1uXj.png",
    },
    {
        id: "love_letter",
        title: "Love Letter",
        description: "A game of risk, deduction, and luck. Get your letter to the Princess!",
        minPlayers: 2,
        maxPlayers: 4,
        playtime: "20 min",
        complexity: "Low",
        rules: "**Objective:** Be the last player standing OR hold the highest card when the deck ends.\n\n**On Your Turn:**\n• Draw one card to your hand (you now hold 2).\n• Choose one to **Play** face-up and resolve its effect.\n\n**Key Rules:**\n• 🛡️ **Handmaid:** Grants immunity for one round.\n• 💂 **Guard:** Guess an opponent's card to knock them out.\n• 👸 **Princess (8):** If you discard this card (voluntarily or forced), you are eliminated immediately!\n\n**Winning:** Win enough rounds to collect the required 'Tokens of Affection'.",
        thumbnail: "https://i.imgur.com/example-loveletter.png",
    },
    {
        id: "skull",
        title: "Skull",
        description: "The ultimate bluffing game. Flowers or Skulls? Play your friends like a fiddle.",
        minPlayers: 3,
        maxPlayers: 6,
        playtime: "30-45 min",
        complexity: "Low",
        rules: "**Objective:** Win 2 challenges by flipping discs without finding a Skull.\n\n**Phase 1: Placement:**\n• Place a disc (Flower 🌸 or Skull ☠️) face-down on your stack.\n\n**Phase 2: Challenge:**\n• Instead of placing, bid a number. Players must bid higher or pass.\n• Highest bidder becomes the **Challenger**.\n\n**The Reveal:**\n• You **must** flip all your own discs first.\n• Then, flip opponents' discs until you reach your bid number.\n• Hit a ☠️? You lose one of your discs permanently.\n• All 🌸? You score 1 point (2 points wins the game).",
        thumbnail: "https://i.imgur.com/example-skull.png",
    }
];

gameList.forEach(game => {
    games.set(game.id, game);
});