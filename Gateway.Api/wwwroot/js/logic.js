const submitButton = document.getElementById("submitButton");

function getBaseUrl() {
    return window.location.origin;
}

function readInputs() {
    const emailId = document.getElementById("emailInput").value;
    const password = document.getElementById("passwordInput").value;

    return { emailId, password };
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
        const errorMessage = loginResponse.text;
        console.log(errorMessage);
    }
    else {
        const apiRepsonse = await loginResponse.json();
        localStorage.setItem("accessToken", apiRepsonse.data.accessToken);
        localStorage.setItem("refreshToken", apiRepsonse.data.refreshToken);

        const params = new URLSearchParams(window.location.search);
        const redirectUrl = params.get("redirect") || "/";

        window.location.href = redirectUrl;
    }
}

submitButton.addEventListener("click", onFormSubmit);
