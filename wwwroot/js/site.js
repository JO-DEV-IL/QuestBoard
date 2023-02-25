// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleStats() {
    const stats = document.getElementById("char-stats-partial");
    if (stats.style.display === "none") {
        stats.style.display = "block";
    } else {
        stats.style.display = "none";
    }
}