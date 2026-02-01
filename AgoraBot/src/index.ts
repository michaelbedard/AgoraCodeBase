import {Client, Collection, Events, GatewayIntentBits, Partials} from "discord.js";
import {config} from "./config";
import {loadCommands} from "./utils/commandLoader";
import {loadEvents} from "./utils/eventLoader";
import "./keep-alive"
import path from "path";
import {initGiveawaySync} from "./stores/giveawayStore";
import {initReactionRoleSync} from "./stores/reactionRoleMap";
import {initRecurrentSync} from "./stores/recurrentMessageStore";
import {initSettingsSync} from "./stores/settingsStore";
import {initTicketMessageSync} from "./stores/ticketButtonStore";


declare module "discord.js" {
    interface Client {
        commands: Collection<string, any>;
    }
}

const client = new Client({
    intents: [
        GatewayIntentBits.Guilds,
        GatewayIntentBits.GuildMessages,
        GatewayIntentBits.MessageContent,
        GatewayIntentBits.DirectMessages,
        GatewayIntentBits.GuildMessageReactions,
        GatewayIntentBits.GuildVoiceStates,
    ],
    partials: [
        Partials.Channel,
        Partials.Message,
        Partials.Reaction
    ]
});

client.setMaxListeners(50);
loadCommands(client, path.join(__dirname, "commands"));
loadEvents(client, path.join(__dirname, "events"))

// sync database
client.once(Events.ClientReady, async () => {
    console.log('🤖 Bot is online! Starting database sync...');

    // This pulls data from Firebase and keeps your local Maps updated
    await initGiveawaySync();
    await initReactionRoleSync();
    await initRecurrentSync();
    await initSettingsSync();
    await initTicketMessageSync();

    console.log('✅ Database sync active');
});

client.login(config.BOT_TOKEN);