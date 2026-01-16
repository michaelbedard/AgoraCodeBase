import fs from "fs";
import path from "path";
import { Client } from "discord.js";

/**
 * Recursively loads all events from a directory into the client
 * @param client Discord Client
 * @param eventsDir Path to events folder
 */
export function loadEvents(client: Client, eventsDir?: string) {
    const dir = eventsDir || path.join(__dirname, "../events");
    const files = fs.readdirSync(dir);

    for (const file of files) {
        const fullPath = path.join(dir, file);
        const stat = fs.statSync(fullPath);

        if (stat.isDirectory()) {
            // Recursively load events from subfolders
            loadEvents(client, fullPath);
        } else if (file.endsWith(".js") || file.endsWith(".ts")) {
            try {
                const eventImport = require(fullPath);
                const event = eventImport.default || eventImport;

                if (!event?.name || typeof event.execute !== "function") {
                    console.warn(`⚠️ Invalid event file: ${file}`);
                    continue;
                }

                if (event.once) {
                    client.once(event.name, (...args) => event.execute(...args, client));
                } else {
                    client.on(event.name, (...args) => event.execute(...args, client));
                }

                console.log(`✅ Loaded event: ${event.name} (${file})`);
            } catch (error) {
                console.error(`❌ Failed to load event file ${file}:`, error);
            }
        }
    }
}
