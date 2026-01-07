const filterBtn = document.querySelector('.map-btn');
const filter = document.querySelector('.map-filter');

filterBtn.addEventListener('click', () => {
    filter.classList.toggle('map-filter-show');
});

const MarkerIcon = L.Icon.extend({
    options: {
        iconSize: [60,60],
    }
})

const reportMarkerIcon = new MarkerIcon({iconUrl: "/img/report-marker.png"})
const sightMarkerIcon = new MarkerIcon({iconUrl: "/img/sight-marker.png"})

let startingCordsLat = 52.23;
let startingCordsLon = 21.01;

if (cordsFromFilter) {
    startingCordsLat = cordsFromFilter.lat;
    startingCordsLon = cordsFromFilter.lon;
}

const map = L.map('map').setView([startingCordsLat, startingCordsLon], 12);

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
        const popupHtml = `
            <div class="map-popup">
                <div class="map-popup-header">
                    <img src="${p.PhotoPath !== "" ? p.PhotoPath : "/uploads/default-animal.png"}" alt="Zdjęcie zwierzaka"/>
                    <div class="map-popup-info">
                        <p class="map-popup-type"><i class="fa-solid fa-flag" style="color: #78c841;"></i> <span>${p.Type}</span></p>
                        <p class="map-popup-species" style="margin: .5rem 0;">Zaginiony/na ${p.Species}</p>
                        <div>
                            <p class="map-popup-status ${p.Status === "zaginiony" ? "status-lost" : "status-found"}"><span></span>${p.Status}</p>
                            <a href="/report/${p.LostReportId}" class="map-popup-page"> <i class="fa-solid fa-angle-right fa-xl" style="color: #78c841;"></i> </a>
                        </div>
                    </div>
                </div>
                <div class="map-popup-buttons">
                    <a class="popup-chat-button">
                       <i class="fa-solid fa-message" style="color: #ff9b2f;"></i>
                        <span>Rozpocznij chat</span>
                    </a>

                    ${
                        p.Type === "zgłoszenie"
                            ? `<a href="/sighting/create/${p.LostReportId}" class="popup-sight-button">
                                    <i class="fa-solid fa-circle-plus" style="color: #fff;"></i>
                                    <span>Dodaj doniesienie</span>
                               </a>
                            ` : ""
                    }
                </div>
            </div>`;

        const marker = L.marker([p.Latitude, p.Longitude], {icon: p.Type === "doniesienie" ? sightMarkerIcon : reportMarkerIcon}).addTo(map).bindPopup(popupHtml);

        markers.push(marker);
    });
}

document.addEventListener('DOMContentLoaded', () => {
    renderPoints(initialPoints);
});
