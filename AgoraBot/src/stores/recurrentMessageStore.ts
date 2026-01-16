import { Collection } from 'discord.js';
import { recurrentCol } from "../db";

export interface RecurrentMessage {
    id: string;
    title: string;
    description: string;
    footer: string | null;
    banner: string | null;
    thumbnail: string | null;
    ping: [];
    color: string | null;
    frequency: string;
    frequencyMs: number;
    channelId: string;
    lastSent: number;
}

// ==========================================================
// 1. GLOBAL SINGLETON SETUP
// ==========================================================
const globalAny: any = global;

if (!globalAny.recurrentMessageStore) {
    globalAny.recurrentMessageStore = new Collection<string, RecurrentMessage>();
}

// for lazy loading
export function getRecurrentMessageStore(): Collection<string, RecurrentMessage> {
    return globalAny.recurrentMessageStore;
}

export const recurrentMessageStore = globalAny.recurrentMessageStore as Collection<string, RecurrentMessage>;

// ==========================================================
// 2. SYNC FUNCTION
// ==========================================================
export async function initRecurrentSync() {
    console.log("🔄 Starting Recurrent Message Sync...");

    const unsubscribe = recurrentCol.onSnapshot((snapshot) => {
        snapshot.docChanges().forEach((change) => {
            const data = change.doc.data();

            if (change.type === 'added' || change.type === 'modified') {
                recurrentMessageStore.set(data.id, data);
            }
            if (change.type === 'removed') {
                recurrentMessageStore.delete(data.id);
            }
        });

        console.log(`✅ Synced ${recurrentMessageStore.size} recurrent messages.`);
    }, (error) => {
        console.error("❌ Recurrent Message Sync Error:", error);
    });
}

// ==========================================================
// 3. HELPER FUNCTIONS
// ==========================================================
export async function createRecurrentMessage(data: RecurrentMessage) {
    recurrentMessageStore.set(data.id, data);
    await recurrentCol.doc(data.id).set(data);
}

export async function deleteRecurrentMessage(id: string) {
    recurrentMessageStore.delete(id);
    await recurrentCol.doc(id).delete();
}

export async function updateRecurrentMessage(id: string, updates: Partial<RecurrentMessage>) {
    const existing = recurrentMessageStore.get(id);
    if (!existing) return;

    const updatedData = { ...existing, ...updates };
    recurrentMessageStore.set(id, updatedData);

    await recurrentCol.doc(id).update(updates);
}