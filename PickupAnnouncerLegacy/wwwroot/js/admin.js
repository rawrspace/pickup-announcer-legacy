$(document).ready(function () {
    $("#registrationFile").fileinput({
        showPreview: false,
        allowedFileExtensions: ["csv"],
        elErrorContainer: "#errorBlock"
    });

    $('#updateSettings').click(function (e) {
        e.preventDefault();
        var numberOfCones = parseInt($("#numberOfCones").text());
        if (!Number.isInteger(numberOfCones)) {
            toastr.error(`Number of Cones only support whole numbers. Detected: ${numberOfCones}`, "Validation Error");
        } else {
            $.post({
                url: `api/Site`,
                data: JSON.stringify({ numberOfCones: numberOfCones }),
                contentType: 'application/json; charset=utf-8'
            })
                .done(() => {
                    toastr.success("Settings have been updated.")
                })
                .fail((jqXHR, settings, ex) => {
                    toastr.error("Failed to update settings, please refresh and try again.", "API Error")
                });
        }
    });

    $.get(`api/GradeLevel`, (data) => {
        if (data && data.length > 0) {
            data.forEach(gradeLevel => {
                addGradeLevelRow(gradeLevel.id, gradeLevel.name, gradeLevel.backgroundColor, gradeLevel.textColor);
            });
        } else {
            toastr.warning("No grade level entries.", "Data Error")
        }
    }).fail((jqXHR, settings, ex) => {
        toastr.error("Failed to locate any grade level entries.", "API Error")
    });


    $('#addGradeLevel').click(function (e) {
        e.preventDefault();
        addGradeLevelRow(undefined, "00", "#FFF", "#000");
    });
});

var gradeLevelCount = 0;
function addGradeLevelRow(id, name, backgroundColor, textColor) {
    const index = gradeLevelCount;
    //Create Row
    var row = document.createElement("tr");
    //Create Columns
    var gradeLevelCol = document.createElement("td");
    var gradeLevelName = document.createElement("div");
    $(gradeLevelName).click(eventHandler);
    gradeLevelName.innerText = name;
    gradeLevelCol.appendChild(gradeLevelName);
    row.appendChild(gradeLevelCol);

    var bgColorCol = document.createElement("td");
    var bgColorInput = document.createElement("input");
    bgColorInput.value = backgroundColor;
    bgColorCol.appendChild(bgColorInput);
    row.appendChild(bgColorCol);

    var textColorCol = document.createElement("td");
    var textColorInput = document.createElement("input");
    textColorInput.value = textColor;
    textColorCol.appendChild(textColorInput);
    row.appendChild(textColorCol);

    var previewCol = document.createElement("td");
    previewCol.innerHTML = `
                    <div class="row border border-light rounded grade-level-row-preview-${index}" style=" background-color: ${backgroundColor};  color: ${textColor}; ">
                        <div class="col-12 text-center">
                            <h2 class="mt-4">Student Name</h2>
                            <h4 class="align-text-bottom text-right">Teacher</h4>
                        </div>
                    </div>
                `
    row.appendChild(previewCol);

    var buttonCol = document.createElement("td");

    var saveButton = document.createElement("button");
    saveButton.classList = "btn btn-primary mr-2";
    saveButton.innerHTML = "<i class='fas fa-save' aria-hidden='true'></i>"
    $(saveButton).click(function (e) {
        e.preventDefault();
        saveToDatabase(id, gradeLevelName.innerText, bgColorInput.value, textColorInput.value);
    });
    buttonCol.appendChild(saveButton);

    var deleteButton = document.createElement("button");
    deleteButton.classList = "btn btn-primary mr-2";
    deleteButton.innerHTML = "<i class='fas fa-trash-alt' aria-hidden='true'></i>"
    $(deleteButton).click(function (e) {
        e.preventDefault();
        deleteFromDatabase(id);
        row.remove();
    });
    buttonCol.appendChild(deleteButton);

    row.appendChild(buttonCol);

    //Add to DOM
    $('#gradeLevelTable').append(row);

    //Activate Color Picker
    var bgHueb = new Huebee(bgColorInput, {
        // options
        setBGColor: true,
        notation: 'hex'
    });
    var textHueb = new Huebee(textColorInput, {
        // options
        setBGColor: true,
        notation: 'hex'
    });
    //Setup Actions
    bgHueb.on('change', function (color) { $(`.grade-level-row-preview-${index}`).css('background-color', color); });
    textHueb.on('change', function (color) { $(`.grade-level-row-preview-${index}`).css('color', color); });

    gradeLevelCount++;
}

function saveToDatabase(id, gradeLevel, backgroundColor, textColor) {
    $.post({
        url: `api/GradeLevel`,
        data: JSON.stringify({ id: id, name: gradeLevel, backgroundColor: backgroundColor, textColor: textColor }),
        contentType: 'application/json; charset=utf-8'
    })
        .done((response) => {
            if (response.success) {
                toastr.success(response.message);
            } else {
                toastr.error(response.message);
            }
        })
        .fail((jqXHR, settings, ex) => {
            toastr.error("Failed to save to database.", "API Error")
        });
}

function deleteFromDatabase(id) {
    if (id) {
        $.ajax({
            url: `api/GradeLevel?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error("Failed to delete from database.", "API Error")
            }
        });
    }
}


var eventHandler = function (e) { e.preventDefault(); editDiv(this); };
document.querySelector('.editable').addEventListener("click", eventHandler);

function editDiv(div) {
    var text = div.innerText,
        input = document.createElement("input");
    input.value = text;

    div.innerHTML = "";
    div.append(input);
    input.focus();
    input.addEventListener("focusout", function (e) {
        finishEditDiv(div);
    });

    div.removeEventListener("click", eventHandler);

}

function finishEditDiv(div) {
    //handle your data saving here
    var text = div.querySelector('input').value;
    div.innerHTML = text;
    document.querySelector('.editable').addEventListener("click", eventHandler);
}