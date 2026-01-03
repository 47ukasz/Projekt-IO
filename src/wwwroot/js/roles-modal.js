const roleModalOpenButtons = document.querySelectorAll("[data-role-modal-open]");
const roleModalCloseButtons = document.querySelectorAll("[data-role-modal-close]");

roleModalOpenButtons.forEach(button => {
    button.addEventListener("click", () => {
        const modalId = button.dataset.roleModalOpen;
        const userId = button.dataset.value;

        const currentRoles = button.dataset.roles.split(",").map(role => role.trim());
        
        const modal = document.getElementById(modalId);
        const modalInput = modal.querySelector(".modal-input");
        const modalCheckboxes = modal.querySelectorAll(".modal-checkbox");
        
        modalInput.value = userId;
        
        modalCheckboxes.forEach((checkbox) => {
            checkbox.checked = currentRoles.includes(checkbox.value);
        })

        modal.closest(".background").classList.add("background-show");
    })
})

roleModalCloseButtons.forEach(button => {
    button.addEventListener("click", () => {
        button.closest(".background").classList.remove("background-show");
    })
})

