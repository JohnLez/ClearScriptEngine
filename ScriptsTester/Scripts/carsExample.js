function custom(input) {
    debugger;
    for (var i = 0; i < input.Length; i++) {
        input[i].Id =  100 +i;
        input[i].Brand = "ModifiedBrand" + (100 + i);
        input[i].ModelName = "ModifiedModelName" + (100 + i);
        input[i].DateManufactured = input[i].DateManufactured.AddDays(i + 1);
    }
    Console.WriteLine("Reporting new values from inside the script");
    for (var i = 0; i < input.Length; i++) {
        Console.WriteLine(input[i].ToString());
    }

    return input;
}