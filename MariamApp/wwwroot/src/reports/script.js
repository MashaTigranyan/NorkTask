let connection;

document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("connect");
    
    button.addEventListener("click", () => {
        connection?.stop();
        
        const status = document.getElementById("status");
        const token = document.getElementById("token").value;
        
        connection = new signalR.HubConnectionBuilder()
            .withUrl(`/notificationHub?access_token=${token}`)
            .build();

        connection.start()
            .then(() => status.innerText = "Connected to SignalR")
            .catch(err => status.innerText = `SignalR Connection Error: ${err}`);

        connection.on("ReceiveReportNotification", (message) => {
            console.log(`File ready for download`);

            const messageElement = document.getElementById('message');
            if (messageElement) {
                messageElement.innerText = message;
            }
        });
    })
});