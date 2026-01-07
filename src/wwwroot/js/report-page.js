const map = L.map("map").setView([52.23, 21.01], 12);

L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap",
}).addTo(map);

const MarkerIcon = L.Icon.extend({
    options: {
        iconSize: [60, 60],
    },
});

const reportMarkerIcon = new MarkerIcon({ iconUrl: "/img/report-marker.png" });

const marker = L.marker([52.23, 21.01], {
    icon: reportMarkerIcon,
}).addTo(map);
