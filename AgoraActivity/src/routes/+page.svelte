<script context="module" lang="ts">
    let globalAuthPromise: Promise<{ code: string, channelId: string | null }> | null = null;
</script>

<script lang="ts">
    import { onMount } from "svelte";
    import { config } from "$lib/config";
    import { DiscordSDK } from "@discord/embedded-app-sdk";
    import { PUBLIC_DISCORD_CLIENT_ID } from '$env/static/public';
    import { page } from "$app/stores";
    import { fade } from 'svelte/transition';

    let unityCanvas: HTMLCanvasElement;
    let discordSdk: DiscordSDK;
    let debugError = "";

    let isLoading = true;
    let loadingProgress = 0;

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

                initializeUnity(false);
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

        initializeUnity(false);

        return { code, channelId: sdk.channelId };
    }

    function initializeUnity(isDiscordEnvironment: boolean) {
        const script = document.createElement("script");
        script.src = "/Build/WebGL.loader.js";
        script.async = true;

        // 1. Start a fake progress interval
        // It increases fast at first, then slows down as it gets closer to 90%
        const progressInterval = setInterval(() => {
            loadingProgress += (90 - loadingProgress) * 0.1;
        }, 100);

        script.onload = () => {
            const cacheBuster = isDiscordEnvironment ? "?v=" + new Date().getTime() : "";

            // @ts-ignore
            createUnityInstance(unityCanvas, {
                dataUrl: `${config.BUILD_PATH}/WebGL.data${config.COMPRESSION}` + cacheBuster,
                frameworkUrl: `${config.BUILD_PATH}/WebGL.framework.js${config.COMPRESSION}` + cacheBuster,
                codeUrl: `${config.BUILD_PATH}/WebGL.wasm${config.COMPRESSION}` + cacheBuster,
                streamingAssetsUrl: "StreamingAssets",
                companyName: config.COMPANY_NAME,
                productName: config.PRODUCT_NAME,
                productVersion: config.PRODUCT_VERSION,
                onProgress: (p: number) => {
                    // Only use real progress if it's actually working (greater than our fake value)
                    const realProgress = p * 100;
                    if (realProgress > loadingProgress) {
                        loadingProgress = realProgress;
                    }
                },
            }).then((instance: any) => {
                // 2. Clear the fake timer
                clearInterval(progressInterval);

                // 3. Force 100% and hide
                loadingProgress = 100;
                window.unityInstance = instance;
                console.log("[Svelte] Unity Instance Loaded.");

                setTimeout(() => {
                    isLoading = false;
                }, 500);

            }).catch((error: any) => {
                clearInterval(progressInterval);
                logError(error);
                isLoading = false;
            });
        };

        script.onerror = () => {
            logError("Failed to load WebGL.loader.js script.");
            clearInterval(progressInterval);
            isLoading = false;
        }

        document.body.appendChild(script);
    }

    function logError(err: any) {
        console.error(err);
        debugError = String(err?.message || err);
    }

</script>

<div class="container">

    {#if isLoading}
        <div class="loading-overlay" transition:fade={{ duration: 1000 }}>
            <div class="spinner"></div>
            <p>Loading... {Math.round(loadingProgress)}%</p>
        </div>
    {/if}

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

    /*loading*/

    .loading-overlay {
        position: absolute;
        top: 0; left: 0;
        width: 100%; height: 100%;
        background-color: #000;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        color: white;
        z-index: 50; /* Ensure it sits on top of canvas */
    }

    .spinner {
        width: 40px; height: 40px;
        border: 4px solid rgba(255, 255, 255, 0.3);
        border-radius: 50%;
        border-top: 4px solid #fff;
        animation: spin 1s linear infinite;
        margin-bottom: 15px;
    }

    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }
</style>