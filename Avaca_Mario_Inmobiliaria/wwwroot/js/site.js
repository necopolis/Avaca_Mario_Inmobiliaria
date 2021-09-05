// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function editar(x) {
    //#editarModal==id="editarModal"(Modal) .modal(show) para que se vea

    $(document).ready(function () {
        $("#p1").hover(function () {
            alert("You entered p1!");
        },
            function () {
                alert("Bye! You now leave p1!");
            });
    });
    
    
}
$(document).ready(function () {
    $("#p1").hover(function () {
        alert("You entered p1!");
    },
        function () {
            alert("Bye! You now leave p1!");
        });
});

const app = new Vue({
    el: "#app",
    data: {
        titulo: "Hola mundo desde Vue - Bienvenidos"
    }
});