const submitButton = document.getElementById("submitButton");

function getBaseUrl() {
    return window.location.origin;
}

function readInputs() {
    const email = document.getElementById("emailInput").value;
    const password = document.getElementById("passwordInput").value;

    return { email, password };
}

async function onFormSubmit(e) {
    e.preventDefault();

    const userInputs = readInputs();

    const baseUrl = getBaseUrl();
    const loginResponse = await fetch(`${baseUrl}/api/auth/v1/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userInputs)
    });

    if (!loginResponse.ok) {
        const errorMessage = await response.text;
        console.log(errorMessage);
    }
    else {
        const apiRepsonse = await response.json();
        console.log(apiRepsonse);
    }
}

submitButton.addEventListener("click", onFormSubmit);
