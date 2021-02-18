function custom(input) {
    debugger;
    var query = "";

    Console.WriteLine("Querying database from inside js");
    var result = db.Query("Select * from cars");

    Console.WriteLine("Reporting values of query result from inside js:");
    for (var i = 0; i < result.Length; i++) {
        Console.WriteLine(`Values of first car: ID:${result[i].Id} Brand:${result[i].Brand} DateManufactured:${result[i].DateManufactured.ToString()} ModuleName:${result[i].ModuleName} `);
    }

    return null;
}