"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/pickup").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("FailureAnnouncement", function (errorMessage) {
    error.log(errorMessage);
    toast.error(errorMessage, "Announcement Failure");
});

connection.on("SuccessAnnouncement", function () {
    toastr.success('Notification Sent');
});

function addToCarInput(number) {
    var carInput = $('#carInput');
    carInput.val(carInput.val() + number);
}

function clearCarInput() {
    $('#carInput').val('');
}

function calculateRequiredCones(index, total, groupSize) {
    return Math.min(total - (index * groupSize), groupSize);
}

function createConeButtons(numberOfCones, conesPerGroup) {
    var totalCones = 0;
    var numberOfGroups = Math.ceil(numberOfCones / conesPerGroup);
    for (var group = 0; group < numberOfGroups; group++) {
        var conesForGroup = calculateRequiredCones(group, numberOfCones, conesPerGroup);
        var div = document.createElement("div");
        div.className = `btn-group btn-group-lg d-flex w-${(conesForGroup / conesPerGroup) * 100} mb-2`;
        for (var i = 0; i < conesForGroup; i++) {
            var button = document.createElement("button");
            button.className = "btn btn-outline-primary p-4 w-100 border-2";
            const coneNumber = totalCones + 1;
            button.innerHTML = coneNumber;
            div.appendChild(button);
            totalCones++;
        }
        $('#cone-container').append(div);
    }
}

createConeButtons(numberOfCones, 4);

$('#cone-container button').click(function () {
    $('#cone-container button').removeClass('active');
    $(this).addClass('active');
});

function getActiveCone() {
    const coneValue = $('#cone-container .active:first');
    if (coneValue.length > 0) {
        return coneValue[0].innerHTML;
    }
    return undefined;
}

document.getElementById("sendButton").addEventListener("click", function (event) {
    var car = document.getElementById("carInput").value;
    var cone = getActiveCone();
    if (!car && !cone) {
        toastr.error("Registration number and cone selection are required.", "Input Error");
    }
    else if (!car) {
        toastr.error("Registration number is required.", "Input Error");
    }
    else if (!cone) {
        toastr.error("Cone selection is required.","Input Error");
    }
    else {
        $.get(`api/Registration?id=${car}`, (data, textStatus, jqXHR) => {
            if (data) {
                connection.invoke("AnnouncePickup", { 'car': car, 'cone': cone })
                    .then(() => {
                        $('#cone-container button').removeClass('active');
                        $("#carInput").val('');
                    }).catch(function (err) {
                        toastr.error(err.toString());
                    });
            } else {
                toastr.error("Invalid registration number.", "Validation Error")
            }
        }).fail((jqXHR, settings, ex) => {
            toastr.error("Failed to lookup registration number.", "Validation Error")
        });
    }
    event.preventDefault();
});