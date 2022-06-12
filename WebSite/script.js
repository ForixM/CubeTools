// Bar
const wrapper = document.querySelector('.wrapper'),
            sidebar = wrapper.querySelector('nav'),
            toggle = wrapper.querySelector('.toggle');

        toggle.addEventListener("click", () => {
            sidebar.classList.toggle("close");
        })

// Chrono
const Chrono = document.getElementsByClassName('ChronoBox');

// UI References
const multiple_download = document.getElementById('multiple_download');
const multiple_documentation = document.getElementById('multiple_documentation');
const download_button = document.getElementById('download_button');
const documentation_button = document.getElementById('documentation_button');

// Variables
_download_button_clicked = false;
_documentation_button_clicked = false;

// Adding listener

download_button.onclick = function() {
   if (_download_button_clicked) {
        multiple_download.style.visibility = "hidden";
   }
   else {
        multiple_download.style.visibility = "visible";
   }
   _download_button_clicked = !_download_button_clicked;
};

documentation_button.onclick = function() {
    if (_documentation_button_clicked) {
        multiple_documentation.style.visibility = "hidden";
   }
   else {
        multiple_documentation.style.visibility = "visible";
   }
   _documentation_button_clicked = !_documentation_button_clicked;
};

console.log(multiple_documentation);
console.log(multiple_download);

for (i = 0; i < Chrono.length; i++){
    Chrono[i].addEventListener('click', function(){
        this.classList.toggle('active')
    })
}