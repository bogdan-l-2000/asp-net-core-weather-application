

function getExampleWeatherData() {
    $.ajax({
        type: 'GET',
        url: "/?handler=Defaults",
        dataType: "json",
        async: true,
        headers: {
            "Access-Control-Allow-Origin": "*"
        },
        // data: {

        // },
        success: function(data) {
            console.log(data);
        },
        error: function(){
            alert("Error: Unable to Process the Request");
        }
    })
    .done(function(result) {
        console.log("Completed!");
    });
}


$(document).ready(function() {
    console.log("Ready!");
    getExampleWeatherData();
});