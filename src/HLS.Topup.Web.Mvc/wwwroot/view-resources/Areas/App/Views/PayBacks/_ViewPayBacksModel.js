(function ($) {
    app.modals.ViewPayBacksModal = function () {

        var _payBacksService = abp.services.app.payBacks;
        var _modalManager;
        var _dataTable;

        let viewMode = $('input[name="mode"]').val();

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PayBacks.Create'),
            edit: abp.auth.hasPermission('Pages.PayBacks.Edit'),
            approval: abp.auth.hasPermission('Pages.PayBacks.Approval'),
            cancel: abp.auth.hasPermission('Pages.PayBacks.Cancel'),
            delete: abp.auth.hasPermission('Pages.PayBacks.Delete')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('input#ImportFromExcel').on('change', uploadFileImport);

            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _dataTable = initTable();
            modal.find(".select2").select2();
            
            if (_permissions.approval !== true || $('#pbStatus').val() !== 'Init') {
                $('#approvalButton').css({'display': 'none'});
            }
        };

        function initTable(){
            let $table = _modalManager.getModal().find('#AgentPayBacksTable');
            
            return $table.DataTable({
                paging: true,
                serverSide: false,
                ordering: false,
                data: [],
                listAction: {
                    ajaxFunction: _payBacksService.getPayBacksDetailsTable,
                    inputFilter: function () {
                        return {
                            payBacksId: $('input[name="id"]').val(),
                            //productIds: getCateIds()
                        };
                    }
                },
                createdRow: function (row, data, dataIndex) {
                    $(row).attr('data-id', data.userId);
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: "order",
                        render: function ( data, type, row, meta ) {
                            return  meta.row + 1;
                        }
                    },
                    {
                        targets: 1,
                        data: "agentCode"
                    },
                    {
                        targets: 2,
                        data: "phoneNumber"
                    },
                    {
                        targets: 3,
                        data: "fullName"
                    },
                    {
                        targets: 4,
                        data: "amount",
                        render: function(amount) {
                            return Sv.NumberToString(amount);
                        }
                    },
                    {
                        targets: 5,
                        data: "transCode",
                        render: function (data, type, row) {
                            if (viewMode === 'view') {
                                return data;
                            } else {
                                return '<button class="btn btn-danger btn-remove" onclick="$(\'#AgentPayBacksTable\').DataTable().row($(this).parents(\'tr\')).remove().draw(false)"> Xoá </button>';
                            }
                        }
                    }
                ],
                order: [[ 1, 'asc' ]],
                drawCallback: function( settings) {
                    var data = _dataTable.data();
                    var t = 0;
                    if(data.length > 0){
                        t = data.reduce((a, b) => parseFloat(a) + parseFloat(b["amount"] || 0), 0);
                    }
                    $table.find("tbody.f tr td:nth-child(2)").html(Sv.NumberToString(t) + "đ");
                }
            });
        }

        function uploadFileImport() {
            let $f = _modalManager.getModal().find('input#ImportFromExcel');
            let files = $f[0].files;
            if (files && files.length > 0) {
                let file = files[0];

                //File type check
                let type = file.type.slice(file.type.lastIndexOf('/') + 1);
                let typeAlow = ['xlsx', 'xls', 'csv'];
                if (typeAlow.indexOf(type) > -1) {
                    abp.message.warn("Định dạng file không hợp lệ!");
                    resetValueFile();
                    return false;
                }

                //File size check
                if (file.size > 1048576 * 100) //100 MB
                {
                    abp.message.warn("Dung lượng file vượt quá giới hạn!");
                    resetValueFile();
                    return false;
                }
                $f.closest('label').find('span').html(file.name);

                var formData = new FormData();
                formData.append("file", file);
                _modalManager.setBusy(true);
                Sv.AjaxPostFile2({
                    url: abp.appPath + "App/PayBacks/ReadFileImport",
                    data: formData
                }).then(function(response){
                    //_dataTable.clear();
                    let data = [];
                    let path = "";
                    if (response.success){
                        data = response.result;
                    }

                    let existAgent = [];
                    $('#AgentPayBacksTable').DataTable().rows().every( function ( rowIdx, tableLoop, rowLoop ) {
                        let d = this.data();
                        if (d.userId != null || d.userId != '' || d.userId.length > 0 && d.amount != null || d.amount != '' || d.amount.length > 0) {
                            existAgent.push(d.userId);
                        }
                    } );

                    if (find_duplicate_in_array(data)) {
                        abp.message.warn("File Import có đại lý trùng lặp");
                        return false;
                    }

                    let lock = false;
                    $.each(data, function(i, e) {
                        if (!e.isActive) {
                            lock = true;
                            abp.message.warn("File Import có đại lý bị khóa");
                            return false;
                        }
                    });

                    if (lock) {
                        resetValueFile();
                        _dataTable.clear();
                        return false;
                    }

                    // Xoá object đã tồn tại trên bảng
                    if (existAgent != null || existAgent.length > 0) {
                        for (let i = data.length - 1; i >= 0; i--) {
                            let obj = data[i];

                            if (existAgent.indexOf(obj.userId) !== -1) {
                                data.splice(i, 1);
                            }
                        }
                    }
                    
                    _dataTable.rows.add(data).draw();
                    Sv.SetupAmountMask();
                    if(response.success == false){
                        resetValueFile();
                        abp.message.warn(response.error);
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });

            } else {
                _dataTable.rows.add([]).draw();
                $f.closest('label').find('span').html('Chọn File');
            }
        }

        function find_duplicate_in_array(arr) {
            let uniqueValues = new Set(arr.map(v => v.agentCode));
            return uniqueValues.size < arr.length;
        }

        function resetValueFile(){
            document.getElementById('ImportFromExcel').value = "";
        }

        function reloadTable() {
            $('#PayBacksTable').DataTable().ajax.reload();
        }

        function approval(id) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt danh sách trả phí khuyến mại này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        _payBacksService.approval(id).done(function () {
                            $('#AgentPayBacksTable').DataTable().ajax.reload();
                            abp.message.success('Trả thưởng thành công!');
                        }).always(function () {
                            abp.ui.clearBusy();
                        });
                    }
                }
            );
        }
        
        $('#approvalButton').click(function() {
            let id = $(this).attr('data-id');

            approval(id);
        });

        $('#ExportDetail').click(function () {
            _payBacksService
                .getDetailPayBacksToExcel({
                    payBacksId: $('input[name="id"]').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        this.save = function () {
            let data = [];
            let checkAmount = true;
            _dataTable.data().map(function(x, i){
                if(x.amount == null || x.amount < 0){
                    checkAmount = false;
                }
                data.push(x);
            });

            if(checkAmount == false){
                abp.message.warn("Vui lòng kiểm tra lại số tiền!");
                return false;
            }

            let formData = new FormData();
            let $f = _modalManager.getModal().find('input#ImportFromExcel');
            if($f[0].files.length == 0 && data.length == 0){
                abp.message.warn("Vui lòng chọn File Import!");
                resetValueFile();
                return false;
            }

            if(data.length == 0){
                abp.message.warn("Dữ liệu không chính xác, vui lòng thử lại!");
                resetValueFile();
                return false;
            }

            let _$importInformationsForm = _modalManager.getModal().find('form[name=PayBacksInformationsForm]');
            var obj = _$importInformationsForm.serializeFormToObject();
            if(obj.name == null || obj.name.length == 0){
                abp.message.warn("Vui lòng nhập tên chương trình!");
                return false;
            }

            let listAgent = [];
            let totalAgent = 0;
            let totalAmount = 0;

            $('#AgentPayBacksTable').DataTable().rows().every( function ( rowIdx, tableLoop, rowLoop ) {
                let data = this.data();

                if (data.userId != null || data.userId != '' || data.userId.length > 0 && data.amount != null || data.amount != '' || data.amount.length > 0) {
                    let item = {
                        userId: data.userId,
                        amount: data.amount,
                    };

                    totalAgent += 1;
                    totalAmount += data.amount;

                    listAgent.push(item);
                }
            } );

            obj.total = totalAgent;
            obj.totalAmount = totalAmount;
            obj.payBacksDetail = listAgent;

            delete obj.AgentPayBacksTable_length;

            _modalManager.setBusy(true);
            _payBacksService.createOrEdit(
                obj
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditPayBacksModalSaved');
                reloadTable();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        }
    };
})(jQuery);
