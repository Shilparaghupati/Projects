function goToOptions() {
    document.getElementById("slide1").classList.remove("active");
    document.getElementById("slide2").classList.add("active");
}

function generateAnime() {
    const img = document.getElementById("animeImg");
    const category = document.getElementById("category").value;

    img.src = "https://via.placeholder.com/300x300?text=Loading...";

    fetch(`https://api.waifu.pics/sfw/${category}`)
        .then(response => response.json())
        .then(data => {
            img.src = data.url;
        })
        .catch(() => {
            alert("Failed to load anime image 😢");
        });
}
