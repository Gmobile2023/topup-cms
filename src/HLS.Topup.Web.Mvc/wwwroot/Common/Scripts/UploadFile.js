(function () {
    $(function () {
        console.log('a');
        $("#imageId").on('change', function () {
            console.log('b');
            app.uploadImage($("#imageId"), $('#thumbImageId'));
        });
    });
})();
