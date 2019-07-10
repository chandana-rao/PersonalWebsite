// This is a javascript file that initializes the menu bar at the top

function init() {
    let topmenu = document.getElementsByTagName("menubar");

    topmenu[0].innerHTML =
        `<div class='menu'>
        <a href='../'>Home</a>
    </div>
    <div style='clear:all;'></div>`

    let foot1 = document.getElementsByTagName("foot");
    foot1[0].innerHTML = "<p>Chandana Rao 2019</p>"
}

