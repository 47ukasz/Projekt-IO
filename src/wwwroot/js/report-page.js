let startingCordsLat = 52.23;
let startingCordsLon = 21.01;

if (lostReportCords) {
    startingCordsLat = lostReportCords.lat;
    startingCordsLon = lostReportCords.lon;
}

const map = L.map("map").setView([startingCordsLat, startingCordsLon], 12);

L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap",
}).addTo(map);

const MarkerIcon = L.Icon.extend({
    options: {
        iconSize: [60, 60],
    },
});

const reportMarkerIcon = new MarkerIcon({ iconUrl: "/img/report-marker.png" });

const marker = L.marker([startingCordsLat, startingCordsLon], {
    icon: reportMarkerIcon,
}).addTo(map);
