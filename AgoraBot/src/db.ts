import {initializeApp, cert, ServiceAccount, getApps} from 'firebase-admin/app';
import { getFirestore, FirestoreDataConverter } from 'firebase-admin/firestore';
import {Giveaway} from "./stores/giveawayStore";
import {config} from "./config"
import {ReactionRole} from "./stores/reactionRoleMap";
import {TicketMessage} from "./stores/ticketButtonStore";
import {Setting} from "./stores/settingsStore";
import {RecurrentMessage} from "./stores/recurrentMessageStore";
import {User} from "./stores/UserStore";

let serviceAccount: ServiceAccount;

// 1. INITIALIZE
if (getApps().length === 0) {
    let serviceAccount: ServiceAccount;

    if (config.ENVIRONMENT === "Production") {
        console.log("🚀 Starting in PRODUCTION mode...");
        serviceAccount = require('../service-account-prod.json');
    } else {
        console.log("🛠️ Starting in DEVELOPMENT mode...");
        serviceAccount = require('../service-account-dev.json');
    }

    initializeApp({
        credential: cert(serviceAccount)
    });
}

const db = getFirestore();

// 2. Define Converters (This maps your TS Interface <-> Firestore)

const userConverter: FirestoreDataConverter<User> = {
    toFirestore: (data) => data,
    fromFirestore: (snap) => {
        const data = snap.data();
        return {
            id: snap.id,
            username: data.username || "Unknown Player",
            joinedAt: data.joinedAt || Date.now(),
            stats: {
                wins: data.stats?.wins || 0,
                losses: data.stats?.losses || 0,
                gamesPlayed: data.stats?.gamesPlayed || 0
            },

            coins: data.coins || 0,
            reputation: data.reputation || 0
        } as User;
    }
};

const giveawayConverter: FirestoreDataConverter<Giveaway> = {
    toFirestore: (data) => data, // Saves as-is
    fromFirestore: (snap) => {
        const data = snap.data()
        return {
            id: snap.id, // Ensure ID is always set from the document ID
            channelId: data.channelId || "",
            title: data.title || "Unknown Giveaway",
            numberOfWinners: data.numberOfWinners || 1,
            duration: data.duration || 0,
            host: data.host || "",
            createdAt: data.createdAt || Date.now(),
            participants: data.participants || [],
            winners: data.winners || [],
            status: data.status || "active",
        } as Giveaway
    }
};

const reactionRoleConverter: FirestoreDataConverter<ReactionRole> = {
    toFirestore: (data) => data,
    fromFirestore: (snap) => {
        const data = snap.data()
        return {
            id: snap.id, // Always use the document ID (which is "messageId_emoji")
            messageId: data.messageId || "",
            emoji: data.emoji || "",
            roleId: data.roleId || ""
        } as ReactionRole
    }
};

const recurrentConverter: FirestoreDataConverter<RecurrentMessage> = {
    toFirestore: (data) => {
        // Optional: Handle ColorResolvable here if it's complex, otherwise save as string/number
        return { ...data };
    },
    fromFirestore: (snap) => {
        const data = snap.data()
        return {
            id: snap.id, // Always use the document ID as the source of truth
            title: data.title || "Untitled Message",
            description: data.description || "",
            footer: data.footer || null,
            banner: data.banner || null,
            thumbnail: data.thumbnail || null,
            ping: data.ping || [],
            color: data.color || null,
            frequency: data.frequency || "0s",
            frequencyMs: data.frequencyMs || 0,
            channelId: data.channelId || "",
            lastSent: data.lastSent || 0
        } as RecurrentMessage;
    }
};

const ticketMessageConverter: FirestoreDataConverter<TicketMessage> = {
    toFirestore: (data) => data,
    fromFirestore: (snap) => {
        const data = snap.data();
        return {
            id: snap.id,
            title: data.title || "Support Ticket",
            description: data.description || "Click below to open a ticket.",
            footer: data.footer || null,
            banner: data.banner || null,
            thumbnail: data.thumbnail || null,
            color: data.color || null,
            pingRoles: data.pingRoles || [],
            channelId: data.channelId || ""
        } as TicketMessage;
    }
};

const settingsConverter: FirestoreDataConverter<Setting> = {
    toFirestore: (data) => {
        return { value: data.value }; // We only need to save the value field
    },
    fromFirestore: (snap) => {
        const data = snap.data();
        return {
            id: snap.id, // The document ID is the key
            value: data.value || ""
        } as Setting;
    }
};


// 3. Export Typed Collections
export const usersCol = db.collection('users').withConverter(userConverter); // <--- Export new collection
export const giveawaysCol = db.collection('giveaways').withConverter(giveawayConverter);
export const reactionRolesCol = db.collection('reaction_roles').withConverter(reactionRoleConverter);
export const recurrentCol = db.collection('recurrent_messages').withConverter(recurrentConverter);
export const ticketMessagesCol = db.collection('ticket_panels').withConverter(ticketMessageConverter);
export const settingsCol = db.collection('bot_settings').withConverter(settingsConverter);
