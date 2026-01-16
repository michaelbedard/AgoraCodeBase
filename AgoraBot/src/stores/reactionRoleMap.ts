import { Collection } from 'discord.js';
import {reactionRolesCol} from "../db";

// 1. Define the DB Interface
// Even though the Map only holds the roleId, the DB should hold the context
export interface ReactionRole {
    id: string; // This will be "messageId_emojiId"
    messageId: string;
    emoji: string;
    roleId: string;
}

// 2. THE FIX: USE GLOBAL SCOPE
// We check if the global map already exists. If not, we create it.
const globalAny: any = global;

if (!globalAny.reactionRoleMap) {
    globalAny.reactionRoleMap = new Collection<string, string>();
}

// Export the GLOBAL reference, not a new instance
export const reactionRoleMap = globalAny.reactionRoleMap as Collection<string, string>;

// 3. The Sync Function
export async function initReactionRoleSync() {
    console.log("🔄 Starting Reaction Role Sync...");

    // Listen to the entire collection
    const unsubscribe = reactionRolesCol.onSnapshot((snapshot) => {
        snapshot.docChanges().forEach((change) => {
            const data = change.doc.data();

            // The document ID is already "messageId_emojiId"
            const compositeKey = data.id;

            if (change.type === 'added' || change.type === 'modified') {
                reactionRoleMap.set(compositeKey, data.roleId);
            }
            if (change.type === 'removed') {
                reactionRoleMap.delete(compositeKey);
            }
        });
        console.log(`✅ Synced ${reactionRoleMap.size} reaction roles.`);
    }, (error) => {
        console.error("❌ Reaction Role Sync Error:", error);
    });
}

// 4. Helper: Add a Reaction Role
export async function addReactionRole(
    messageId: string,
    emoji: string,
    roleId: string,
) {
    const compositeKey = `${messageId}_${emoji}`;

    const data: ReactionRole = {
        id: compositeKey,
        messageId,
        emoji,
        roleId
    };

    reactionRoleMap.set(compositeKey, roleId);

    // Save to Firestore (This automatically triggers the listener above to update the Map)
    await reactionRolesCol.doc(compositeKey).set(data);
}

// 5. Helper: Remove a Reaction Role
export async function removeReactionRole(messageId: string, emoji: string) {
    const compositeKey = `${messageId}_${emoji}`;
    reactionRoleMap.delete(compositeKey);
    await reactionRolesCol.doc(compositeKey).delete();
}