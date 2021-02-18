function custom(input) {


    var genericListType = xLib.type('System.Collections.Generic.List');
    var carList = xHost.newObj(genericListType(Car));
    for (var i = 0; i < 10; i++) {

        if (i % 2 == 0) {
            var newCar = xHost.newObj(Car, "Nissan");
            carList.Add(newCar);
        }
        else {
            var newCar = xHost.newObj(Car, "Audi");
            carList.Add(newCar);
        }
    }



    return carList;
}