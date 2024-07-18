// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


function validateWeatherRequestFormCoordinate() {
    console.log("Function has been called!");
    let lat = document.forms["WeatherRequestFormCoordinate"]["Latitude"].value;
    let lon = document.forms["WeatherRequestFormCoordinate"]["Longitude"].value;
    console.log(lat);
    console.log(lon);
    var coordinateRegex = new RegExp("^[-]?[0-9]+[.,]?[0-9]*$");
    if (coordinateRegex.test(lat) && coordinateRegex.test(lon)) {
        console.log("works!");
    } else {
        alert("Input a decimal value representing your coordinates");
        return false;
    }

    if ((parseFloat(lat) >= -90.0 && parseFloat(lat) <= 90.0) && (parseFloat(lon) >= -180.0 && parseFloat(lon) <= 180.0)) {
        console.log("correct coordinates");
    } else {
        alert("Input latitude values must be between -90 and 90 degrees.\n Input longitude values must be between -180 and 180 degrees.");
        return false;
    }
}

function validateWeatherRequestFormCity() {
    
}