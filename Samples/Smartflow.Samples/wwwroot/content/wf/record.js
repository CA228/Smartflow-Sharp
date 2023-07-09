(function (initialize) {

    function Record(option) {
        this.setting = $.extend({}, option);
        this.init();
    }

    Record.prototype.init = function () {
        var $this = this,
            setting = $this.setting;
        $this.load(setting.instanceId);
    }

    Record.prototype.load = function (instanceId) {
        var $this = this,
            setting = this.setting,
            url = util.buildArgs($this.setting.url, { instanceId: instanceId });
        util.ajaxWFService({
            url: url,
            type: 'get',
            success: function (serverData) {
                var htmlArray = [];
                $.each(serverData, function () {
                    var el = setting.templet;
                    htmlArray.push(
                        el.replace(/{{Name}}/ig, setting.Type ? this.OrganizationName : this.Name)
                            .replace(/{{Comment}}/ig, this.Comment)
                            .replace(/{{CreateTime}}/ig, this.CreateTime ? layui.util.toDateString(this.CreateTime, 'yyyy.MM.dd HH:mm') : '')
                            //.replace(/{{Sign}}/ig, util.isEmpty(this.Url) ? '' : "<image src=\"" + this.Url + "\" />")
                            .replace(/{{CreatorName}}/ig, this.CreatorName)
                    );
                });
                $(setting.id).html(htmlArray.join(''));
                setting.done && setting.done(serverData);
            }
        });
    }

    initialize(function (option) {
        return new Record(option);
    });

})(function (initialize) {
    $.Record = function (option) {
        var templet = "<tr><td class=\"flow-node\" rowspan=\"2\">{{Name}}</td><td class=\"flow-message\">{{Comment}}</td><tr><td colspan=\"2\" class=\"flow-sign\">{{CreateTime}}&nbsp;&nbsp;&nbsp;{{CreatorName}}</td></tr></tr>";
        return initialize(Object.assign({
            templet: templet,
            id: 'table.record-table',
            url: 'api/setting/record/{instanceId}/list'
        }, option));
    }
});
