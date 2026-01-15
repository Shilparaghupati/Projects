const months = ["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];

function populate() {
    const dobMonth = document.getElementById("dobMonth");
    const toMonth = document.getElementById("toMonth");
    const dobDay = document.getElementById("dobDay");
    const toDay = document.getElementById("toDay");
    const dobYear = document.getElementById("dobYear");
    const toYear = document.getElementById("toYear");

    months.forEach((m, i) => {
        dobMonth.add(new Option(m, i));
        toMonth.add(new Option(m, i));
    });

    for (let d = 1; d <= 31; d++) {
        dobDay.add(new Option(d, d));
        toDay.add(new Option(d, d));
    }

    const year = new Date().getFullYear();
    for (let y = 1900; y <= year + 5; y++) {
        dobYear.add(new Option(y, y));
        toYear.add(new Option(y, y));
    }

    toYear.value = year;
    toMonth.value = new Date().getMonth();
    toDay.value = new Date().getDate();
}

populate();

function calculateAge() {
    const dob = new Date(
        dobYear.value,
        dobMonth.value,
        dobDay.value
    );

    const toDate = new Date(
        toYear.value,
        toMonth.value,
        toDay.value
    );

    if (dob > toDate) {
        document.getElementById("result").innerText =
            "Date of Birth cannot be after target date.";
        return;
    }

    let diff = toDate - dob;

    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const weeks = Math.floor(days / 7);

    let years = toDate.getFullYear() - dob.getFullYear();
    let monthsDiff = toDate.getMonth() - dob.getMonth();

    if (monthsDiff < 0) {
        years--;
        monthsDiff += 12;
    }

    document.getElementById("result").innerHTML = `
        Age is:<br><br>
        ${years} Years<br>
        ${monthsDiff} Months<br>
        ${weeks} Weeks<br>
    
       
    `;
}
