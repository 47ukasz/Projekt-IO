const filterBtn = document.querySelector('.map-btn');
const filter = document.querySelector('.map-filter');

filterBtn.addEventListener('click', () => {
    filter.classList.toggle('map-filter-show');
});

const map = L.map('map').setView([52.23, 21.01], 12);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap'
}).addTo(map);

let markers = [];

function clearMarkers() {
    markers.forEach(m => map.removeLayer(m));
    markers = [];
}

function renderPoints(points) {
    clearMarkers();

    if (!points || points.length === 0) {
        console.warn('Brak danych do wyświetlenia');
        return;
    }

    points.forEach(p => {
        const marker = L.marker([p.Latitude, p.Longitude]).addTo(map);

        marker.bindTooltip(`
      <strong>${p.Title}</strong><br>
      ${p.Type} • ${p.Status}
    `, {
            direction: 'top',
            offset: [0, -10],
            opacity: 0.95
        });

        markers.push(marker);
    });
}

document.addEventListener('DOMContentLoaded', () => {
    renderPoints(initialPoints);
});
