function custom(input) {
    Console.WriteLine("Attempting to web request from script");
    var result = web.Get("https://www.google.com/?ping=");
    Console.WriteLine(result);


    for (var i = 0; i < 5; i++) {
        var result2 = web.Get("https://official-joke-api.appspot.com/random_joke");
        var resultObj = JSON.parse(result2.Content.ReadAsStringAsync().Result);

        Console.WriteLine(resultObj.setup + "          " + resultObj.punchline);
    }

    return null;
}