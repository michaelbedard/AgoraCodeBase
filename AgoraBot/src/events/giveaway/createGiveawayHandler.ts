import {
    Events,
    EmbedBuilder,
    ButtonBuilder,
    ButtonStyle,
    ActionRowBuilder,
    MessageFlags,
    ColorResolvable,
    RoleSelectMenuBuilder,
    ChannelSelectMenuBuilder,
    ChannelType,
    ComponentType,
    TextChannel
} from "discord.js";
import {createGiveaway, Giveaway, giveaways} from "../../stores/giveawayStore";
import { parseDuration } from "../../utils/parseduration";
import { endGiveaway } from "../../commands/giveaway/gend";
import { settingsStore } from "../../stores/settingsStore";

// --- TEMPORARY DRAFT STORE ---
// Stores data while the user is clicking menus.
// Key: User ID, Value: Partial Giveaway Data
const giveawayDrafts = new Map<string, any>();

export default {
    name: Events.InteractionCreate,
    async execute(interaction: any, client: any) {

        // ====================================================
        // 1. HANDLE MODAL SUBMIT (Step 1: Text Inputs)
        // ====================================================
        if (interaction.isModalSubmit() && interaction.customId === "giveawayCreateForm") {
            const title = interaction.fields.getTextInputValue("giveawayTitle");
            const entriesStr = interaction.fields.getTextInputValue("giveawayEntries");
            const numberOfWinners = parseInt(entriesStr);

            // Validate Duration immediately
            const durationStr = settingsStore.get("giveawayDuration") || "1d";
            const durationMs = parseDuration(durationStr);
            if (durationMs === null) {
                return interaction.reply({
                    content: "❌ Invalid duration format in settings! Check your config.",
                    flags: MessageFlags.Ephemeral
                });
            }

            // Save to Draft Store
            giveawayDrafts.set(interaction.user.id, {
                title,
                numberOfWinners,
                durationMs,
                allowedRoles: [],    // Default: All allowed
                targetChannelId: interaction.channelId // Default: Current channel
            });

            // --- Build Step 2 Interface (Menus) ---

            // A. Role Selector
            const roleSelect = new RoleSelectMenuBuilder()
                .setCustomId('giveaway_setup_roles')
                .setPlaceholder('Select Allowed Roles (Optional)')
                .setMinValues(0)
                .setMaxValues(5);

            // B. Launch Button
            const launchBtn = new ButtonBuilder()
                .setCustomId('giveaway_setup_launch')
                .setLabel('🚀 Launch Giveaway')
                .setStyle(ButtonStyle.Success);

            // Embed to show current status
            const previewEmbed = new EmbedBuilder()
                .setTitle("🛠️ Configure Giveaway")
                .setDescription(
                    `**Title:** ${title}\n` +
                    `**Winners:** ${numberOfWinners}\n` +
                    `**Duration:** ${durationStr}\n\n` +
                    `👇 **Optional:** Select roles below.`
                )
                .setColor(0x00AAFF);

            const row1 = new ActionRowBuilder<RoleSelectMenuBuilder>().addComponents(roleSelect);
            const row2 = new ActionRowBuilder<ButtonBuilder>().addComponents(launchBtn);

            await interaction.reply({
                embeds: [previewEmbed],
                components: [row1, row2],
                flags: MessageFlags.Ephemeral
            });
            return;
        }

        // ====================================================
        // 2. HANDLE MENU SELECTIONS (Step 2a: Updating Draft)
        // ====================================================
        if (interaction.isAnySelectMenu()) {
            if (!['giveaway_setup_roles'].includes(interaction.customId)) return;

            const draft = giveawayDrafts.get(interaction.user.id);
            if (!draft) {
                return interaction.reply({ content: "❌ Session expired. Please run /gcreate again.", flags: MessageFlags.Ephemeral });
            }

            // Update Draft based on what they picked
            if (interaction.customId === 'giveaway_setup_roles') {
                draft.allowedRoles = interaction.values; // Array of Role IDs
            }

            giveawayDrafts.set(interaction.user.id, draft);

            // Update the Embed visually to confirm selection
            const updatedEmbed = EmbedBuilder.from(interaction.message.embeds[0])
                .setDescription(
                    `**Title:** ${draft.title}\n` +
                    `**Winners:** ${draft.numberOfWinners}\n` +
                    `**Allowed Roles:** ${draft.allowedRoles.length > 0 ? draft.allowedRoles.map((r:string) => `<@&${r}>`).join(", ") : "Everyone"}\n\n` +
                    `👇 Click **Launch** when ready!`
                );

            await interaction.update({ embeds: [updatedEmbed] });
            return;
        }

        // ====================================================
        // 3. HANDLE LAUNCH BUTTON (Step 3: Finalize)
        // ====================================================
        if (interaction.isButton() && interaction.customId === "giveaway_setup_launch") {
            const draft = giveawayDrafts.get(interaction.user.id);
            if (!draft) {
                return interaction.reply({ content: "❌ Session expired. Please run /gcreate again.", flags: MessageFlags.Ephemeral });
            }

            // --- ORIGINAL LOGIC RESTORED HERE ---

            // 1. Get Color
            const rawColor = settingsStore.get("giveawayColor") || "87CEEB";
            const color: ColorResolvable = parseInt(rawColor.replace(/^#/, ""), 16);

            // 2. Resolve Channel
            let targetChannel = interaction.channel;
            const configuredChannelId = settingsStore.get("giveawayChannelId");
            if (configuredChannelId) {
                try {
                    const fetchedChannel = await client.channels.fetch(configuredChannelId);
                    if (fetchedChannel && fetchedChannel.isTextBased()) {
                        targetChannel = fetchedChannel;
                    }
                } catch (error) {
                    console.warn(`⚠️ Configured giveaway channel (${configuredChannelId}) not found. Defaulting to current channel.`);
                }
            }

            // 3. Build Public Embed
            const endTime = new Date(Date.now() + draft.durationMs);
            const publicEmbed = new EmbedBuilder()
                .setTitle(`🎉 ${draft.title}`)
                .setDescription(
                    `**Hosted by:** <@${interaction.user.id}>\n` +
                    `**Winners:** ${draft.numberOfWinners}\n` +
                    `**Ends:** <t:${Math.floor(endTime.getTime() / 1000)}:R>\n` +
                    (draft.allowedRoles.length > 0 ? `**Req. Roles:** ${draft.allowedRoles.map((r:string) => `<@&${r}>`).join(" ")}\n` : "") +
                    `\nClick below to enter!`
                )
                .setColor(color)
                .setTimestamp();

            // 4. Send to Public Channel
            const publicMessage = await targetChannel.send({
                embeds: [publicEmbed],
                components: []
            });

            // 5. Add Join Button
            const id = publicMessage.id;
            const joinButton = new ButtonBuilder()
                .setCustomId(`join_${id}`)
                .setLabel("🎉 Join Giveaway")
                .setStyle(ButtonStyle.Success);

            await publicMessage.edit({
                components: [new ActionRowBuilder<ButtonBuilder>().addComponents(joinButton)]
            });

            // 6. Store Giveaway
            const giveaway: Giveaway = {
                id,
                channelId: targetChannel.id,
                title: draft.title,
                numberOfWinners: draft.numberOfWinners,
                duration: draft.durationMs,
                host: interaction.user.id,
                createdAt: Date.now(),
                participants: [],
                winners: [],
                status: "active",
                // Store the roles so we can check them when users try to join later!
                allowedRoles: draft.allowedRoles
            };

            await createGiveaway(giveaway);

            // 7. Clear Draft & Confirm
            giveawayDrafts.delete(interaction.user.id);

            await interaction.update({
                content: `✅ **Giveaway Launched!** Check <#${targetChannel.id}>`,
                embeds: [],
                components: []
            });

            // 8. Schedule End
            setTimeout(async () => {
                const endedGiveaway = giveaways.get(id);
                if (!endedGiveaway || endedGiveaway.status === "ended") return;
                const result = await endGiveaway(client, endedGiveaway);
                if (result.success) {
                    try {
                        const ch = await client.channels.fetch(endedGiveaway.channelId);
                        if(ch && ch.isTextBased()) await ch.send(result.message);
                    } catch (e) { console.error("Failed to send winner announcement", e); }
                }
            }, draft.durationMs);
        }
    },
};