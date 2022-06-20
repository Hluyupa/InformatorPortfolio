$(document).ready(function () {
    var flag = true;
    $('.callMenu, .navOverlay').click(function () {
        $('.callMenu').toggleClass('callMenuActive');
        if (flag) {
            $('.nav').css({ 'left': '0px' });
            $('.navOverlay').css({ 'left': '0%' });
            flag = false;

        }
        else {
            flag = true;
            $('.nav').css({ 'left': '-200px' })
            $('.navOverlay').css({ 'left': '-100%' });
        }
       
    });

    /*$('.callMenu .callMenuActive, .navOverlay').click(function () {
       
    });*/
});