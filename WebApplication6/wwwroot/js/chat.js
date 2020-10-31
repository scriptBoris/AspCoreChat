$("#sendForm").on("submit", function (e) {
    e.preventDefault();
    sendMessage();
});


// Идет в данный момент отправка сообщения
var isSend = false;

// Актуальный ID для AJAX обновления данных
var stateId = Number.parseInt($("#stateId").val());

// GUID текущего пользователя
var userGuid = $("#userGuid").val();

function sendMessage() {
    // Отключаем возможность отправлять сообщения в момент 
    // отправки на сервер
    if (isSend)
        return;

    isSend = true;

    var message = $("#sendText").val();
    var json = JSON.stringify({ Message: message, UserGuid: userGuid });

    //alert("try send: " + message + " " + userGuid + " " + json);

    if (message == null || message == "")
        return;

    $.ajax({
        type: 'POST',
        url: 'http://localhost:5000/Api/SendMessage',
        data: json,
        success: sendMessageSuccess,
        error: sendMessageError,
        contentType: "application/json"
    });
}
function sendMessageSuccess(data) {
    isSend = false;
    //alert("Success");

    if (data.code != 0 || data.result == null) {
        alert("Произошла ошибка при отправке сообщения, повторите позже");
        return;
    }

    var date = data.result.date;
    var author = data.result.author;
    var text = data.result.text;
    var dateFix = parseDate(date);

    $("#sendText").val("");

    addMessage(dateFix, author, text);
    $("#messageBox").animate({ scrollTop: $('#messageBox').prop("scrollHeight") }, 1000);
}
function sendMessageError() {
    isSend = false;
    alert("Произошла ошибка при отправке сообщения, повторите позже");
}


function addMessage(date, author, text) {
    var align = "justify-content-start";
    if (author.guid == userGuid)
        align = "justify-content-end";

    $("#messageBox").append($(
        '<div class="d-flex '+ align +'">' + 
            '<div class="msg_containerLeft mb-2 p-1">' +
                '<table>' +
                    '<tr>'+
                        '<td class="chattop">' +
                            '<strong class="chatname">' + author.name + '</strong>' +
                            '<span class="chatdate">&nbsp;&nbsp;' + date +'</span>' +
                        '</td>' +
                    '</tr>' +
                    '<tr>' +
                        '<td class="chattext">' +
                            text +
                        '</td>' +
                    '</tr>' +
                '</table >' +
            '</div>' +
        '</div >'
    ));
}

function addUser(name, subname, id) {
    var html = $(
        '<div id="' + id + '" class="d-flex mb-3 mx-3 p-2 bg-light">' +
            '<span class="fas fa-user-circle chatuserimage"></span>' +
            '<table class="msg_cotainer ml-2">'+
                '<tr>' +
                    '<td class="chattop">' +
                        '<strong id="userNameId'+ id +'" class="chatname">' + name + '</strong>' +
                    '</td>' +
                '</tr>'+
                '<tr>' +
                    '<td id="userStatusId' + id + '" class="chattext">' +
                        subname +
                    '</td>' +
                '</tr>' +
            '</table>' +
        '</div>'
    );

    $("#usersBox").append(html);
}

function changeUser(name, subname, id) {

    $("#userNameId" + id).text(name);
    $("#userStatusId" + id).text(subname);
    //addUser(name, subname, id, id);
    //$("#usersBox").remove(id);
}


function parseDate(stringDate) {
    var hour = stringDate.substring(11, 13);
    var min = stringDate.substring(14, 16);
    //var sec = stringDate.substring(17, 19);
    return hour + ":" + min /*+ ":" + sec*/;
}



async function getUpdate() {
    var json = JSON.stringify({ UserGuid: userGuid, StateId:stateId });
    await $.ajax({
        type: 'POST',
        url: 'http://localhost:5000/Api/GetUpdates',
        data: json,
        success: loopSuccess,
        error: loopError,
        timeout: 60000,
        contentType: "application/json"
    });
}

//Клиентская реализация лонг пулинга
function loopSuccess(data) {
    if (data.code == 0) {
        stateId = data.result.newStateId;

        //Новые сообщения
        if (data.result.newMessages != null) {

            for (var i = 0; i < data.result.newMessages.length; i++) {
                var msg = data.result.newMessages[i];
                var date = parseDate(msg.date);
                var author = msg.author;
                var text = msg.text;
                addMessage(date, author, text)
            }

            // Авто прокрутка сообщений
            if (data.result.newMessages.length > 0) {
                var scrollY = $("#messageBox").scrollTop();
                var scrollHeight = $("#messageBox").height();

                if (scrollY >= scrollHeight * 0.9)
                    $("#messageBox").animate({ scrollTop: $('#messageBox').prop("scrollHeight") }, 1000);
            }
        }

        // Новые пользователи
        if (data.result.newUsers != null)
            for (var i = 0; i < data.result.newUsers.length; i++) {
                var user = data.result.newUsers[i];
                var name = user.name;
                var status = "В сети";
                addUser(name, status, user.id);
            }

        // Удаленные пользователи
        if (data.result.removeUsers != null) {
            for (var i = 0; i < data.result.removeUsers.length; i++) {
                var user = data.result.removeUsers[i];
                var name = user.name;
                $("#usersBox").remove(user.id);
            }
        }

        // Измененные пользователи
        if (data.result.editUsers != null) {
            for (var i = 0; i < data.result.editUsers.length; i++) {
                var user = data.result.editUsers[i];
                var name = user.name;
                var status = user.status;

                if (status == 0)
                    status = "В сети";
                else
                    status = "Не в сети";

                changeUser(name, status, user.id);
            }
        }
    }
    getUpdate();
}

function loopError(jqXHR, textStatus, errorThrown) {
    getUpdate();
}

getUpdate();

// Прокручиваем в дно списка сообщений
var msgbox = $("#messageBox");
msgbox.scrollTop(msgbox.prop("scrollHeight"));