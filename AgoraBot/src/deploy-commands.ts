import { REST, Routes, RESTPostAPIApplicationCommandsJSONBody } from "discord.js";
import { config } from "./config";

const rest = new REST({ version: "10" }).setToken(config.BOT_TOKEN);

type DeployCommandsProps = {
    guildId: string;
    commands: RESTPostAPIApplicationCommandsJSONBody[]; // This accepts ALL command types
};

export async function deployCommands({ guildId, commands }: DeployCommandsProps) {
    try {
        console.log("Started refreshing application commands.");

        await rest.put(
            Routes.applicationGuildCommands(config.CLIENT_ID, guildId),
            {
                body: commands,
            }
        );

        console.log(`Successfully reloaded ${commands.length} application commands.`);
    } catch (error) {
        console.error(error);
    }
}