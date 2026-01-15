const popup = document.getElementById("popup");
const page = document.getElementById("pageContent");

function openPopup() {
    popup.style.display = "flex";
    page.classList.add("blur");
}

function closePopup() {
    popup.style.display = "none";
    page.classList.remove("blur");
}

function submitForm() {
    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();

    // Gmail-only validation
    const gmailPattern = /^[a-zA-Z0-9._%+-]+@gmail\.com$/;

    if (name === "" || email === "") {
        alert("Please fill all fields");
        return;
    }

    if (!gmailPattern.test(email)) {
        alert("Email must be in @gmail.com format");
        return;
    }

    alert(`Success ✅\nName: ${name}\nEmail: ${email}`);
    closePopup();
}

/* Close popup when clicking outside */
popup.addEventListener("click", (e) => {
    if (e.target === popup) {
        closePopup();
    }
});
