import { settingsCol } from "../db"; // Ensure this export is added to db.ts

export interface Setting {
    id: string;    // The key (e.g. "giveawayChannelId")
    value: string; // The value
}

// ==========================================================
// 1. GLOBAL SINGLETON (With Default Values)
// ==========================================================
const globalAny: any = global;

if (!globalAny.settingsStore) {
    // Initialize with your defaults
    globalAny.settingsStore = new Map<string, string>([
        ["giveawayChannelId", ""],
        ["giveawayDuration", "1d"],
        ["giveawayColor", "#87CEEB"],
        ["joinChannelId", ""],
        ["joinRoleId", ""],
    ]);
}

// Export the GLOBAL reference
export const settingsStore = globalAny.settingsStore as Map<string, string>;

// ==========================================================
// 2. SYNC FUNCTION
// ==========================================================
export async function initSettingsSync() {
    console.log("🔄 Starting Settings Sync...");

    const unsubscribe = settingsCol.onSnapshot((snapshot) => {
        snapshot.docChanges().forEach((change) => {
            const data = change.doc.data();

            // The doc ID is the setting key (e.g. "giveawayChannelId")
            if (change.type === 'added' || change.type === 'modified') {
                settingsStore.set(data.id, data.value);
            }
            if (change.type === 'removed') {
                // If deleted from DB, revert to empty string or keep last known?
                // Usually safe to just delete or set to ""
                settingsStore.set(data.id, "");
            }
        });
        console.log(`✅ Synced ${settingsStore.size} settings.`);
    }, (error) => {
        console.error("❌ Settings Sync Error:", error);
    });
}

// ==========================================================
// 3. HELPER FUNCTIONS
// ==========================================================

/**
 * Saves a setting to the database and updates local cache.
 */
export async function setSetting(key: string, value: string) {
    // 1. Optimistic Update
    settingsStore.set(key, value);

    // 2. Database Update
    // We store it as a document: { id: "giveawayChannelId", value: "123" }
    await settingsCol.doc(key).set({ id: key, value: value });
}

/**
 * Helper to get a setting safely (though .get() works fine directly)
 */
export function getSetting(key: string): string {
    return settingsStore.get(key) || "";
}