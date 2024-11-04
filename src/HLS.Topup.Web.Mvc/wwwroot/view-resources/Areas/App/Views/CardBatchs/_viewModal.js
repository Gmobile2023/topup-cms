(function ($) {
    app.modals.ViewCardBatchModal = function () {
        this.init = function (modalManager) { 
            var modal = modalManager.getModal();
            modal.find('#ProductTable').DataTable({
                paging: true,
                serverSide: false,  
                data: window.cardBatchItems,
                columnDefs: [
                    {
                        targets: 0,
                        data: "ServiceCode", 
                    },  
                    {
                        targets: 1,
                        data: "CategoryName", 
                    },
                    {
                        targets: 2,
                        data: "ProductName", 
                    },
                    {
                        targets: 3,
                        data: "ItemValue",
                        class : "text-right",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data) ;
                        }
                    },
                    {
                        targets: 4,
                        class : "text-right",
                        data: "Quantity",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data) ;
                        }
                    },
                    {
                        targets: 5,
                        class : "text-right",
                        data: "QuantityImport",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data) ;
                        }
                    },
                    {
                        targets: 6,
                        class : "text-right",
                        data: "Discount",
                        render: function (data, e, row) {
                            return (data) + "%" ;
                        }
                    },
                    {
                        targets: 7,
                        class : "text-right",
                        data: "Amount",
                        render: function (data, e, row) {
                            if(row.QuantityImport <= 0)
                                return 0;
                            var val = (row.QuantityImport * row.ItemValue ) - (row.QuantityImport * row.ItemValue) * (row.Discount/100);
                            return Sv.NumberToString(val) + "đ" ;
                        }
                    } 
                ]
            });
        }; 
    };
})(jQuery);
