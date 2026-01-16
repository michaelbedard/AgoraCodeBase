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
    AppId: string;
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
        maxPlayers: 10,
        playtime: "15-30 min",
        complexity: "Low",
        rules: "Match the top card on the discard pile by color or number. Play wild cards to change color. First to 0 cards wins!",
        thumbnail: "https://i.imgur.com/v1k1uXj.png", // Placeholder or real Uno icon
        AppId: "1433193941381156978"
    },
    {
        id: "coup",
        title: "Coup",
        description: "A game of bluffing, bribery, and manipulation. Destroy your rivals' influence.",
        minPlayers: 2,
        maxPlayers: 6,
        playtime: "15 min",
        complexity: "Medium",
        rules: "You have 2 character cards. Take actions associated with characters (Tax, Assassinate, Steal). You can bluff having a character you don't actually have, but if challenged and caught, you lose a life.",
        thumbnail: "https://i.imgur.com/J3y4vXj.png", // Placeholder or real Coup icon
        AppId: "842074278203621376"
    }
];

gameList.forEach(game => {
    games.set(game.id, game);
});