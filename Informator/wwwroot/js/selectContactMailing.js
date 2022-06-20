$(document).ready(function () {
    var contactsMailing = [];
    var groupsMailing = [];

    /*Получение контактов*/
    $.ajax({
        type: "GET",
        url: "/Home/GetContacts",

        success: function (responce) {
            var result = JSON.parse(responce);
            result.forEach(function (item) {
                $('.contactTable').find('tbody')
                    .append($('<tr>')
                        .click(function () {
                            if (contactsMailing.includes(item)) {
                                $(this).css({ 'background-color': '#FFFFFF' });
                                contactsMailing.splice(contactsMailing.indexOf(item));
                            }
                            else {
                                $(this).css({ 'background-color': '#28A737' });
                                contactsMailing.push(item);
                            }
                        })
                        .append($('<td>').text(item.FirstName)));
                        //.append($('<td>').text(item.SystemType.Name)));
            });
        },
        faulure: function (responce) {
            alert(responce.responseText);
        },
        error: function (responce) {
            alert(responce.responseText);
        }
    });

    /*Получение групп рассылки*/
    $.ajax({
        type: "GET",
        url: "/Home/GetGroups",

        success: function (responce) {
            var result = JSON.parse(responce);
            result.forEach(function (item) {
                $('.groupTable').find('tbody')
                    .append($('<tr>')
                        .click(function () {
                            if (groupsMailing.includes(item)) {
                                $(this).css({ 'background-color': '#FFFFFF' });
                                groupsMailing.splice(groupsMailing.indexOf(item));
                            }
                            else {
                                $(this).css({ 'background-color': '#28A737' });
                                groupsMailing.push(item);
                            }
                        })
                        .append($('<td>').text(item.Name)));
                        
            });
        },
        faulure: function (responce) {
            alert(responce.responseText);
        },
        error: function (responce) {
            alert(responce.responseText);
        }
    });


    /*Отправка выбранных пользователей для рассылки*/
    $('.okButton').click(function () {

        var obj = {
            TextMessage: CKEDITOR.instances.editor1.getData(),
            Contacts: contactsMailing,
            Groups: groupsMailing
        }

        $.ajax({
            type: "POST",
            url: "/Home/SendSelectedContacts",
            data: obj,

            success: function (responce) {
                alert("rytgfdgfd");
            },
            faulure: function (responce) {
                alert(responce.responseText);
            },
            error: function (responce) {
                alert(responce.responseText);
            }
        });
    });
});