import {
    Client,
    Events,
    Interaction
} from "discord.js";
import {showGiveawayCreationModal} from "../../commands/giveaway/gcreate";

export default {
    name: Events.InteractionCreate,
    async execute(interaction: Interaction, client: Client) {

        // --- User Clicks "Create Giveaway" Button ---
        if (interaction.isButton() && interaction.customId === 'createGiveawayButton') {
            await showGiveawayCreationModal(interaction);
        }
    }
};