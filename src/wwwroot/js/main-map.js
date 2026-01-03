const filterBtn = document.querySelector('.map-btn');
const filter = document.querySelector('.map-filter');

filterBtn.addEventListener('click', () => {
    filter.classList.toggle('map-filter-show');
})