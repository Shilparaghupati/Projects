window.addEventListener("scroll", () => {
    const scrolled = window.pageYOffset;
    const parallaxSections = document.querySelectorAll(".parallax");

    parallaxSections.forEach(section => {
        section.style.backgroundPositionY = -(scrolled * 0.3) + "px";
    });
});
