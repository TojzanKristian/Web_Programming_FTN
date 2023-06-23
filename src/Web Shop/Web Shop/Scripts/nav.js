function changePrikazHref(newHref) {
    localStorage.setItem("prikazHref", newHref);
    var prikazLink = document.getElementById("prikazLink");
    prikazLink.setAttribute("href", newHref);
}

window.addEventListener("load", function () {
    var prikazLink = document.getElementById("prikazLink");
    var savedHref = localStorage.getItem("prikazHref");
    if (savedHref) {
        prikazLink.setAttribute("href", savedHref);
    }
});

function changePrikazHrefOdjava() {
    localStorage.setItem("prikazHref", "");
    var prikazLink = document.getElementById("prikazLink");
    prikazLink.setAttribute("href", "");
}