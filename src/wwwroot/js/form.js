const textarea = document.querySelector(".textarea-wrapper textarea");
const counter = document.querySelector(".textarea-counter");
const max = 500;
const fileInput = document.querySelector(".form-file-button input[type='file']");
const fileName = document.getElementById("file-name");

textarea.addEventListener("input", () => {
    const length = textarea.value.length;
    counter.textContent = `${length}/${max}`;

    if (length >= max) {
        counter.style.color = "#ef4444";
    } else {
        counter.style.color = "#757575";
    }
});

fileInput.addEventListener("change", () => {
    fileName.textContent = fileInput.files.length
        ? fileInput.files[0].name
        : "Nie wybrano pliku";
});
