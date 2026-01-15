const words = ["Web Developer", "QA Engineer", "Learner"];
let wordIndex = 0;
let charIndex = 0;
let currentWord = "";
let isDeleting = false;

function typeEffect() {
    if (!isDeleting) {
        currentWord = words[wordIndex].substring(0, charIndex + 1);
        charIndex++;
    } else {
        currentWord = words[wordIndex].substring(0, charIndex - 1);
        charIndex--;
    }

    document.getElementById("text").innerText = currentWord;

    let speed = isDeleting ? 80 : 150;

    if (!isDeleting && charIndex === words[wordIndex].length) {
        speed = 1000;
        isDeleting = true;
    }

    if (isDeleting && charIndex === 0) {
        isDeleting = false;
        wordIndex = (wordIndex + 1) % words.length;
    }

    setTimeout(typeEffect, speed);
}

typeEffect();

