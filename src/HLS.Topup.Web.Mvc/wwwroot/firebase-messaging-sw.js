importScripts('/view-resources/Views/Notification/firebase-app.js');
importScripts('/view-resources/Views/Notification/firebase-messaging.js');
// importScripts('~/view-resources/Views/Notification/init.js');
var firebaseConfig = {
    apiKey: "AIzaSyBKUNoEv1XetPHKwyEyb21wqK-L2ZvrPHE",
    authDomain: "habnotificaion.firebaseapp.com",
    databaseURL: "https://habnotificaion.firebaseio.com",
    projectId: "habnotificaion",
    storageBucket: "",
    messagingSenderId: "587466292773",
    appId: "1:587466292773:web:c08b2a086b13617b"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
var messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function (payload) {
    var _appUserNotificationHelper = new app.UserNotificationHelper();
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    var notificationTitle = payload.data.title;
    var notificationOptions = {
        body: payload.data.body,
        icon: '/habassets/images/icons/notifications/' + payload.data.icon,
        image: payload.data.image,
        click_action: payload.data.link//_appUserNotificationHelper.getUrl(payload.data)
    };

    return self.registration.showNotification(notificationTitle,
        notificationOptions);
});
// [END background_handler]
