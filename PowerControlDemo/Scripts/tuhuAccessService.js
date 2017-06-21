function initAccess(accessKey) {
    //
    $.post('/Home/Access', { "accessKey": accessKey }, function (data) {
        if (!data) {
            $("button,input,a[tuhu-accessKey]='" + accessKey + "'").remove();
        }
    });
}