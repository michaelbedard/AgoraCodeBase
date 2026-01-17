import { DISCORD_CLIENT_SECRET } from "$env/static/private";
import { PUBLIC_DISCORD_CLIENT_ID } from "$env/static/public";
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
                client_id: PUBLIC_DISCORD_CLIENT_ID,
                client_secret: DISCORD_CLIENT_SECRET,
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
