// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const btn1 = document.querySelector(".btn-toggle");
const prefersDarkScheme = window.matchMedia("(prefers-color-scheme: dark)");

const currentTheme = localStorage.getItem("theme");
document.documentElement.setAttribute("data-bs-theme", currentTheme);
if (currentTheme == "dark") {
    document.body.classList.toggle("dark-theme");
} else if (currentTheme == "light") {
    document.body.classList.toggle("light-theme");
}

btn1.addEventListener("click", function () {
    if (prefersDarkScheme.matches) {
        document.body.classList.toggle("light-theme");
        var theme = document.body.classList.contains("light-theme")
            ? "light"
            : "dark";
    } else {
        document.body.classList.toggle("dark-theme");
        var theme = document.body.classList.contains("dark-theme")
            ? "dark"
            : "light";
    }
    localStorage.setItem("theme", theme);
    document.documentElement.setAttribute("data-bs-theme", theme);
});

var date = new Date();

var day = date.getDate();
var month = date.getMonth() + 1;
var year = date.getFullYear();

if (month < 10) month = "0" + month;
if (day < 10) day = "0" + day;

var today = year + "-" + month + "-" + day;
var element = document.getElementById("dob");
if (element != null) {
    document.getElementById("dob").value = today;

    $('#dob').attr('max', today);
}



function upload(file){
    var name = file.files[0].name;
    var span = document.getElementById("upload");
    span.innerHTML = name;
} 

function pass() {
    var x = document.getElementById("pass");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }

    var e = document.querySelector("#eye");
    e.classList.toggle("bi-eye-slash");
    e.classList.toggle("bi-eye");
}

document.addEventListener('DOMContentLoaded', function () {
    var tableRows = document.querySelectorAll('#responsiveTable tbody .table-row');

    function toggleDetails() {
        if (window.innerWidth <= 600) {
            var details = this.querySelectorAll('.mobile-hide');
            details.forEach(function (detail) {
                detail.style.display = detail.style.display === 'none' ? 'block' : 'none';
            });
        }
    }

    tableRows.forEach(function (row) {
        row.addEventListener('click', toggleDetails);
    });

    // Reset when resizing
    window.addEventListener('resize', function () {
        var details = document.querySelectorAll('.mobile-hide');
        if (window.innerWidth > 600) {
            // Reset display property for desktop view
            details.forEach(function (detail) {
                detail.style.removeProperty('display');
            });
        } else {
            // Apply correct display property for mobile view
            details.forEach(function (detail) {
                detail.style.display = 'none';
            });
        }
    });

    const phoneInputField = document.querySelectorAll("input[type='tel']");
    console.log(phoneInputField);
    for (var i = 0; i < phoneInputField.length; i++) {
        const phoneInput = window.intlTelInput(phoneInputField[i], {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        })
    };
});

