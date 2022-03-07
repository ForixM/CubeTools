const wrapper = document.querySelector('.wrapper'),
            sidebar = wrapper.querySelector('nav'),
            toggle = wrapper.querySelector('.toggle');

        toggle.addEventListener("click", () => {
            sidebar.classList.toggle("close");
        })

const Chrono = document.getElementsByClassName('ChronoBox');

for (i = 0; i < Chrono.length; i++){
    Chrono[i].addEventListener('click', function(){
        this.classList.toggle('active')
    })
}