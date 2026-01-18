<script context="module" lang="ts">
    let globalAuthPromise: Promise<{ code: string, channelId: string | null }> | null = null;
</script>

<script lang="ts">
    import { onMount } from "svelte";
    import { config } from "$lib/config";
    import { DiscordSDK } from "@discord/embedded-app-sdk";
    import { PUBLIC_DISCORD_CLIENT_ID } from '$env/static/public';
    import { page } from "$app/stores";

    let unityCanvas: HTMLCanvasElement;
    let discordSdk: DiscordSDK;
    let debugError = "";

    onMount(() => {
        const queryParams = $page.url.searchParams;
        const isDiscordEnvironment = queryParams.has('frame_id');

        if (!globalAuthPromise) {
            if (isDiscordEnvironment) {
                // --- REAL DISCORD MODE ---
                console.log("[Svelte] Detected Discord Environment.");
                discordSdk = new DiscordSDK(PUBLIC_DISCORD_CLIENT_ID);
                globalAuthPromise = startDiscordAuth(discordSdk);
            } else {
                console.warn("[Svelte] No frame_id found. Running in BROWSER MODE (Mock Auth).");
                globalAuthPromise = Promise.resolve({
                    code: "mock_code_12345",
                    channelId: "mock_channel_001"
                });

                initializeUnity();
            }
        }

        window.dispatchDiscordData = async () => {
            console.log("[Svelte] Unity requested Discord Data.");

            try {
                if (!globalAuthPromise) return;
                const { code, channelId } = await globalAuthPromise;

                const payload = JSON.stringify({ channelId, authCode: code });

                if (window.unityInstance) {
                    window.unityInstance.SendMessage("DiscordBridge", "OnDiscordDataReceived", payload);
                }
            } catch (error: any) {
                if (window.unityInstance) {
                    const errorMessage = error?.message || String(error) || "Unknown Svelte Error";
                    window.unityInstance.SendMessage("DiscordBridge", "OnDiscordError", errorMessage);
                }

                logError(error)
            }
        };
    })

    async function startDiscordAuth(sdk: DiscordSDK) {
        await sdk.ready();
        console.log("[Svelte] Authorizing...");

        const { code } = await sdk.commands.authorize({
            client_id: PUBLIC_DISCORD_CLIENT_ID,
            response_type: "code",
            state: "",
            scope: ["identify", "guilds"],
        });

        console.log("[Svelte] Authorized!");

        initializeUnity();

        return { code, channelId: sdk.channelId };
    }

    function initializeUnity() {
        const script = document.createElement("script");
        script.src = "/Build/WebGL.loader.js";
        script.async = true;

        script.onload = () => {
            const cacheBuster = "?v=" + new Date().getTime();

            // @ts-ignore
            createUnityInstance(unityCanvas, {
                dataUrl: `${config.BUILD_PATH}/WebGL.data${config.COMPRESSION}` + cacheBuster,
                frameworkUrl: `${config.BUILD_PATH}/WebGL.framework.js${config.COMPRESSION}` + cacheBuster,
                codeUrl: `${config.BUILD_PATH}/WebGL.wasm${config.COMPRESSION}` + cacheBuster,
                streamingAssetsUrl: "StreamingAssets",
                companyName: config.COMPANY_NAME,
                productName: config.PRODUCT_NAME,
                productVersion: config.PRODUCT_VERSION,
            }).then((instance: any) => {
                window.unityInstance = instance;
                console.log("[Svelte] Unity Instance Loaded.");
            }).catch((error: any) => {
                logError(error);
            });
        };

        script.onerror = () => {
            logError("Failed to load WebGL.loader.js script.");
        }

        document.body.appendChild(script);
    }

    function logError(err: any) {
        console.error(err);
        debugError = String(err?.message || err);
    }

</script>

<div class="container">
    <canvas
            bind:this={unityCanvas}
            id="unity-canvas"
            tabindex="-1"
    ></canvas>

    {#if debugError}
        <div class="error-console">
            <h3>⚠️ Error</h3>
            <pre>{debugError}</pre>
            <button on:click={() => navigator.clipboard.writeText(debugError)}>Copy</button>
        </div>
    {/if}
</div>

<style>
    :global(body) {
        margin: 0;
        background-color: #000;
        overflow: hidden;
    }

    .container {
        position: relative;
        width: 100vw;
        height: 100vh;
    }

    canvas {
        display: block;
        width: 100%;
        height: 100%;
        background: url('/Build/WebGL.jpg') center / cover;
    }

    .error-console {
        position: absolute;
        bottom: 20px;
        left: 20px;
        right: 20px;
        background: rgba(255, 0, 0, 0.9);
        color: white;
        padding: 15px;
        border-radius: 8px;
        z-index: 100;
        text-align: left;
        font-family: monospace;
    }
</style>