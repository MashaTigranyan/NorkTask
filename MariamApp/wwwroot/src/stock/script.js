document.addEventListener("DOMContentLoaded", function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();
    
    connection.start()
        .then(() => console.log("Connected to SignalR"))
        .catch(err => console.error("SignalR Connection Error: ", err));
    
    connection.on("ReceiveStockUpdate", (message, productName, newStockLevel) => {
        console.log(`Stock Update: ${productName} - New Level: ${newStockLevel}`);

        const messageElement = document.getElementById('message');
        const productElement = document.getElementById('product-element');
        const stockElement = document.getElementById('stock-element');

        if (productElement) {
            productElement.innerText = `Product Name: ${productName}`;
        }
        if (messageElement) {
            messageElement.innerText = message;
        }
        
        if (stockElement) {
            stockElement.innerText = `Stock Level: ${newStockLevel}`;
        }
        
    });
});
