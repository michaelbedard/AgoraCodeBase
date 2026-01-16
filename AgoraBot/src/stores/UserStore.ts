import { usersCol } from "../db"; // Ensure you export usersCol from your db file
import { Collection } from "discord.js";

// 1. Define the Shape of a User Profile
export interface User {
    id: string;
    username: string;
    joinedAt: number;
    stats: {
        wins: number;
        losses: number;
        gamesPlayed: number;
    };
    coins: number;
}

// 2. Setup Global Cache
const globalAny: any = global;

if (!globalAny.users) {
    globalAny.users = new Collection<string, User>();
}

export const users = globalAny.users as Collection<string, User>;

// 3. Create the Sync Function
export async function initUserSync() {
    const q = usersCol;

    const unsubscribe = q.onSnapshot((snapshot) => {
        snapshot.docChanges().forEach((change) => {
            const data = change.doc.data() as User;

            if (change.type === 'added' || change.type === 'modified') {
                users.set(data.id, data);
            }
            if (change.type === 'removed') {
                users.delete(data.id);
            }
        });
        console.log(`👤 Synced ${users.size} user profiles.`);
    }, (error) => {
        console.error("❌ User Database Sync Error:", error);
    });
}

/**
 * 4. GET OR CREATE (Helper)
 * This is the most common operation: "Get this user, if they don't exist, make them."
 */
export async function getOrCreateUser(discordId: string, username: string): Promise<User> {
    // 1. Check Cache
    if (users.has(discordId)) {
        return users.get(discordId)!;
    }

    // 2. If not in cache, create default data
    const newUser: User = {
        id: discordId,
        username: username,
        joinedAt: Date.now(),
        stats: {
            wins: 0,
            losses: 0,
            gamesPlayed: 0
        },
        coins: 0,
    };

    // 3. Save to Store/DB
    await createUser(newUser);
    return newUser;
}

/**
 * 5. CREATE
 * Saves a new user to the store and database.
 */
export async function createUser(data: User) {
    users.set(data.id, data);
    await usersCol.doc(data.id).set(data);
}

/**
 * 6. UPDATE
 * Generic update function for stats, coins, etc.
 */
export async function updateUser(id: string, updates: Partial<User>) {
    // 1. Check if exists locally
    const existing = users.get(id);
    if (!existing) {
        console.warn(`⚠️ Tried to update missing user: ${id}`);
        return;
    }

    // 2. Merge updates locally
    const updatedData = { ...existing, ...updates };
    users.set(id, updatedData);

    // 3. Update Database
    await usersCol.doc(id).update(updates);
}

/**
 * 7. DELETE
 * Removes a user profile (e.g., GDPR request).
 */
export async function deleteUser(id: string) {
    users.delete(id);
    await usersCol.doc(id).delete();
}