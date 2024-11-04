var _notificationsService = abp.services.app.notificationManagement;
var _appUserNotificationHelper = new app.UserNotificationHelper();
var userId = abp.session.userId;
var messaging = firebase.messaging();
messaging.usePublicVapidKey('BKfx-TVY0OAgsOxZCOy7myb0mzWBE0PTq-MZdurSgFaCdg4I3RulwJ5rSnZsp9IPSEe_N8cAcaOPVzjjsFdtkcw');

// [START refresh_token]
// Callback fired if Instance ID token is updated.
messaging.onTokenRefresh(() => {
    messaging.getToken().then((refreshedToken) => {
        console.log('Token refreshed.');
        // Indicate that the new Instance ID token has not yet been sent to the
        // app server.
        removeTokenSentToServer(userId);
        // Send Instance ID token to app server.
        sendTokenToServer(refreshedToken);
        // [START_EXCLUDE]
        // Display new Instance ID token and clear UI of all previous messages.
        resetUI();
        // [END_EXCLUDE]
    }).catch((err) => {
        console.log('Unable to retrieve refreshed token ', err);
        showToken('Unable to retrieve refreshed token ', err);
    });
});

messaging.onMessage((payload) => {
    console.log('Message received. ', payload);
    // [START_EXCLUDE]
    // Update the UI to include the received message.
    notificationPayload(payload);
    // [END_EXCLUDE]
});
function requestPermission() {
    console.log('Requesting permission...');
    // [START request_permission]
    Notification.requestPermission().then((permission) => {
        if (permission === 'granted') {
            console.log(permission);
            console.log('Notification permission granted.');
            // TODO(developer): Retrieve an Instance ID token for use with FCM.
            // [START_EXCLUDE]
            // In many cases once an app has been granted notification permission,
            // it should update its UI reflecting this.
            resetUI();
            // [END_EXCLUDE]
        } else {
            console.log('Unable to get permission to notify.');
        }
    });
    // [END request_permission]
}
function resetUI() {
    showToken('loading...');
    // [START get_token]
    // Get Instance ID token. Initially this makes a network call, once retrieved
    // subsequent calls to getToken will return from cache.
    messaging.getToken().then((currentToken) => {
        if (currentToken) {
            sendTokenToServer(currentToken);
            showToken(currentToken);
        } else {
            // Show permission request.
            console.log('No Instance ID token available. Request permission to generate one.');
            // Show permission UI.
            localStorage.removeItem(userId);
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
        showToken('Error retrieving Instance ID token. ', err);
        localStorage.removeItem(userId);
    });
    // [END get_token]
}


function sendTokenToServer(currentToken) {
    //console.log(userId);
    if (!isTokenSentToServer(userId)) {
        console.log('Sending token to server...');
        // TODO(developer): Send the current token to your server.
        subcribeToken(currentToken);
        setTokenSentToServer(userId);
    } else {
        console.log('Token already sent to server so won\'t send it again ' +
            'unless it changes');
    }
}
function showToken(currentToken) {
    console.log('Token:' + currentToken);
}
function isTokenSentToServer(userId) {
    return window.localStorage.getItem('sentToServer') === userId.toString();
}

function setTokenSentToServer(userId) {
    window.localStorage.setItem('sentToServer', userId);
}
function removeTokenSentToServer(userId) {
    window.localStorage.removeItem('sentToServer', userId);
}
function notificationPayload(payload) {
    console.log(JSON.stringify(payload, null, 2));
    var notificationTitle = payload.notification.title;
    var notificationOptions = {
        body: payload.notification.body,
        icon: '/habassets/images/icons/notifications/' + payload.notification.icon,
        image: payload.notification.image,
        click_action: payload.notification.link,//_appUserNotificationHelper.getUrl(payload.data),//payload.data.click_action, // To handle notification click when notification is moved to notification tray
        data: {
            click_action: payload.notification.link//_appUserNotificationHelper.getUrl(payload.data)//payload.data.click_action
        }
    };
    var notification = new Notification(notificationTitle, notificationOptions);
    self.addEventListener('notificationclick', function (event) {

        //console.log(event.notification.data.click_action);
        if (!event.action) {
            console.log('Notification Click.');
            notification.clients.openWindow(event.notification.notification.click_action, '_blank');
        }
    });

    //abp.notify.info(payload.data.body);
    _appUserNotificationHelper.loadNotifications();
}

function deleteToken() {
    // Delete Instance ID token.
    // [START delete_token]
    messaging.getToken().then((currentToken) => {
        messaging.deleteToken(currentToken).then(() => {
            console.log('Token deleted.');
            removeTokenSentToServer(userId);
            unsubcribeToken(currentToken);
            // [START_EXCLUDE]
            // Once token is deleted update UI.
            //resetUI();
            // [END_EXCLUDE]
        }).catch((err) => {
            console.log('Unable to delete token. ', err);
        });
        // [END delete_token]
    }).catch((err) => {
        console.log('Error retrieving Instance ID token. ', err);
        showToken('Error retrieving Instance ID token. ', err);
    });

}


//---------------Function
function subcribeToken(token) {
    var input = {
        Token: token,
        Channel: 'WEB'
    };
    _notificationsService.subscribe(
        input
    ).done(function (rs) {
        console.log('subcribeToken success');
    }).always(function () {

    });
}
function unsubcribeToken(token) {
    _notificationsService.unSubscribe(
        token
    ).done(function (rs) {
        console.log('unsubcribeToken success');
    }).always(function () {

    });
}

$(document).ready(function () {

    //document.cookie = "web_view_url=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjI4NCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiIwOTY5ODk4ODM2IiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJPRE5RQVBJSFRDN0lFQVBBWVAzQ0k0SklMQkdFUFJTNiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJVc2VyIiwiQWRtaW4iLCJIQUIiLCJDbHViT3duZXIiXSwiaHR0cDovL3d3dy5hc3BuZXRib2lsZXJwbGF0ZS5jb20vaWRlbnRpdHkvY2xhaW1zL3RlbmFudElkIjoiMSIsIkFwcGxpY2F0aW9uX1VzZXJFbWFpbCI6ImhvYW5nbHRAem9wb3N0LnZuIiwiQXBwbGljYXRpb25fQWNjb3VudENvZGUiOiJLSF8wMDAwMjg0IiwiQXBwbGljYXRpb25fVXNlck5hbWUiOiIwOTY5ODk4ODM2IiwiQXBwbGljYXRpb25fUGhvbmVOdW1iZXIiOiIwOTY5ODk4ODM2IiwiQXBwbGljYXRpb25fRnVsbE5hbWUiOiJob2FuZyBuZXcgSG_DoG5nIiwic3ViIjoiMjg0IiwianRpIjoiMzJkY2YzOTItNTdmYi00MzgxLThhMGYtYjJmZTQ3MzAyMzMyIiwiaWF0IjoxNTY1MTY4OTkzLCJ0b2tlbl92YWxpZGl0eV9rZXkiOiIxOWQwM2I1Mi1kNjNiLTRiNzItODVmZS1lOWE1Y2JlOGE4NmYiLCJ1c2VyX2lkZW50aWZpZXIiOiIyODRAMSIsIm5iZiI6MTU2NTE2ODk5MywiZXhwIjoxNTY1MjU1MzkzLCJpc3MiOiJTYWxlU3lzdGVtIiwiYXVkIjoiU2FsZVN5c3RlbSJ9.llFaQje1ZM3wVpDzh7TfQLhAYQa6qCgilvElu2v4-3k"
    requestPermission();
    //$("#btnDelete").click(function () {
    //    deleteToken();
    //});

    _appUserNotificationHelper.loadNotifications();

});
