function convertWeight() {
    let weight = document.getElementById("weight").value;
    let unit = document.getElementById("unit").value;
    let result = document.getElementById("result");

    if (weight === "") {
        result.innerHTML = "Please enter a value";
        return;
    }

    if (unit === "kg") {
        result.innerHTML =
            `${weight} kg = ${weight * 1000} g <br>
             ${weight} kg = ${(weight * 2.20462).toFixed(2)} lb`;
    }
    else if (unit === "g") {
        result.innerHTML =
            `${weight} g = ${weight / 1000} kg <br>
             ${weight} g = ${(weight / 453.592).toFixed(2)} lb`;
    }
    else {
        result.innerHTML =
            `${weight} lb = ${(weight / 2.20462).toFixed(2)} kg <br>
             ${weight} lb = ${(weight * 453.592).toFixed(2)} g`;
    }
}
