mergeInto(LibraryManager.library, {
  ShowAlert: function (text) {
    if (window && window.Telegram && window.Telegram.WebApp) {
      window.Telegram.WebApp.BackButton.showAlert(UTF8ToString(text));
    }
  },
  HapticFeedback: function (level) {
    if (window && window.Telegram && window.Telegram.WebApp) {
      window.Telegram.WebApp.HapticFeedback.notificationOccurred(
        UTF8ToString(level)
      );
    }
  },
  GetUserData: function () {
    if (window && window.Telegram && window.Telegram.WebApp) {
      const userdata = window.Telegram.WebApp.initDataUnsafe;
      var userData = JSON.stringify(userdata);
      var bufferSize = lengthBytesUTF8(userData) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(userData, buffer, bufferSize);
      return buffer;
    } else {
      return null;
    }
  },
  GetUserId: function () {
    console.log("GetUserId");
    if (window && window.Telegram && window.Telegram.WebApp) {
      const userdata = window.Telegram.WebApp.initDataUnsafe;
      const userId = userdata.user.id;
      return userId;
    }
  },
  Validate: function (url) {
    if (window.Telegram.WebApp) {
      (async () => {
        const l = UTF8ToString(url);
        const initData = window.Telegram.WebApp.initData;
        console.log({ initData });
        const response = await fetch(`${l}validate`, {
          body: initData,
          method: "POST",
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
          },
        })
          .then((res) => res.json())
          .catch((err) => console.log({ err }));
        console.log({ response });
      })();
    }
  },
  Screenshot: function (base64String) {
    console.log("Sharing image started");
    try {
      // Decode base64 to binary data
      const base64 = UTF8ToString(base64String);
      const binaryString = atob(base64); // decode base64 string
      const len = binaryString.length;
      const bytes = new Uint8Array(len);
      for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
      }
      // Create a Blob from the byte array
      const blob = new Blob([bytes], { type: "image/png" });
      // Create a File object from the Blob
      const file = new File([blob], "Flappy_Pav.png", { type: "image/png" });
      const shareData = {
        text: "Check Out My High Score!\nI just played Flappy Pav! Think you can beat me? Join the community now and give it a try: https://t.me/flappy_pav_bot. Let's see who can get the highest score!",
        title: "Check Out My High Score!",
        files: [file], // Sharing the image file
      };
      // Check if sharing is supported
      if (navigator.canShare && navigator.canShare(shareData)) {
        navigator
          .share(shareData)
          .then(() => console.log("Image shared successfully"))
          .catch((err) => console.error("Error sharing", err));
      } else {
        console.log("Sharing not supported on this browser or device");
        if (window && window.Telegram && window.Telegram.WebApp) {
          window.Telegram.WebApp.showAlert(
            "OOPS Sharing not supported on this browser or device"
          );
        }
      }
    } catch (exception) {
      console.log({ exception });
    }
  },
  ShareInviteLink: function (link) {
    const l = UTF8ToString(link);
    (async (l) => {
      let shared = false;
      const shareData = {
        text: `Want to join the fun and access the $PAV Airdrop?\nConnect with me and start playing:${l}.\nLet's build the best gaming community on TON together`,
        title: "Join Me in Flappy Pav!",
      };
      try {
        if (navigator && navigator.canShare(shareData)) {
          await navigator.share(shareData);
          shared = true;
        } else {
          console.log("Sharing not supported", shareData);
        }
      } catch (err) {
        if (err.name !== "AbortError") {
          console.log(err.name, err.message);
        }
      }
      try {
        if (navigator) {
          await navigator.clipboard.writeText(
            `Want to join the fun and access the $PAV Airdrop?\nConnect with me and start playing:${l}.\nLet's build the best gaming community on TON together`
          );
          if (!shared) {
            window.Telegram.WebApp.showAlert(
              "Referral Copied! Let's build a top gaming community on TON!"
            );
          }
        }
      } catch (err) {
        console.log("URL not copied : " + err);
      }
    })(l);
  },
});