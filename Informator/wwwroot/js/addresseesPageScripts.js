function deleteFunc(group, row) {

    row.remove();
    $.ajax({
        type: "POST",
        url: "/Home/DeleteAddressee",
        data: group,
        success: function (responce) {
            alert("Харош")
        },
        faulure: function (responce) {
            alert(responce.responseText);
        },
        error: function (responce) {
            alert(responce.responseText);
        }
    });
}

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Home/GetContacts",

        success: function (responce) {
            var contacts = JSON.parse(responce);
            contacts.forEach(function (contact) {
                $('.addresseesTable').find('tbody')
                    .append($('<tr>')
                        .append($('<td>').text(contact.FirstName))
                        .append($('<td>')
                            .append($('<div class="udPanel">')
                                .append($('<button class="updateButton">Редактировать</button>')
                                    .on('click', function () {
                                        $('.shadowModalWindow, .updateGroupWindow').addClass('active');
                                        updateFunc(contact);
                                    }))
                                .append($('<button class="deleteButton">Удалить</button>')
                                    .on('click', function (e) {
                                        deleteFunc(contact, $(e.target).closest("tr"));
                                    })))));                    
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
});