Vue.filter('formatDate', function (value) {
    if (value === 'null' || value === null || value === '' || value === undefined) {
        return '';
    } else {
        return layui.util.toDateString(value, 'yyyy.MM.dd HH:mm');
    }
});

Vue.filter('formatShortDate', function (value) {
    return layui.util.toDateString(value, 'yyyy.MM.dd');
});

Vue.filter('formatShort2Date', function (value) {
    return layui.util.toDateString(value, 'MM.dd');
});