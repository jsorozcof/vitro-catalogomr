$(document).ready(function () {
    var window = $(document).height();
    var navview = $(".navview").height();

    if (window > navview) {
        $(".navview").height(window);
        $("#footer").hide();
    }
    console.log(window);
    console.log(navview);
});