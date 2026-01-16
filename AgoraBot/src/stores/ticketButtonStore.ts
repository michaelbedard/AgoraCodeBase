import { ticketMessagesCol } from "../db"; // Make sure to add this export in db.ts (see below)

export interface TicketMessage {
    id: string; // The Message ID where the ticket panel is sent
    title: string;
    description: string;
    footer: string | null;
    banner: string | null;
    thumbnail: string | null;
    color: string | null;
    pingRoles: string[]; // Fixed from [] to string[]
}

// ==========================================================
// 1. GLOBAL SINGLETON
// ==========================================================
const globalAny: any = global;

if (!globalAny.ticketMessages) {
    globalAny.ticketMessages = new Map<string, TicketMessage>();
}

// Key = Message ID (The ID of the bot message containing the button)
export const ticketMessages = globalAny.ticketMessages as Map<string, TicketMessage>;

// ==========================================================
// 2. SYNC FUNCTION
// ==========================================================
export async function initTicketMessageSync() {
    console.log("🔄 Starting Ticket Message Sync...");

    const unsubscribe = ticketMessagesCol.onSnapshot((snapshot) => {
        snapshot.docChanges().forEach((change) => {
            const data = change.doc.data();

            if (change.type === 'added' || change.type === 'modified') {
                ticketMessages.set(data.id, data);
            }
            if (change.type === 'removed') {
                ticketMessages.delete(data.id);
            }
        });
        console.log(`✅ Synced ${ticketMessages.size} ticket panels.`);
    }, (error) => {
        console.error("❌ Ticket Message Sync Error:", error);
    });
}

// ==========================================================
// 3. HELPER FUNCTIONS
// ==========================================================

/**
 * Saves a ticket panel configuration.
 */
export async function createTicketPanel(data: TicketMessage) {
    // 1. Optimistic Update
    ticketMessages.set(data.id, data);

    // 2. Database Update
    await ticketMessagesCol.doc(data.id).set(data);
}

/**
 * Updates a ticket panel (e.g. changing the title).
 */
export async function updateTicketPanel(messageId: string, updates: Partial<TicketMessage>) {
    const existing = ticketMessages.get(messageId);
    if (!existing) return;

    // Merge
    const newData = { ...existing, ...updates };
    ticketMessages.set(messageId, newData);

    await ticketMessagesCol.doc(messageId).update(updates);
}

/**
 * Deletes a ticket panel config (e.g. if the message is deleted).
 */
export async function deleteTicketPanel(messageId: string) {
    ticketMessages.delete(messageId);
    await ticketMessagesCol.doc(messageId).delete();
}