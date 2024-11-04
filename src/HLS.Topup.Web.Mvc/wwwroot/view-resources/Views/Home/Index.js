$(document).ready(function () {
    $('.group-service-item').click(function() {
        let dataIcon = $(this).attr('data-src');
        let categoryName = $(this).attr('data-category-name');
        let categoryCode = $(this).attr('data-category-code');

        localStorage.setItem('dataIcon', dataIcon);
        localStorage.setItem('categoryCode', categoryCode);
        localStorage.setItem('categoryName', categoryName);
    });
});
