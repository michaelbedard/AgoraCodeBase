import {Events, ActivityType, Guild, Client} from "discord.js";
import {deployCommands} from "../deploy-commands";

export default {
    name: Events.GuildCreate,
    once: true,
    async execute(guild: Guild, client: Client) {
        const commandsData = Array.from(client.commands.values()).map(cmd => cmd.data.toJSON());
        await deployCommands({ guildId: guild.id, commands: commandsData });
    },
};
