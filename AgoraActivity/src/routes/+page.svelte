<script lang="ts">
    import { onMount } from "svelte";
    import { config } from "$lib/config";
    // import { DiscordSDK } from "@discord/embedded-app-sdk";
    // ... other imports

    let unityCanvas: HTMLCanvasElement;
    let isGameLoaded = false;
    let isGameStarted = false;
    let debugError = ""; // Variable to store error text

    // Function to print errors to the screen
    function logError(err: any) {
        console.error(err);
        debugError = String(err?.message || err);
    }

    function initializeUnity() {
        // Prevent double loading
        if (isGameLoaded) {
            console.log("[Svelte] Game already loading/loaded.");
            return;
        }
        isGameLoaded = true;

        const script = document.createElement("script");
        script.src = "/Build/WebGL.loader.js";
        script.async = true;

        script.onload = () => {
            // @ts-ignore
            createUnityInstance(unityCanvas, {
                dataUrl: `${config.BUILD_PATH}/WebGL.data${config.COMPRESSION}`,
                frameworkUrl: `${config.BUILD_PATH}/WebGL.framework.js${config.COMPRESSION}`,
                codeUrl: `${config.BUILD_PATH}/WebGL.wasm${config.COMPRESSION}`,
                streamingAssetsUrl: "StreamingAssets",
                companyName: config.COMPANY_NAME,
                productName: config.PRODUCT_NAME,
                productVersion: config.PRODUCT_VERSION,
            }).then((instance: any) => {
                window.unityInstance = instance;
                console.log("[Svelte] Unity Instance Loaded.");
            }).catch((error: any) => {
                // PRINT ERROR TO SCREEN
                logError(error);
            });
        };

        script.onerror = () => {
            logError("Failed to load WebGL.loader.js script.");
        }

        document.body.appendChild(script);
    }

    function handleStartClick() {
        isGameStarted = true;
        initializeUnity();
    }
</script>

<div class="container">
    <canvas
            bind:this={unityCanvas}
            id="unity-canvas"
            tabindex="-1"
    ></canvas>

    {#if !isGameStarted}
        <div class="overlay">
            <button class="start-btn" on:click={handleStartClick}>
                Start Game
            </button>
        </div>
    {/if}

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
        /* Moved background here */
        background: url('/Build/WebGL.jpg') center / cover;
    }

    .overlay {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0,0,0,0.7);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 10;
    }

    .start-btn {
        padding: 20px 40px;
        font-size: 20px;
        background-color: #5865F2;
        color: white;
        border: none;
        border-radius: 8px;
        cursor: pointer;
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











<!--<script context="module" lang="ts">-->
<!--    let globalAuthPromise: Promise<{ code: string, channelId: string | null }> | null = null;-->
<!--</script>-->

<!--<script lang="ts">-->
<!--    import { onMount } from "svelte";-->
<!--    import { DiscordHelper } from "$lib/utils/DiscordHelper";-->
<!--    import { config } from "$lib/config";-->
<!--    import { DiscordSDK } from "@discord/embedded-app-sdk";-->
<!--    import { PUBLIC_DISCORD_CLIENT_ID } from '$env/static/public';-->
<!--    import { page } from "$app/stores";-->

<!--    let unityCanvas: any;-->
<!--    // let discordSdk: DiscordSDK;-->
<!--    // let discordHelper: DiscordHelper | null = null;-->

<!--    let isGameLoaded = false;-->
<!--    let isGameStarted = false;-->

<!--    async function startDiscordAuth(sdk: DiscordSDK) {-->
<!--        await sdk.ready();-->
<!--        console.log("[Svelte] Authorizing...");-->

<!--        const { code } = await sdk.commands.authorize({-->
<!--            client_id: PUBLIC_DISCORD_CLIENT_ID,-->
<!--            response_type: "code",-->
<!--            state: "",-->
<!--            scope: ["identify", "guilds"],-->
<!--        });-->

<!--        console.log("[Svelte] Authorized!");-->

<!--        initializeUnity();-->

<!--        return { code, channelId: sdk.channelId };-->
<!--    }-->

<!--    onMount(() => {-->
<!--        initializeUnity(); // for testing-->

<!--        // const queryParams = $page.url.searchParams;-->
<!--        // const isDiscordEnvironment = queryParams.has('frame_id'); // Intelligent check-->
<!--        //-->
<!--        // if (!globalAuthPromise) {-->
<!--        //     if (isDiscordEnvironment) {-->
<!--        //         // -&#45;&#45; REAL DISCORD MODE -&#45;&#45;-->
<!--        //         console.log("[Svelte] Detected Discord Environment.");-->
<!--        //         discordSdk = new DiscordSDK(PUBLIC_DISCORD_CLIENT_ID);-->
<!--        //         globalAuthPromise = startDiscordAuth(discordSdk);-->
<!--        //     } else {-->
<!--        //         console.warn("[Svelte] No frame_id found. Running in BROWSER MODE (Mock Auth).");-->
<!--        //         globalAuthPromise = Promise.resolve({-->
<!--        //             code: "mock_code_12345",-->
<!--        //             channelId: "mock_channel_001"-->
<!--        //         });-->
<!--        //-->
<!--        //         initializeUnity();-->
<!--        //     }-->
<!--        // }-->

<!--        window.dispatchDiscordData = async () => {-->
<!--            console.log("[Svelte] Unity requested Discord Data.");-->

<!--            // try {-->
<!--            //     if (!globalAuthPromise) return;-->
<!--            //     const { code, channelId } = await globalAuthPromise;-->
<!--            //-->
<!--            //     const payload = JSON.stringify({ channelId, authCode: code });-->
<!--            //-->
<!--            //     if (window.unityInstance) {-->
<!--            //         window.unityInstance.SendMessage("DiscordBridge", "OnDiscordDataReceived", payload);-->
<!--            //     }-->
<!--            // } catch (error: any) {-->
<!--            //     console.error("[Svelte] Auth Error during dispatch:", error);-->
<!--            //-->
<!--            //     if (window.unityInstance) {-->
<!--            //         const errorMessage = error?.message || String(error) || "Unknown Svelte Error";-->
<!--            //         window.unityInstance.SendMessage("DiscordBridge", "OnDiscordError", errorMessage);-->
<!--            //     }-->
<!--            // }-->
<!--        };-->

<!--        // // Initialize Discord Helper ONLY if in Discord-->
<!--        // if (isDiscordEnvironment) {-->
<!--        //     if (window.location.hostname.includes("discordsays.com")) {-->
<!--        //         setupProxy();-->
<!--        //     }-->
<!--        //-->
<!--        //     try {-->
<!--        //         discordHelper = new DiscordHelper();-->
<!--        //         discordHelper.setupParentIframe();-->
<!--        //     } catch (error: any) {-->
<!--        //         console.error("[Svelte] Failed to setup Discord Helper:", error);-->
<!--        //-->
<!--        //         if (window.unityInstance) {-->
<!--        //             const errorMessage = error?.message || String(error) || "Unknown Svelte Error";-->
<!--        //             window.unityInstance.SendMessage("DiscordBridge", "OnDiscordError", errorMessage);-->
<!--        //         }-->
<!--        //     }-->
<!--        // } else {-->
<!--        //     console.log("[Svelte] Skipping DiscordHelper setup (Browser Mode).");-->
<!--        // }-->
<!--    });-->

<!--    function setupProxy() {-->
<!--        const originalFetch = window.fetch;-->
<!--        window.fetch = (input, init) => {-->
<!--            const url = typeof input === 'string' ? input : input instanceof Request ? input.url : '';-->
<!--            if (url.includes("StreamingAssets") || url.includes("/Build/") ||-->
<!--                url.includes(".wasm") || url.includes(".data") || url.includes(".proxy/")) {-->
<!--                return originalFetch(input, init);-->
<!--            }-->
<!--            return originalFetch(".proxy/" + input, init);-->
<!--        };-->
<!--    }-->

<!--    function initializeUnity() {-->
<!--        if (isGameLoaded) return;-->
<!--        isGameLoaded = true;-->

<!--        const script = document.createElement("script");-->
<!--        script.src = "/Build/WebGL.loader.js";-->
<!--        script.async = true;-->

<!--        script.onload = async () => {-->
<!--            const buildPath = config.BUILD_PATH-->
<!--            const extension = config.COMPRESSION-->

<!--            // @ts-ignore-->
<!--            createUnityInstance(unityCanvas, {-->
<!--                dataUrl: `${buildPath}/WebGL.data${extension}`,-->
<!--                frameworkUrl: `${buildPath}/WebGL.framework.js${extension}`,-->
<!--                codeUrl: `${buildPath}/WebGL.wasm${extension}`,-->
<!--                streamingAssetsUrl: "StreamingAssets",-->
<!--                companyName: config.COMPANY_NAME,-->
<!--                productName: config.PRODUCT_NAME,-->
<!--                productVersion: config.PRODUCT_VERSION,-->
<!--            }).then((instance: any) => {-->
<!--                window.unityInstance = instance;-->
<!--                console.log("[Svelte] Unity Instance Loaded.");-->
<!--            }).catch((error: any) => {-->
<!--                console.error("[Svelte] Unity Load Error:", error);-->

<!--                if (window.unityInstance) {-->
<!--                    const errorMessage = error?.message || String(error) || "Unknown Svelte Error";-->
<!--                    window.unityInstance.SendMessage("DiscordBridge", "OnDiscordError", errorMessage);-->
<!--                }-->
<!--            });-->
<!--        };-->

<!--        document.body.appendChild(script);-->

<!--        // Mobile adjustment logic remains same...-->
<!--        if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {-->
<!--            var meta = document.createElement("meta");-->
<!--            meta.name = "viewport";-->
<!--            meta.content = "width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes";-->
<!--            document.getElementsByTagName("head")[0].appendChild(meta);-->

<!--            if(unityCanvas) {-->
<!--                unityCanvas.style.width = "100%";-->
<!--                unityCanvas.style.height = "100%";-->
<!--                unityCanvas.style.position = "fixed";-->
<!--            }-->
<!--            document.body.style.textAlign = "left";-->
<!--        }-->
<!--    }-->

<!--    function handleStartClick() {-->
<!--        isGameStarted = true;-->
<!--        initializeUnity();-->
<!--    }-->

<!--</script>-->

<!--<div class="container">-->
<!--    <canvas-->
<!--            bind:this={unityCanvas}-->
<!--            id="unity-canvas"-->
<!--            tabindex="-1"-->
<!--    ></canvas>-->

<!--    {#if !isGameStarted}-->
<!--        <div class="overlay">-->
<!--            <button class="start-btn" on:click={handleStartClick}>-->
<!--                Start Game-->
<!--            </button>-->
<!--        </div>-->
<!--    {/if}-->
<!--</div>-->

<!--<canvas-->
<!--        bind:this={unityCanvas}-->
<!--        id="unity-canvas"-->
<!--        tabindex="-1"-->
<!--        width="960"-->
<!--        style="-->
<!--        width: 100vw;-->
<!--        height: 100vh;-->
<!--        background: url('/Build/WebGL.jpg') center / cover;-->
<!--    "-->
<!--&gt;</canvas>-->

<!--<style>-->
<!--    :global(body) {-->
<!--        margin: 0px;-->
<!--        background-color: #000000;-->
<!--        padding: 0px;-->
<!--        text-align: center;-->
<!--        border: 0;-->
<!--        height: 100vh;-->
<!--        width: 100vw;-->
<!--        overflow: hidden;-->
<!--    }-->

<!--    .container {-->
<!--        position: relative;-->
<!--        width: 100vw;-->
<!--        height: 100vh;-->
<!--        overflow: hidden;-->
<!--    }-->

<!--    .overlay {-->
<!--        position: absolute;-->
<!--        top: 0;-->
<!--        left: 0;-->
<!--        width: 100%;-->
<!--        height: 100%;-->
<!--        background: rgba(0,0,0,0.5); /* Semi-transparent dim */-->
<!--        display: flex;-->
<!--        align-items: center;-->
<!--        justify-content: center;-->
<!--        z-index: 10;-->
<!--    }-->

<!--    .start-btn {-->
<!--        padding: 15px 40px;-->
<!--        font-size: 24px;-->
<!--        font-weight: bold;-->
<!--        color: white;-->
<!--        background-color: #5865F2; /* Discord Blurple */-->
<!--        border: none;-->
<!--        border-radius: 8px;-->
<!--        cursor: pointer;-->
<!--        transition: transform 0.2s;-->
<!--    }-->
<!--    .start-btn:hover {-->
<!--        transform: scale(1.05);-->
<!--        background-color: #4752C4;-->
<!--    }-->
<!--</style>-->