const latInput = document.getElementById("Lat");
const lngInput = document.getElementById("Lng");
const coordsInfo = document.getElementById("coordsInfo");

const hasCoords = latInput.value && lngInput.value;
const startLat = hasCoords ? parseFloat(latInput.value) : 52.23;
const startLng = hasCoords ? parseFloat(lngInput.value) : 21.01;

const formMap = L.map('map').setView([startLat, startLng], 12);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors'
}).addTo(formMap);

let marker = null;

if (hasCoords) {
    marker = L.marker([startLat, startLng]).addTo(formMap);
    coordsInfo.textContent = `Wybrano: ${startLat.toFixed(6)}, ${startLng.toFixed(6)}`;
}

function setLocation(lat, lng) {
    latInput.value = lat;
    lngInput.value = lng;

    coordsInfo.textContent = `Wybrano: ${lat.toFixed(6)}, ${lng.toFixed(6)}`;
    
    if (!marker) {
        marker = L.marker([lat, lng]).addTo(formMap);
    } else {
        marker.setLatLng([lat, lng]);
    }
}

formMap.on('click', (e) => {
    setLocation(e.latlng.lat, e.latlng.lng);
});