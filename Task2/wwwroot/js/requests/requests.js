export async function getFiles(callback) {
    const response = await fetch("/api/files");

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body
        const responseData = await response.json();
        console.log(responseData);

        callback(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}

export async function loadFile(file) {
    const response = await fetch("/api/files/load", {
        method: 'POST',
        body: file
    });

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body
        const responseData = await response.json();
        console.log(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}