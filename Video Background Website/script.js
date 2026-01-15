const video = document.getElementById("bgVideo");
const button = document.querySelector("button");

function toggleVideo() {
    if (video.paused) {
        video.play();
        button.innerText = "Pause Video";
    } else {
        video.pause();
        button.innerText = "Play Video";
    }
}
