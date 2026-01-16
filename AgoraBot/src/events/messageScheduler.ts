import { Events, Client } from "discord.js";
import { displayEmbeddedMessage } from "../utils/displayEmbeddedMessage";

// DO NOT import the const 'recurrentMessageStore' here.
// It causes the loop.
import { getRecurrentMessageStore, updateRecurrentMessage } from "../stores/recurrentMessageStore";

const TICK_RATE = 1000;

export default {
    name: Events.ClientReady,
    once: true,
    async execute(client: any) {
        startScheduler(client);
    },
};

function startScheduler(client: Client) {
    console.log("⏰ Scheduler started.");

    // Check immediately safely
    checkScheduledMessages(client);

    setInterval(() => checkScheduledMessages(client), TICK_RATE);
}

async function checkScheduledMessages(client: Client) {
    const now = Date.now();

    // 👇 USE THE FUNCTION, NOT THE CONST
    const store = getRecurrentMessageStore();

    // This should now always succeed
    if (!store) {
        console.error("❌ Scheduler Error: Global store is missing!");
        return;
    }

    const messages = store.values();

    for (const message of messages) {
        if (!message || !message.frequencyMs || message.frequencyMs === 0) continue;

        const timeSinceLastSent = now - message.lastSent;

        if (timeSinceLastSent >= message.frequencyMs) {
            console.log(`🚀 Sending recurring message ${message.id}`);

            try {
                await displayEmbeddedMessage(client, message);

                await updateRecurrentMessage(message.id, {
                    lastSent: now
                });

            } catch (error) {
                console.error(`❌ Failed to send ${message.id}:`, error);
            }
        }
    }
}