mergeInto(LibraryManager.library, {
  // This function matches the [DllImport] name in C#
  RequestDiscordData: function () {
    try {
      // We call a global function that we will define in your Svelte app
      if (window.dispatchDiscordData) {
        window.dispatchDiscordData();
      } else {
        console.warn("[Unity] window.dispatchDiscordData is not defined yet.");
      }
    } catch (e) {
      console.error("[Unity] Error requesting Discord data: " + e);
    }
  },
});