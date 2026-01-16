// Assets/Plugins/WebGL/TabCloseListener.jslib
mergeInto(LibraryManager.library, {

  RegisterTabCloseListener: function () {
    // Determine the visibility change event for different browsers
    var hidden, visibilityChange;
    if (typeof document.hidden !== "undefined") { 
      hidden = "hidden";
      visibilityChange = "visibilitychange";
    } else if (typeof document.msHidden !== "undefined") {
      hidden = "msHidden";
      visibilityChange = "msvisibilitychange";
    } else if (typeof document.webkitHidden !== "undefined") {
      hidden = "webkitHidden";
      visibilityChange = "webkitvisibilitychange";
    }

    // Listener for Tab Close / Refresh
    window.addEventListener("beforeunload", function (event) {
       // SendMessage('GameObject', 'Method', 'Parameter')
       // 'WebGLLifecycle' is the name of the GameObject we will create in Step 2
       // 'OnBrowserClose' is the method name we will call
       Module.SendMessage('WebGLLifecycle', 'OnBrowserClose');
    });

    // Listener for Tab Switching (Optional: strictly for closing/unloading logic)
    window.addEventListener("pagehide", function (event) {
       Module.SendMessage('WebGLLifecycle', 'OnBrowserClose');
    });
  }
});