import { ContextMenuCommandBuilder, ApplicationCommandType } from "discord.js";
import {handleDirectEdit} from "./medit";

export default {
    data: new ContextMenuCommandBuilder()
        .setName("Edit message")
        .setType(ApplicationCommandType.Message),

    async execute(interaction: any) {
        const message = interaction.targetMessage;
        await handleDirectEdit(interaction, message.id);
    },
};
