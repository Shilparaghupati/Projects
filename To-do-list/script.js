const taskInput = document.getElementById("taskInput");
const taskList = document.getElementById("taskList");

function addTask() {
    if (taskInput.value === "") {
        alert("Please enter a task");
        return;
    }

    const li = document.createElement("li");

    const span = document.createElement("span");
    span.innerText = taskInput.value;

    const btnDiv = document.createElement("div");
    btnDiv.className = "btn-group";

    const editBtn = document.createElement("button");
    editBtn.innerText = "✏️";
    editBtn.onclick = () => editTask(span);

    const deleteBtn = document.createElement("button");
    deleteBtn.innerText = "🗑️";
    deleteBtn.onclick = () => li.remove();

    btnDiv.appendChild(editBtn);
    btnDiv.appendChild(deleteBtn);

    li.appendChild(span);
    li.appendChild(btnDiv);
    taskList.appendChild(li);

    taskInput.value = "";
}

function editTask(span) {
    const updatedText = prompt("Edit task:", span.innerText);
    if (updatedText !== null && updatedText.trim() !== "") {
        span.innerText = updatedText;
    }
}
