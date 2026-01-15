const testimonials = [
    {
        photo: "/Users/shilpa/Downloads/shilpa.jpeg",
        description: "This project helped me improve my frontend development skills using HTML, CSS, and JavaScript.",
        name: "Shilpa R"
    },
    {
        photo: "https://i.pravatar.cc/150?img=11",
        description: "The testimonial slider UI looks modern and professional.",
        name: "Ananya"
    },
    {
        photo: "https://i.pravatar.cc/150?img=12",
        description: "Smooth transitions and clean design make it user friendly.",
        name: "Rahul"
    },
    {
        photo: "https://i.pravatar.cc/150?img=13",
        description: "This project demonstrates good DOM manipulation skills.",
        name: "Sneha"
    },
    {
        photo: "https://i.pravatar.cc/150?img=14",
        description: "A simple yet effective testimonial slider implementation.",
        name: "Karthik"
    }
];

let index = 0;

function showTestimonial() {
    document.getElementById("photo").src = testimonials[index].photo;
    document.getElementById("description").innerText = testimonials[index].description;
    document.getElementById("name").innerText = testimonials[index].name;
}

function next() {
    index = (index + 1) % testimonials.length;
    showTestimonial();
}

function prev() {
    index = (index - 1 + testimonials.length) % testimonials.length;
    showTestimonial();
}
