import fs from "fs";
import path from "path";
import { Collection, Client } from "discord.js";

export function loadCommands(client: Client, dirPath?: string): Collection<string, any> {
    if (!client.commands) client.commands = new Collection();

    const commandsDir = dirPath || path.join(__dirname, "../commands");
    const files = fs.readdirSync(commandsDir);

    for (const file of files) {
        const fullPath = path.join(commandsDir, file);
        const stat = fs.statSync(fullPath);

        if (stat.isDirectory()) {
            loadCommands(client, fullPath);
        } else if (file.endsWith(".js") || file.endsWith(".ts")) {
            try {
                const commandImport = require(fullPath);
                const command = commandImport.default || commandImport;

                if ("data" in command && "execute" in command) {
                    client.commands.set(command.data.name, command);
                    console.log(`✅ Loaded command: ${command.data.name}`);
                } else {
                    console.warn(`⚠️ Invalid command file: ${fullPath}`);
                }
            } catch (err) {
                console.error(`❌ Failed to load command from ${fullPath}:`, err);
            }
        }
    }

    return client.commands;
}
