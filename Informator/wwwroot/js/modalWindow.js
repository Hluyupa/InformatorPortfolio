$(document).ready(function () {
    $('.selectReceiver').click(function () {
        $('.shadowModalWindow, .modalWindow').addClass('active');
    });
    $('.shadowModalWindow, .cnacelButton').click(function () {
        $('.shadowModalWindow, .modalWindow').removeClass('active'); 
    });
});