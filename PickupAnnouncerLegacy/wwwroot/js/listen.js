"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/pickup").build();

connection.on("PickupAnnouncement", function (pickupAnnouncementJson) {
    var pickupAnnouncement = JSON.parse(pickupAnnouncementJson);
    if (pickupAnnouncement.students && pickupAnnouncement.students.length > 0) {
        addAnnouncement(pickupAnnouncement);
    }
    else {
        studentDetails = "No Students Found"
    }
});

function getStyle(student) {
    var style = "";
    if (student.backgroundColor) {
        style += ` background-color: ${student.backgroundColor}; `
    }
    if (student.textColor) {
        style += ` color: ${student.textColor}; `
    }
    return style;
}

function addAnnouncement(pickupAnnouncement) {
    var div = document.createElement("div");
    div.className = 'card shadow mt-2';
    var announcementCard = `
        <div class="card-body row">
            <div class="col-4 col-sm-2 p-2">
                <div class="h-100">
                    <div class="col-12 h-100">
                        <div class="row h-100 align-items-center" style="background-image: url(images/cone-icon.png); background-position: center center; background-size: auto 100px; background-repeat: no-repeat">
                            <div class="pl-2 pr-2 text-center mx-auto" style="background-color: rgb(255, 255, 255, 0.5)">
                                <h2>${pickupAnnouncement.cone}</h2>
                                <h2><span><i class="fa fa-car" aria-hidden="true"></i></span>
                                ${pickupAnnouncement.car}</h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-8 col-sm-10 ">`;
    pickupAnnouncement.students.forEach(student => {
        announcementCard += `
            <div class="row">
                <div class="col-9">
                    <div class="row border border-light rounded" style="${getStyle(student)}">
                        <div class="col-12 text-center">
                            <h2 class="mt-4">${student.name}</h2>
                            <h4 class="align-text-bottom text-right">${student.teacher}</h4>
                        </div>
                    </div>  
                </div>
                <div class="col-3 d-flex p-2">
                    <div class="align-self-center mx-auto">
                        <span>Grade</span>
                        <h3 class="text-center">${student.gradeLevel}</h3>
                    </div>
                </div>
            </div>`;
    });
    pickupAnnouncement += "</div></div>";
    div.innerHTML = announcementCard;
    var messageList = document.getElementById("messagesList");
    messageList.insertBefore(div, messageList.firstChild);
}

connection.start().then(function () {
    $.post({
        url: `api/PickupLog`,
        data: JSON.stringify({ startOfDayUTC: moment().startOf('day').utc().format() }),
        contentType: 'application/json; charset=utf-8'
    })
        .done((response) => {
            if (response && response.announcements) {
                response.announcements.forEach(announcement => addAnnouncement(announcement));
            }
        })
        .fail((jqXHR, settings, ex) => {
            toastr.error("Failed to lookup today pickup log.", "API Error")
        });
}).catch(function (err) {
    return console.error(err.toString());
});