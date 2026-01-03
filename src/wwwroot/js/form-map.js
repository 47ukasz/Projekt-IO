const formMap = L.map('map').setView([52.2297, 21.0122], 12);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors'
}).addTo(formMap);

let marker = null;

function setLocation(lat, lng) {
    document.getElementById('Lat').value = lat;
    document.getElementById('Lng').value = lng;

    document.getElementById('coordsInfo').textContent =
        `Wybrano: ${lat.toFixed(6)}, ${lng.toFixed(6)}`;

    if (!marker) marker = L.marker([lat, lng]).addTo(formMap);
    else marker.setLatLng([lat, lng]);
}

formMap.on('click', (e) => {
    setLocation(e.latlng.lat, e.latlng.lng);
});