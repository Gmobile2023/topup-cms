$(function () {
    var _$timeNow = new Date();
    $("#date").text("Thời gian: " + moment(_$timeNow).format('DD/MM/YYYY HH:mm:ss'));
    var _$topupRequests = abp.services.app.topupRequests
    var _$table = $("Table");
    var dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        listAction: {
            ajaxFunction: _$topupRequests.getCheckChargesHistoryDetail,
            inputFilter: function () {
                return {
                    codeRequest: $("#CodeRequestFilter").text(),
                    phoneNumber: telephoneCheck($("#PhoneNumberFilter").val())
                };
            }
        },
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                className: "text-center",
                render: (data, type, row, meta) => {
                    return meta.row;
                }
            },
            {
                targets: 1,
                data: "phoneNumber",
                className: "text-center",
                render: (data) => {
                    return data;
                }
            },
            {
                targets: 2,
                className: "text-center",
                data: "debtCharges"
            },
            {
                targets: 3,
                className: "text-center",
                data: "roundingCharges"
            },
            {
                targets: 4,
                className: "text-center",
                data: "oddCharges"
            },
            {
                targets: 5,
                className: "text-center",
                data: "status",
                render: (data) => {
                    return data == 0 ? app.localize("Completed") : "Chưa hoàn thành";
                }
            },
        ]
    });
    $("#btnSearch").click((e) => {
        e.preventDefault();
        dataTable.ajax.reload();
    })
    $("#PhoneNumberFilter").keypress(function (event) {
        if (event.keyCode == 13 || event.which == 13) {
            console.log(event);
            event.preventDefault();  //Không cho submit from bạn có thể bỏ nều k cần
            //Các câu lệnh Logic sẽ thực hiện ở đây
            dataTable.ajax.reload();

        }
    });
})
function telephoneCheck(str) {
    $("#error").text("");
    var patt = new RegExp(/^\+?1?\s*?\(?\d{3}(?:\)|[-|\s])?\s*?\d{3}[-|\s]?\d{4}$/);
    if (patt.test(str) == true || str == "" || str == null) {
        return str;
    } else {
        $("#error").text("Định dạng số điện thoại không đúng");
        return null;
    }

}
