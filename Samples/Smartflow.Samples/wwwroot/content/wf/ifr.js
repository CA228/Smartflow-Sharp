(function (initialize) {

    function Frame(option) {
        this.option = option;
    }

    Frame.prototype.load = function (nx) {
        var $opt = this.option;
       // setTimeout(function () {
            $($opt.el).each(function () {
                $(this).context.contentWindow.setting.load(nx);
            });
       // }, 1000);
    }
    Frame.prototype.set = function (nx) {
        var $opt = this.option;
        $($opt.el).each(function () {
            $(this).context.contentWindow.setting.set(nx);
        });
    }

    initialize(function (option) {
        return new Frame(option);
    });

})(function (createInstance) {
    window.setting = createInstance({
        el: 'iframe'
    });
});
