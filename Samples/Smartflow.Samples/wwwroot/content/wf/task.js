(function (initialize) {

    function Page(option) {
        this.setting = option;
        this.init();
    }

    Page.prototype.init = function () {
        const $this = this;
        $this.bind();
        $this.renderTable();
    }

    Page.prototype.bind = function () {
        const $this = this;
        $.each($this.setting.event, function (propertyName) {
            var selector = '#' + propertyName;
            $(selector).click(function () {
                $this.setting.event[propertyName].call($this);
            });
        });
    }

    Page.prototype.renderTable = function () {
        const $this = this;
        var config = $this.setting.config;
        var selector = '#' + config.id;
        util.table({
            elem: selector
            , url: config.url
            , page:false
            , cols: [[
                { width: 60, type: 'numbers', sort: false, title: '序号', align: 'center', unresize: true }
                , { field: 'CategoryName', width: 120, title: '业务类型', sort: false, align: 'center' }
                , { field: 'TemplateName', width: 120, title: '模板名称', align: 'center', unresize: true }
                , { field: 'Name', width: 240, title: '任务名称', align: 'left'}
                , { field: 'Actor', title: '参与人', minWidth: 120, align: 'left' }
                , { field: 'CreateTime', title: '创建时间', width: 150, align: 'left', templet: function (d) {
                        return layui.util.toDateString(d.CreateTime, 'yyyy.MM.dd HH:mm');
                    }
                }
            ]]
        });
    }

    initialize(function (option) {
        return new Page(option);
    });
})(function (initialize) {
   initialize({
        config: {
           id: 'task-table',
           url: 'api/smf/task/list',
        },
        methods: {
        },
        event: {
        }
    })
});
