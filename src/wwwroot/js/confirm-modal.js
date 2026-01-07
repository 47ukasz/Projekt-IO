const modalOpenButtons = document.querySelectorAll("[data-modal-open]");
const modalCloseButtons = document.querySelectorAll("[data-modal-close]");

modalOpenButtons.forEach(button => {
    button.addEventListener("click", () => {
        const modalId = button.dataset.modalOpen;
        const value = button.dataset.value;
        
        const modal = document.getElementById(modalId);
        const modalInput = modal.querySelector(".modal-input");
        const userInput = modal.querySelector('.modal-user');
        
        if (userInput) {
            userInput.value = button.dataset.user;
        }
        
        modalInput.value = value;
        
        modal.closest(".background").classList.add("background-show");
    })
})

modalCloseButtons.forEach(button => {
    button.addEventListener("click", () => {
        button.closest(".background").classList.remove("background-show");
    })
})
