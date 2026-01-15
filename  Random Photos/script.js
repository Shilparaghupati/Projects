const images = {
    nature: [
        "/Users/shilpa/Downloads/nature/nature 6.webp",
        "/Users/shilpa/Downloads/nature/nature 7.jpeg",
        "/Users/shilpa/Downloads/nature/nature1.jpg",
        "/Users/shilpa/Downloads/nature/nature3.jpeg",
        "/Users/shilpa/Downloads/nature/nature4.webp",
        "/Users/shilpa/Downloads/nature/nature5.jpeg",
        "/Users/shilpa/Downloads/nature/nature7.jpeg"
    ],
    motivation: [
        "/Users/shilpa/Downloads/motivation 5.jpeg",
        "/Users/shilpa/Downloads/motivation 4.jpeg",
        "/Users/shilpa/Downloads/motivation 7.jpeg",
        "/Users/shilpa/Downloads/motivate6.jpeg", 
        "/Users/shilpa/Downloads/motivate3.jpeg", 
        "/Users/shilpa/Downloads/motivate2.jpeg",
        "/Users/shilpa/Downloads/motivate1.jpeg"
    ],
    anime: [
        "/Users/shilpa/Downloads/anime1.jpeg",
        "/Users/shilpa/Downloads/anime2.jpeg",
        "/Users/shilpa/Downloads/anime3.jpeg",
        "/Users/shilpa/Downloads/anime4.jpeg",
        "/Users/shilpa/Downloads/anime5.jpeg",
        "/Users/shilpa/Downloads/anime6.jpeg",
        "/Users/shilpa/Downloads/anime7.jpeg"
        
    ],
    home: [
        "/Users/shilpa/Downloads/home1.jpeg",
        "/Users/shilpa/Downloads/home2.jpeg", 
        "/Users/shilpa/Downloads/home3.jpeg",
        "/Users/shilpa/Downloads/home4.jpeg",
        "/Users/shilpa/Downloads/home5.jpeg",
        "/Users/shilpa/Downloads/home6.jpeg",
        "/Users/shilpa/Downloads/home7.jpeg"
    ]
};

function loadPhotos() {
    const gallery = document.getElementById("gallery");
    const category = document.getElementById("category").value;

    gallery.innerHTML = "";

    images[category].forEach(src => {
        const img = document.createElement("img");
        img.src = src;
        gallery.appendChild(img);
    });
}
