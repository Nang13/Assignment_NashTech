// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleForms() {
    var loginForm = document.getElementById('loginForm');
    var registerForm = document.getElementById('registerForm');
    var authTitle = document.getElementById('auth-title');
    var toggleLink = document.getElementById('toggle-link');

    if (loginForm.style.display === 'none') {
        loginForm.style.display = 'block';
        registerForm.style.display = 'none';
        authTitle.textContent = 'Login to Organic Store';
        toggleLink.innerHTML = 'Don\'t have an account? <span class="toggle-link" onclick="toggleForms()">Register here</span>';
    } else {
        loginForm.style.display = 'none';
        registerForm.style.display = 'block';
        authTitle.textContent = 'Register at Organic Store';
        toggleLink.innerHTML = 'Already have an account? <span class="toggle-link" onclick="toggleForms()">Login here</span>';
    }
}

// Ensure forms are in the correct initial state
document.addEventListener('DOMContentLoaded', function () {

    const stars = document.querySelectorAll('.star-rating .fa-star');
    const ratingInput = document.getElementById('rating');

    stars.forEach(star => {
        star.addEventListener('mouseenter', () => {
            const value = star.getAttribute('data-value');
            highlightStars(value);
        });

        star.addEventListener('mouseleave', () => {
            const value = ratingInput.value;
            highlightStars(value);
        });

        star.addEventListener('click', () => {
            const value = star.getAttribute('data-value');
            ratingInput.value = value;
            highlightStars(value);
        });
    });

    function highlightStars(value) {
        stars.forEach(star => {
            if (star.getAttribute('data-value') <= value) {
                star.classList.add('selected');
            } else {
                star.classList.remove('selected');
            }
        });
    }

    //const reviewForm = document.getElementById('reviewForm');
    //reviewForm.addEventListener('submit', async (e) => {
    //    e.preventDefault();

    //    const reviewText = document.getElementById('reviewText').value;
    //    const rating = document.getElementById('rating').value;

    //    const formData = {
    //        review: reviewText,
    //        rating: rating
    //    };

    //    try {
    //        const response = await fetch('YOUR_API_ENDPOINT_HERE', {
    //            method: 'POST',
    //            headers: {
    //                'Content-Type': 'application/json'
    //            },
    //            body: JSON.stringify(formData)
    //        });

    //        if (response.ok) {
    //            alert('Review submitted successfully!');
    //        } else {
    //            alert('Failed to submit review');
    //        }
    //    } catch (error) {
    //        console.error('Error:', error);
    //        alert('Failed to submit review');
    //    }
    //});
});


