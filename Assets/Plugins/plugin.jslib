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
});