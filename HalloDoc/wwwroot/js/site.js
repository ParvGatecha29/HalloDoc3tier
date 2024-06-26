﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const btn1 = document.querySelector(".btn-toggle");
const prefersDarkScheme = window.matchMedia("(prefers-color-scheme: dark)");

var linkdark = document.createElement('link');
linkdark.rel = "stylesheet";
linkdark.href = "//cdn.jsdelivr.net/npm/@sweetalert2/theme-dark@4/dark.css";
var linklight = document.createElement('link');
linklight.rel = "stylesheet";
linklight.href = "//cdn.jsdelivr.net/npm/@sweetalert2/theme-minimal/minimal.css";

const currentTheme = localStorage.getItem("theme");
if (currentTheme != null) {
    document.documentElement.setAttribute("data-bs-theme", currentTheme);
}
else {
    document.documentElement.setAttribute("data-bs-theme", "light");
    localStorage.setItem("theme", "light");
}

if (currentTheme == "dark") {
    document.body.classList.toggle("dark-theme");
    document.head.appendChild(linkdark);
} else if (currentTheme == "light") {
    document.body.classList.toggle("light-theme");
    document.head.appendChild(linklight);
}
else {
    document.body.classList.toggle("light-theme");
    document.head.appendChild(linklight);
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
    if (theme == "light") {
        document.head.appendChild(linklight);
        document.head.removeChild(linkdark);
    }
    else {
        document.head.removeChild(linklight);
        document.head.appendChild(linkdark);
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


function upload(file) {
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
    if (window.innerWidth <= 600) {
        var details = document.querySelectorAll('.mobile-hide');
        details.forEach(function (detail) {
            detail.style.display = detail.style.display === 'none' ? 'block' : 'none';
        });
    }
    
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
    for (var i = 0; i < phoneInputField.length; i++) {
        const phoneInput = window.intlTelInput(phoneInputField[i], {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
            nationalMode: false,
            showSelectedDialCode: true,
            autoInsertDialCode: true,
            formatOnDisplay: false,
        })
    };


    //$(document).ajaxSend(function () {
    //    $("#spinner-div").fadeIn(250);
    //});
    //$(document).ajaxComplete(function () {
    //    $("#spinner-div").fadeOut(250);
    //});
});


// Function to request notification permission
function askNotificationPermission() {
    console.log("hello");
    if ('Notification' in window && navigator.serviceWorker) {
        Notification.requestPermission().then(status => {
            console.log('Notification permission status:', status);
            if (status === 'granted') {
                initializeServiceWorker();
            } else {
                console.warn('Notification permission not granted');
            }
        });
    }
}

// Function to initialize the service worker
function initializeServiceWorker() {
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/sw.js')
            .then(function (registration) {
                console.log('Service Worker registered with scope:', registration.scope);
            })
            .catch(function (error) {
                console.error('Service Worker registration failed:', error);
            });
    }
}

// Function to display notification
function displayNotification(user, message) {

    if (Notification.permission === 'granted') {

        navigator.serviceWorker.getRegistration().then(function (reg) {
            console.log(navigator);
            if (reg) {
                console.log("hiii");

                const options = {
                    body: `${user}: ${message}`,
                    // icon: 'images/icon.png',
                    // vibrate: [100, 50, 100],
                    data: {
                        dateOfArrival: Date.now(),
                        primaryKey: 1
                    }
                };
                reg.showNotification('New Message', options);
            }
        });
    } else {
        console.warn('Notification permission not granted');
    }
}

// Call the function to ask for notification permission when the page loads

