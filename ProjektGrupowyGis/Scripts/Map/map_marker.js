var allMarkers = [];

function createMarker(place) {
    console.log("Create marker");
    var marker = new google.maps.Marker({
        map: map,
        draggable: true,
        position: place.geometry.location,
        clickable: true
    });

    var content = createMarkerContent(place);

    marker.info = new google.maps.InfoWindow({
        content: content,
        maxHeight: 200,
        
    });
    google.maps.event.addListener(marker, 'click', function() {
    	marker.info.open(map, marker);
    });
    allMarkers.push(marker);
}

function createMarkerDB(place, isAdmin) {
    console.log("Create marker");
    var markPos = new google.maps.LatLng(place.Lat, place.Lng)
    var marker = new google.maps.Marker({
        map: map,
        draggable: Boolean(isAdmin),
        position: markPos,
        clickable: true
    });
    google.maps.event.addListener(marker, 'dragend', function () {
        geocodePosition(marker.getPosition(), marker, place);       
        
    });

    var content = createMarkerContentDB(place);

    marker.info = new google.maps.InfoWindow({
        content: content,
        maxHeight: 200,

    });
    google.maps.event.addListener(marker, 'click', function () {
        marker.info.open(map, marker);
    });
    allMarkers.push(marker);
}

function geocodePosition(pos, marker, place) {
    geocoder = new google.maps.Geocoder();
    geocoder.geocode
        ({
            latLng: pos
        },
        function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                console.log(results);
                place.FullAddress = results[0].formatted_address;
                place.Lat = marker.getPosition().lat();
                place.Lng = marker.getPosition().lng();

                var content = createMarkerContentDB(place);
                marker.info = null;
                marker.info = new google.maps.InfoWindow({
                    content: content,
                    maxHeight: 200
                });

                changeHotelAddress(place);
                return results;
                $("#mapErrorMsg").hide(100);
            }
            else {
                $("#mapErrorMsg").html('Cannot determine address at this location.' + status).show(100);
                return "";
            }
        }
        );
}

function changeHotelAddress(place) {
    $.ajax({
        url: "/Hotels/ChangeHotelAddress",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(place)
    });
}

function createMarkerContentDB(place) {
    var hostnameRegexp = new RegExp('^https?://.+?/');
    var content = '<div class="scrollFix"><b><a href="' + place.Webpage +
        '"><br>' + place.Name + '</a></b><br>' + place.FullAddress;

    if (place.Phone) {
        content += "<br>" + place.Phone;
    }
    if (place.Rate) {
        var ratingHtml = '';
        for (var i = 0; i < 5; i++) {
            if (place.Rate < (i + 0.5)) {
                ratingHtml += '&#10025;';
            } else {
                ratingHtml += '&#10029;';
            }
        }
        content += "<br>" + ratingHtml;
    }
    if (place.Website) {
        var fullUrl = place.Qebsite;
        var website = hostnameRegexp.exec(place.Website);
        if (website === null) {
            website = 'http://' + place.Website + '/';
            fullUrl = website;
        }
        content += "<br><a href='" + website + "'>" + website + "</a>";
    }
    content += "</div>";
    return content;
}

function createMarkerContent(place) {
    var hostnameRegexp = new RegExp('^https?://.+?/');
    var content = '<div class="scrollFix"><img class="hotelIcon" ' +
        'src="' + place.icon + '" height="25" width="25" /> ' + '<b><a href="' + place.url +
        '"><br>' + place.name + '</a></b><br>' + place.vicinity;

    if (place.formatted_phone_number) {
        content += "<br>" + place.formatted_phone_number;
    } 
    if (place.rating) {
        var ratingHtml = '';
        for (var i = 0; i < 5; i++) {
            if (place.rating < (i + 0.5)) {
                ratingHtml += '&#10025;';
            } else {
                ratingHtml += '&#10029;';
            }
        }
        content += "<br>" + ratingHtml;
    } 
    if (place.website) {
        var fullUrl = place.website;
        var website = hostnameRegexp.exec(place.website);
        if (website === null) {
            website = 'http://' + place.website + '/';
            fullUrl = website;
        }
        content += "<br><a href='" + website + "'>" + website + "</a>";
    }
    content += "</div>";
    return content;
}

function clearMarkers() {
    infoWindow.close();
    for (var i = 0; i < allMarkers.length; i++) {
        allMarkers[i].setMap(null);
    }
    allMarkers = [];
    removePlacesfromList();
}

function find_closest_marker(lat1, lon1) {
    var pi = Math.PI;
    var R = 6371;
    var distances = [];
    var closest = -1;
    for (i = 0; i < allMarkers.length; i++) {
        var lat2 = allMarkers[i].position.lat();
        var lon2 = allMarkers[i].position.lng();

        var chLat = lat2 - lat1;
        var chLon = lon2 - lon1;
        var dLat = chLat * (pi / 180);
        var dLon = chLon * (pi / 180);

        var rLat1 = lat1 * (pi / 180);
        var rLat2 = lat2 * (pi / 180);

        var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) + Math.sin(dLon / 2) * Math.sin(dLon / 2) * Math.cos(rLat1) * Math.cos(rLat2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        var d = R * c;

        distances[i] = d;
        if (closest == -1 || d < distances[closest]) {
            closest = i;
        }
    }
    return closest;
}