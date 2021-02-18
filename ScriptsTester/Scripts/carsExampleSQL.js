function custom(input) {
    var query = "";
    for (var i = 0; i < input.Length; i++) {
        input[i].Brand = "ModifiedBrand" + (100 + i);
        input[i].ModelName = "ModifiedModelName" + (100 + i);
        input[i].DateManufactured = input[i].DateManufactured.AddDays(i + 1);
    }
    Console.WriteLine("Attempting to insert a records in db from inside the script");
    for (var i = 0; i < input.Length; i++) {
        

        query = `INSERT INTO CARS(Brand,ModuleName,DateManufactured) Values('${input[i].Brand}',
        '${input[i].ModelName}','${input[i].DateManufactured.ToString("MM/dd/yyyy HH:mm:ss")}')`;
        var affectedRows = db.Execute(query);
        Console.WriteLine(`Rows affected: ${affectedRows}`);
    }
    Console.WriteLine("Reporting new values from inside the script");
    for (var i = 0; i < input.Length; i++) {
        Console.WriteLine(input[i].ToString());
    }

    return null;
}