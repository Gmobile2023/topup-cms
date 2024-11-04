/************************************************************************
* responsive extension for datatables    - LongLD                       *
*************************************************************************/
(function ($) {
    hlsTableBackend(); 
    //console.log($.fn.dataTable.defaults); 
    function hlsTableBackend(){
        if( $(window).width() >= 768){
            $.extend(true, $.fn.dataTable.defaults, {
                "scrollX": true,
                responsive: {
                    details: false
                },
                column : {
                    sClass : "all"
                }
            });
        }else{
            if( $("body table.dataTable").find('td.all').length > 0){
                 $("body table.dataTable").find('td.all').removeClass('all');
            }
        }
    }

    $('table.table').on('show.bs.dropdown', function(e) {
        if($(e.target).parents('.dataTables_scrollBody').length > 0){
            $(e.target).parents('.dataTables_scrollBody').css({"overflow": "visible"});
        }
    }).on('hide.bs.dropdown', function (e) {
        if($(e.target).parents('.dataTables_scrollBody').length > 0){
            $(e.target).parents('.dataTables_scrollBody').css({"overflow": "auto"});
        }
    })

})(jQuery);
