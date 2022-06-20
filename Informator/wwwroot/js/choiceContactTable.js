var selectedGroupMember;
var selectedNewMember;
var updatedGroup;
//Очистка таблицы
function ClearTable(classNameTable) {
    $(classNameTable).find('tbody').empty();
}

//Создание тыблицы списка групп
function CreateGroupsTable(groups) {
    groups.forEach(function (group) {
        $('.groupsTable').find('tbody')
            .append($('<tr>')
                .append($('<td>').text(group.Name))
                .append($('<td>').text(group.Members.length))
                .append($('<td>')
                    .append($('<div class="udPanel">')
                        .append($('<button class="updateButton">Редактировать</button>')
                            .on('click', function () {
                                $('.shadowModalWindow, .updateGroupWindow').addClass('active');
                               
                                updateFunc(group);
                            }))
                        .append($('<button class="deleteButton">Удалить</button>')
                            .on('click', function (e) {
                                deleteFunc(group, $(e.target).closest("tr"));
                            })))));

    });
}

//Перестройка таблиц в режиме редактирования
function rebuildTables(object) {
    ClearTable('.contactsTableUpdatedGroup');
    ClearTable('.contactsTableOutGroup');
    object.UsersIntoGroup.forEach(function (member) {
        $('.contactsTableUpdatedGroup').find('tbody')
            .append($('<tr>')
                .on('click', function () {

                    if ($(this).hasClass('selected')) {
                        $(this).removeClass('selected');
                    }
                    else {
                        $('.contactsTableUpdatedGroup').find('tr').removeClass('selected');
                        $(this).addClass('selected');
                       
                        selectedGroupMember = member
                    }
                })
                .append($('<td>').text(member.FirstName)));


    });

    object.UsersOutGroup.forEach(function (member) {
        $('.contactsTableOutGroup').find('tbody')
            .append($('<tr>')
                .on('click', function () {

                    if ($(this).hasClass('selected')) {
                        $(this).removeClass('selected');
                    }
                    else {
                        $('.contactsTableOutGroup').find('tr').removeClass('selected');
                        $(this).addClass('selected');
                        
                        selectedNewMember = member;
                    }
                })
                .append($('<td>').text(member.FirstName)));

    });
}

//Функция редатирования группы
function updateFunc(group) {    
    $.ajax({
        type: "POST",
        url: "/Home/GetUpdatedGroup",
        data: group,
        success: function (responce) {
            updatedGroup = JSON.parse(responce);
            
            $('.nameUpdatedGroupInput').val(updatedGroup.Name);
            rebuildTables(updatedGroup);
        },
        faulure: function (responce) {
            alert("Ошибка");
        },
        error: function (responce) {
            alert("Ошибка");
        }
    });

    

    $('.shadowModalWindow, .cnacelButton').on('click', function () {
        $('.shadowModalWindow, .updateGroupWindow').removeClass('active');
        selectedGroupMember = null;
        selectedNewMember = null;
        $('.addButton').off('click');
        $('.removeButton').off('click');
        ClearTable('.contactsTableUpdatedGroup');
        ClearTable('.contactsTableOutGroup');
    });

    $('.updateGroupWindow').find('.okButton').one('click', function () {
        updatedGroup.Name = $('.nameUpdatedGroupInput').val();
        $('.shadowModalWindow, .updateGroupWindow').removeClass('active');
        $.ajax({
            type: "POST",
            url: "/Home/UpdateGroup",
            data: updatedGroup,

            beforeSend: function () {
                //alert("Старт");
                //$('.messageBox').addClass('active');
                
            },
            success: function (responce) {
                alert('Отлично');
                ClearTable('.groupsTable');
                CreateGroupsTable(JSON.parse(responce));
            },
            complete: function () {
                //alert("Финиш");
                //$('.shadowModalWindow, .messageBox').removeClass('active');
            },
            faulure: function (responce) {
                alert(responce.responseText);
            },
            error: function (responce) {
                alert(responce.responseText);
            }
        });
    });
}



//Функция удаления группы
function deleteFunc(group, row) {
    
    row.remove();
    $.ajax({
        type: "POST",
        url: "/Home/DeleteGroup",
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
    var newGroup = [];

    $('.createGroupButton').click(function () {
        $('.shadowModalWindow, .createGroupWindow').addClass('active');
    });


    $('.addButton').on('click', function () {
        if (selectedNewMember == null) {
            alert("Выберите адресат, который хотите добавить группу");
            return;
        }
        if (selectedNewMember != null) {
            updatedGroup.UsersIntoGroup.push(selectedNewMember);
            updatedGroup.UsersOutGroup.splice(updatedGroup.UsersOutGroup.indexOf(selectedNewMember), 1);
            rebuildTables(updatedGroup);
            selectedNewMember = null;
            return;
        }

    });


    $('.removeButton').on('click', function () {
        if (selectedGroupMember == null) {
            alert("Выберите адресат, который хотите удалить из группы");
            return;
        }
        if (selectedGroupMember != null) {
            updatedGroup.UsersOutGroup.push(selectedGroupMember);
            updatedGroup.UsersIntoGroup.splice(updatedGroup.UsersIntoGroup.indexOf(selectedGroupMember), 1);
            rebuildTables(updatedGroup);
            selectedGroupMember = null;
            return;
        }

    });


    //Получение списка групп для crud
    $.ajax({
        type: "GET",
        url: "/Home/GetGroups",

        success: function (responce) {
            var groups = JSON.parse(responce);
            CreateGroupsTable(groups);
        },
        faulure: function (responce) {
            alert(responce.responseText);
        },
        error: function (responce) {
            alert(responce.responseText);
        }
    });

    //Получение контактов
    $.ajax({
        type: "GET",
        url: "/Home/GetContacts",

        success: function (responce) {
            contacts = JSON.parse(responce);
            contacts.forEach(function (contact) {
                $('.contactTable').find('tbody')
                    .append($('<tr>')
                        .click(function () {
                            if (newGroup.includes(contact)) {
                                $(this).css({ 'background-color': '#FFFFFF' });
                                newGroup.splice(newGroup.indexOf(contact));
                            }
                            else {
                                $(this).css({ 'background-color': '#28A737' });
                                newGroup.push(contact);
                            }
                        })
                        .append($('<td>').text(contact.FirstName)));
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

    //Создание новой группы
    $('.createGroupWindow').find('.okButton').click(function () {

        var obj = {
            Name: $('.textAreaGroupName').val().toString(),
            Members: newGroup
        }

        

        $.ajax({
            type: "POST",
            url: "/Home/CreateNewMailingList",
            data: obj,
           
            success: function (responce) {
                alert("Харош");
            },
            faulure: function (responce) {
                alert("asd");
            },
            error: function (responce) {
                alert("fdsd");
            }
        });
    });

    
});