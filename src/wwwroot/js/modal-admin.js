const modal = document.querySelector(".modal");
const modalTriggerButtons = document.querySelectorAll(".modal-trigger");
const modalTitleSpan = modal.querySelector(".modal-title span");
const modalForm = modal.querySelector("form");
const modalFormInput = modalForm.querySelector("input");
const cancelBtn = modalForm.querySelector("button[type='button']");

const modalBackground = modal.parentElement;

function handleOpenModal(item) {
    if (!item) {
        return;
    }

    const userId = item.dataset.id;
    const userFullName = item.querySelector("#full-name");
    const modalTitleText = userFullName.textContent;

    modalFormInput.value = userId;

    modalTitleSpan.textContent = modalTitleText;
    modalBackground.classList.add("background-show");
}

function handleCloseModal() {
    modalBackground.classList.remove("background-show");
}

modalTriggerButtons.forEach((btn) => {
    const user = btn.closest("tr");

    btn.addEventListener("click", () => handleOpenModal(user));
});

cancelBtn.addEventListener("click", handleCloseModal);
modalForm.addEventListener("submit", handleFormSubmit);
