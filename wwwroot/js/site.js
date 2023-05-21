// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Button to show/hide character bar
function toggleStats() {
    const stats = document.getElementById("char-stats-partial");
    if (stats.style.display === "block") {
        stats.style.display = "none";
    } else {
        stats.style.display = "block";
    }
}

// Handle Register stat points
const maxTotalPoints = 50;
const stat1Input = document.getElementById('stat1Points');
const stat2Input = document.getElementById('stat2Points');
const stat3Input = document.getElementById('stat3Points');
const stat4Input = document.getElementById('stat3Points');
const stat5Input = document.getElementById('stat3Points');
const totalPointsSpan = document.getElementById('totalPoints');

stat1Input.addEventListener('input', updateTotalPoints);
stat2Input.addEventListener('input', updateTotalPoints);
stat3Input.addEventListener('input', updateTotalPoints);
stat4Input.addEventListener('input', updateTotalPoints);
stat5Input.addEventListener('input', updateTotalPoints);

function updateTotalPoints() {
    const stat1Value = parseInt(stat1Input.value) || 0;
    const stat2Value = parseInt(stat2Input.value) || 0;
    const stat3Value = parseInt(stat3Input.value) || 0;

    let totalPoints = maxTotalPoints - (stat1Value + stat2Value + stat3Value);

    totalPoints = Math.max(0, Math.min(maxTotalPoints, totalPoints));

    totalPointsSpan.innerText = totalPoints;

    const remainingPoints = maxTotalPoints - totalPoints;
    stat1Input.max = remainingPoints + parseInt(stat1Input.value);
    stat2Input.max = remainingPoints + parseInt(stat2Input.value);
    stat3Input.max = remainingPoints + parseInt(stat3Input.value);
}