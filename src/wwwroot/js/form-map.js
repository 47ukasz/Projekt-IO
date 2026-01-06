const locationLatSpan = document.getElementById("cord-lat");
const locationLonSpan = document.getElementById("cord-lon");
const latInput = document.getElementById("Lat");
const lngInput = document.getElementById("Lng");

const hasCoords = latInput.value && lngInput.value;
const startLat = hasCoords ? parseFloat(latInput.value) : 52.23;
const startLng = hasCoords ? parseFloat(lngInput.value) : 21.01;
let marker = null;

const formMap = L.map("map", {
    zoomControl: false,
}).setView([startLat, startLng], 12);

L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap",
}).addTo(formMap);

const MarkerIcon = L.Icon.extend({
    options: {
        iconSize: [60, 60],
    },
});

const formMarkerIcon = new MarkerIcon({
    iconUrl: "/img/form-map-marker.png",
});

if (hasCoords) {
    marker = L.marker([startLat, startLng], { icon: formMarkerIcon }).addTo(
        formMap
    );
    locationLatSpan.textContent = `${lat.toFixed(4)} °N`;
    locationLonSpan.textContent = `${lng.toFixed(4)} °E`;
}

L.control
    .zoom({
        position: "bottomright",
    })
    .addTo(formMap);

const GeoControl = L.Control.extend({
    options: {
        position: "bottomright",
    },

    onAdd: function (formMap) {
        const btn = L.DomUtil.create("button", "form-map-geolocation");

        btn.type = "button";

        btn.innerHTML =
            "<i class='fa-solid fa-location-crosshairs' style='color: #9e9e9e;'></i>";
        btn.title = "Pokaż moją lokalizację";

        L.DomEvent.disableClickPropagation(btn);
        L.DomEvent.disableScrollPropagation(btn);

        L.DomEvent.on(btn, "click", (e) => {
            L.DomEvent.preventDefault(e);
            L.DomEvent.stopPropagation(e);

            formMap.locate({ setView: true, maxZoom: 16 });
        });

        return btn;
    },
});

formMap.addControl(new GeoControl());

function setLocation(lat, lng) {
    latInput.value = lat;
    lngInput.value = lng;

    locationLatSpan.textContent = `${lat.toFixed(4)} °N`;
    locationLonSpan.textContent = `${lng.toFixed(4)} °E`;

    if (!marker) {
        marker = L.marker([lat, lng], { icon: formMarkerIcon }).addTo(formMap);
    } else {
        marker.setLatLng([lat, lng]);
    }
}

formMap.on("click", (e) => {
    setLocation(e.latlng.lat, e.latlng.lng);
});
