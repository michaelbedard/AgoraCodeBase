import { giveawaysCol } from "../db";
import {Collection} from "discord.js";

export interface Giveaway {
    id: string;
    channelId: string;
    title: string;
    numberOfWinners: number;
    duration: number;
    host: string;
    createdAt: number;
    allowedRoles?: string[];
    participants: string[];
    winners: string[];
    status: "active" | "ended";
}

const globalAny: any = global;

if (!globalAny.giveaways) {
    globalAny.giveaways = new Collection<string, Giveaway>();
}

export const giveaways = globalAny.giveaways as Collection<string, Giveaway>;

// 2. Create a Sync Function
export async function initGiveawaySync() {
    // 1. Create the query
    const q = giveawaysCol;

    // 2. Call .onSnapshot() directly on the query object
    // Note: In Admin SDK, this returns a function to unsubscribe (stop listening)
    const unsubscribe = q.onSnapshot((snapshot) => {
        snapshot.docChanges().forEach((change) => {
            const data = change.doc.data();

            if (change.type === 'added' || change.type === 'modified') {
                giveaways.set(data.id, data);
            }
            if (change.type === 'removed') {
                giveaways.delete(data.id);
            }
        });
        console.log(`🔄 Synced ${giveaways.size} active giveaways.`);
    }, (error) => {
        console.error("❌ Database Sync Error:", error);
    });
}

/**
 * 1. CREATE
 * Saves a new giveaway to the store and database.
 */
export async function createGiveaway(data: Giveaway) {
    // Optimistic Update (Instant)
    giveaways.set(data.id, data);

    // Database Update
    await giveawaysCol.doc(data.id).set(data);
}

/**
 * 2. UPDATE
 * Generic update function for any field (prize, end time, winners, status).
 */
export async function updateGiveaway(id: string, updates: Partial<Giveaway>) {
    // 1. Check if exists locally
    const existing = giveaways.get(id);
    if (!existing) {
        console.warn(`⚠️ Tried to update missing giveaway: ${id}`);
        return;
    }

    // 2. Merge updates locally
    const updatedData = { ...existing, ...updates };
    giveaways.set(id, updatedData);

    // 3. Update Database
    // We only send the partial updates to Firestore to save bandwidth
    await giveawaysCol.doc(id).update(updates);
}

/**
 * 3. DELETE
 * Removes a giveaway permanently.
 */
export async function deleteGiveaway(id: string) {
    // Optimistic Delete
    giveaways.delete(id);

    // Database Delete
    await giveawaysCol.doc(id).delete();
}