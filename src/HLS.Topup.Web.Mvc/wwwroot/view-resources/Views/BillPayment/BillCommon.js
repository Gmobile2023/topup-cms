$('.btn-to-next').click(function() {
    let target = $(this).attr('data-move-to');
    Sv.RequestStart();
    clickHandleShowHide(target);
});

$('.btn-to-prev').click(function() {
    let target = $(this).attr('data-move-to');
    Sv.RequestStart();
    //clearLocalStorageKey("productCode");
    //clearLocalStorageKey("categoryCode");
    //clearLocalStorageKey("amount");
    clickHandleShowHide(target);
});

function getLocalStorage(key) {
    return localStorage.getItem(key)
}

function setLocalStorage(key, val) {
    localStorage.setItem(key, val);
}
function clearLocalStorageKey(key) {
    localStorage.removeItem(key);
}
function clearLocalStorage(data) {
    $.each(data, function (index, val) {
        localStorage.setItem(data[index], '');
    });
}

function clickHandleShowHide(target) {
    if (target > 0) {
        $('div[data-target="' + target + '"]').slideDown();
        $('div[data-target]').not('div[data-target="' + target + '"]').slideUp();
        Sv.RequestEnd();
    }
}
