<script context="module" lang="ts">
    let globalAuthPromise: Promise<{ code: string, channelId: string | null }> | null = null;
</script>

<script lang="ts">
    import { onMount } from "svelte";
    import { DiscordHelper } from "$lib/utils/DiscordHelper";
    import { config } from "$lib/config";
    import { DiscordSDK } from "@discord/embedded-app-sdk";
    import { env } from '$env/dynamic/public';
    import { page } from "$app/stores";

    let discordSdk: DiscordSDK;
    let unityCanvas: any;
    let discordHelper: DiscordHelper;

    async function startDiscordAuth(sdk: DiscordSDK) {
        await sdk.ready();

        console.log("[Svelte] Authorizing...");

        // We only ever hit this line ONCE per page load now.
        const { code } = await sdk.commands.authorize({
            client_id: env.PUBLIC_DISCORD_CLIENT_ID,
            response_type: "code",
            state: "",
            scope: ["identify", "guilds"],
        });

        console.log("[Svelte] Authorized!");

        return {
            code,
            channelId: sdk.channelId
        };
    }

    onMount(() => {
        const queryParams = $page.url.searchParams;

        if (!globalAuthPromise) {
            if (queryParams.has('frame_id')) {
                // Initialize the SDK and start the one-time auth
                discordSdk = new DiscordSDK(env.PUBLIC_DISCORD_CLIENT_ID);
                globalAuthPromise = startDiscordAuth(discordSdk);
            } else {
                // Mock data for browser
                console.warn("Running in browser mode. Using mock auth.");
                globalAuthPromise = Promise.resolve({
                    code: "mock_thomas",
                    channelId: "002"
                });
            }
        }

        window.dispatchDiscordData = async () => {
            console.log("[Svelte] Unity requested Discord Data.");

            try {
                if (!globalAuthPromise) return;

                const { code, channelId } = await globalAuthPromise;

                const payload = JSON.stringify({
                    channelId: channelId,
                    authCode: code
                });

                if (window.unityInstance) {
                    window.unityInstance.SendMessage("DiscordBridge", "OnDiscordDataReceived", payload);
                }
            } catch (error: any) {
                console.error("[Svelte] Auth Error:", error);
            }
        };

        // Initialize Helper and Unity
        if (window.location.hostname.includes("discordsays.com")) {
            const originalFetch = window.fetch;

            window.fetch = (input, init) => {
                // 1. Get the URL string safely
                const url = typeof input === 'string' ? input : input instanceof Request ? input.url : '';

                // 2. IGNORE Unity files (StreamingAssets, Build, etc.)
                // If the URL contains these, let it pass through normally.
                if (url.includes("StreamingAssets") || url.includes("/Build/") || url.includes(".wasm") || url.includes(".data")) {
                    return originalFetch(input, init);
                }

                // 3. IGNORE if it's already proxied (prevent double-proxying)
                if (url.includes(".proxy/")) {
                    return originalFetch(input, init);
                }

                // 4. Proxy everything else (External APIs, etc.)
                return originalFetch(".proxy/" + input, init);
            };
        }

        discordHelper = new DiscordHelper();
        discordHelper.setupParentIframe();
        initializeUnity();
    });

    function initializeUnity() {
        const script = document.createElement("script");
        script.src = "/Build/WebGL.loader.js";
        script.async = true;

        script.onload = async () => {
            const buildPath = config.BUILD_PATH
            const extension = config.COMPRESSION

            // @ts-ignore
            createUnityInstance(unityCanvas, {
                dataUrl: `${buildPath}/WebGL.data${extension}`,
                frameworkUrl: `${buildPath}/WebGL.framework.js${extension}`,
                codeUrl: `${buildPath}/WebGL.wasm${extension}`,
                streamingAssetsUrl: "StreamingAssets",
                companyName: config.COMPANY_NAME,
                productName: config.PRODUCT_NAME,
                productVersion: config.PRODUCT_VERSION,
            }).then((instance: any) => {
                window.unityInstance = instance;
                console.log("[Svelte] Unity Instance Loaded.");
            }).catch((err: any) => {
                console.error("[Svelte] Unity Load Error:", err);
            });
        };

        document.body.appendChild(script);

        if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
            // Mobile device style: fill the whole browser client area with the game canvas:
            var meta = document.createElement("meta");
            meta.name = "viewport";
            meta.content =
                "width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes";
            document.getElementsByTagName("head")[0].appendChild(meta);

            unityCanvas = document.querySelector("#unity-canvas");
            unityCanvas.style.width = "100%";
            unityCanvas.style.height = "100%";
            unityCanvas.style.position = "fixed";
            document.body.style.textAlign = "left";
        }
    }
</script>

<canvas
        bind:this={unityCanvas}
        id="unity-canvas"
        tabindex="-1"
        width="960"
        style="
        width: 100vw;
        height: 100vh;
        background: url('/Build/WebGL.jpg') center / cover;
    "
></canvas>

<style>
    :global(body) {
        margin: 0px;
        background-color: #000000;
        padding: 0px;
        text-align: center;
        border: 0;
        height: 100vh;
        width: 100vw;
        overflow: hidden;
    }
</style>
