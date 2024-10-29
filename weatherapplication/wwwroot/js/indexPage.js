

function getExampleWeatherData() {
    $.ajax({
        type: 'GET',
        url: "/?handler=Defaults",
        dataType: "json",
        async: true,
        headers: {
            "Access-Control-Allow-Origin": "*"
        }
    }).done(function(result) {
        console.log("Completed!");
        console.log(result);
        var weather_data = JSON.parse(result);
        console.log(weather_data);

        for (const key in weather_data) {
            console.log(key);
            console.log(weather_data[key]["main"]["temp"])
            console.log($(`#${key}`));
            $(`#${key} .weather-display-default-city-weather-info .default-city-description`).text(weather_data[key]["weather"][0]["description"]);
            $(`#${key} .weather-display-default-city-weather-info .default-city-temp`).text(weather_data[key]["main"]["temp"] + " °C");
            $(`#${key} .weather-display-default-city-weather-info .default-city-cloud`).text(weather_data[key]["clouds"]["all"] + "%");
            $(`#${key} .weather-display-default-city-weather-info .default-city-wind-speed`).text(weather_data[key]["wind"]["speed"] + " at  " + weather_data[key]["wind"]["deg"] + " degrees");
        }
    }).fail(function(jqXHR, textStatus) {
        alert("Error: Unable to Process the Request");
    });
}


$(document).ready(function() {
    console.log("Ready!");
    getExampleWeatherData();
});