    const slides = document.querySelectorAll('.slide');
    let currentSlide = 0;

    function showNextSlide() {
        slides[currentSlide].classList.remove('active');
        currentSlide = (currentSlide + 1) % slides.length;
        slides[currentSlide].classList.add('active');
    }

    setInterval(showNextSlide, 3000); // Change image every 5 seconds

    // Initialize the first slide as active
    slides[0].classList.add('active');





 // Change slide every 3 seconds

/////////////////////////////////////////////////////////

const serviceCards = document.querySelectorAll('.service-card');

// Add hover event listener to each card
serviceCards.forEach(card => {
    card.addEventListener('mouseover', () => {
    // Get the mask-group image within the card
    const maskGroup = card.querySelector('.mask-group');

    // Change the source of the mask-group image 
    maskGroup.src = 'HomePageImages/mask-group2.svg'; 
    });

  // Add mouseout event listener to each card to revert the image on mouse out
    card.addEventListener('mouseout', () => {
    // Get the mask-group image within the card
    const maskGroup = card.querySelector('.mask-group');

    // Change the source of the mask-group image 
    maskGroup.src = 'HomePageImages/mask-group0.svg'; 
    });
});