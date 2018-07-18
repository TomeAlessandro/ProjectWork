var map;
var state = false;
var interval;
var route = [];
var marker;
var marker2;

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(startPosition);
    } else {
        alert('La geo-localizzazione NON è possibile');
    }
}

function startPosition(position) {
    var div = document.getElementById('Telemetrie');

    var lat = position.coords.latitude;
    var lon = position.coords.longitude;
    route.push({ lat: lat, lng: lon });

    var msg = {
        Longitude: lon,
        Latitude: lat,
        Activity_Id: currentActivtyId,
        Moment: new Date().toISOString()
    };

    sendTelemetry(msg);

    div.innerHTML += '<br>' + lat + ',' + lon + '<br>' + new Date().toISOString() + '<br>';
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: lat, lng: lon },
        zoom: 12
    });

    marker = new google.maps.Marker({
        position: { lat: lat, lng: lon },
        map: map
    });
    marker2 = new google.maps.Marker({
        position: { lat: lat, lng: lon },
        map: map
    });

    console.log(route);
}

function getMoveLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(movePosition);
    } else {
        alert('La geo-localizzazione NON è possibile');
    }
}

function movePosition(position) {
    var div = document.getElementById('Telemetrie');

    var lat = position.coords.latitude;
    //var lat = Math.floor((Math.random() * 50) + 1);
    var lon = position.coords.longitude;
    //var lon = Math.floor((Math.random() * 50) + 1);

    var msg = {
        Longitude: lon,
        Latitude: lat,
        Activity_Id: currentActivtyId,
        Moment: new Date().toISOString()
    };

    sendTelemetry(msg);

    route.push({ lat: lat, lng: lon });
    console.log(route);

    marker2.setPosition({ lat: lat, lng: lon });

    var flightPath = new google.maps.Polyline({
        path: route,
        geodesic: true,
        strokeColor: '#ffc266',
        strokeOpacity: 1.0,
        strokeWeight: 3
    });

    flightPath.setMap(map);

    div.innerHTML += '<br>' + lat + ',' + lon + '<br>' + new Date().toISOString() + '<br>';
    map.setCenter({ lat: lat, lng: lon });

}

getLocation();


function startGeoL() {
    state = true;
    if (state === true) {
        interval = setInterval(getMoveLocation, 2000);
    }
}

function stopGeoL() {
    state = false;
    clearInterval(interval);

}

function sendTelemetry(msg) {
    $.ajax({
        type: "POST",
        url: "SendTelemetry",
        data: JSON.stringify(msg),
        headers: {
            'Accept': "application/json",
            'Content-Type': "application/json"
        },
        dataType: "json",
        success: function (data, stato) {
            //console.log(data);
        }
    });
}