const modalOpenButtons = document.querySelectorAll("[data-modal-open]");
const modalCloseButtons = document.querySelectorAll("[data-modal-close]");

modalOpenButtons.forEach(button => {
    button.addEventListener("click", () => {
        const modalId = button.dataset.modalOpen;
        const value = button.dataset.value;
        
        const modal = document.getElementById(modalId);
        const modalInput = modal.querySelector(".modal-input");
        
        modalInput.value = value;
        
        console.log(modalInput.value)
        
        modal.closest(".background").classList.add("background-show");
    })
})

modalCloseButtons.forEach(button => {
    button.addEventListener("click", () => {
        button.closest(".background").classList.remove("background-show");
    })
})
