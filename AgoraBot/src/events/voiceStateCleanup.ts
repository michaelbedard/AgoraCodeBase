import { Events, VoiceState, ChannelType } from "discord.js";

// The Category ID where games are created (Same as in your Play command)
const GAME_CATEGORY_ID = "1457015111939260436";

export default {
    name: Events.VoiceStateUpdate,
    async execute(oldState: VoiceState, newState: VoiceState) {

        // 1. Get the channel the user LEFT
        const channelLeft = oldState.channel;

        // If they didn't leave a channel (e.g. they just joined one), ignore
        if (!channelLeft) return;

        // 2. Check if that channel is now EMPTY
        if (channelLeft.members.size === 0) {

            // 3. Verify it is a Game Channel
            // Checks: Inside the correct category AND is a Voice Channel
            if (channelLeft.parentId === GAME_CATEGORY_ID && channelLeft.type === ChannelType.GuildVoice) {

                try {
                    await channelLeft.delete();
                    console.log(`🗑️ Deleted empty game channel: ${channelLeft.name}`);
                } catch (err) {
                    console.error("Failed to delete channel:", err);
                }
            }
        }
    },
};