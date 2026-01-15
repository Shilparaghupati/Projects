function calculateBMI() {
    let height = document.getElementById("height").value;
    let weight = document.getElementById("weight").value;

    if (height === "" || weight === "") {
        alert("Please enter height and weight");
        return;
    }

    height = height / 100; // cm → meters
    let bmi = (weight / (height * height)).toFixed(2);

    document.getElementById("result").innerText =
        "Your BMI is: " + bmi;

    let category = "";

    if (bmi < 18.5) {
        category = "Underweight";
    } else if (bmi < 25) {
        category = "Normal weight";
    } else if (bmi < 30) {
        category = "Overweight";
    } else {
        category = "Obese";
    }

    document.getElementById("category").innerText =
        "Category: " + category;
}
