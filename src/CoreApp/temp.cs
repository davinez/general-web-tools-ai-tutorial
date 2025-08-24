const connection = new signalR.HubConnectionBuilder()
    .withUrl("/eventStatusHub")
    .build();

connection.on("ReceiveStatusUpdate", (eventId, status) => {
    console.log(`Event ${eventId} status updated to: ${status}`);
    // Now you can call your API to get the full event details
    fetch(`/api/eventstatus/${eventId}`)
        .then(response => response.json())
        .then(data => {
            // Update your UI with the new status
            // If the status is "Processed", you can enable the download button
        });
});

connection.start().catch(err => console.error(err.toString()));