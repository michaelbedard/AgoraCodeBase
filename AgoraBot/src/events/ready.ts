import { Events, ActivityType } from "discord.js";
import {deployCommands} from "../deploy-commands";

export default {
    name: Events.ClientReady,
    once: true,
    async execute(client: any) {
        if (!client.user) {
            console.error("Client user is null - cannot set presence");
            return;
        }

        console.log(`Ready. Logged as ${client.user.username}`);
        console.log(`Loaded ${client.commands.size} commands`);

        // DEPLOY COMMANDS TO ALL GUILDS
        try {
            const commandsData = Array.from(client.commands.values()).map((cmd : any) => cmd.data.toJSON());

            // Deploy to all guilds the bot is in
            for (const [guildId, guild] of client.guilds.cache) {
                console.log(`Deploying commands to guild: ${guild.name} (${guildId})`);
                await deployCommands({ guildId, commands: commandsData });
            }

            console.log('✅ Commands deployed successfully!');
        } catch (error) {
            console.error('❌ Failed to deploy commands:', error);
        }

        // Set bot presence
        const statusType = process.env.STATUS_TYPE || 'online';
        const activityType = process.env.ACTIVITY_TYPE || 'PLAYING';
        const activityName = process.env.ACTIVITY_NAME || 'Discord';

        client.user.setPresence({
            status: statusType as any,
            activities: [
                {
                    name: activityName,
                    type: ActivityType[activityType as keyof typeof ActivityType],
                }
            ],
        });

        console.log(`Bot status set to: ${statusType}`);
        console.log(`Activity set to: ${activityType} ${activityName}`);
    },
};
