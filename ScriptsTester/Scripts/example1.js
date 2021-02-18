function custom(input) {
    debugger;
    var returnObjs = [];

    for (var i = 0; i < input.Length; i++) {
        var newObj = { "name": input[i] };
        returnObjs.push(newObj);
    }


    return JSON.stringify(returnObjs);
}