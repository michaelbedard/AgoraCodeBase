import { env as privateEnv } from "$env/dynamic/private";
import {env, env as publicEnv} from "$env/dynamic/public";
import { error, json } from "@sveltejs/kit";
import type { RequestHandler } from "./$types";

export const POST: RequestHandler = async ({ request }) => {
    try {
        const { code } = await request.json();

        const response = await fetch("https://discord.com/api/oauth2/token", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded",
            },
            body: new URLSearchParams({
                client_id: publicEnv.PUBLIC_DISCORD_CLIENT_ID,
                client_secret: privateEnv.DISCORD_CLIENT_SECRET,
                grant_type: "authorization_code",
                code,
            }),
        });

        const { access_token } = await response.json();

        return json({ access_token });
    } catch (err) {
        console.error(err);
        throw error(500, "Internal Server Error");
    }
}

export async function GET() {
    throw error(405, "My dude, you can't do that here.");
}
