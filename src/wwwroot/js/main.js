const menuBtn = document.querySelector(".nav-menu");
const navMobile = document.querySelector(".nav-mobile");
const backBtn = document.querySelector(".nav-back-btn");

menuBtn.addEventListener("click", () => {
    navMobile.classList.add("nav-mobile-show");
});

backBtn.addEventListener("click", () => {
    navMobile.classList.remove("nav-mobile-show");
});
