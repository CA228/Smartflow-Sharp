(function (initialize) {

    function Page(option) {
        this.setting = option;
        this.init();
    }

    Page.prototype.init = function () {
        const $this = this;
        $this.bind();
        $this.loadCategory();
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
            , page: true
            , cols: [[
                { width: 60, type: 'numbers', sort: false, title: '序号', align: 'center', unresize: true }
                , { field: 'Sender', title: '实例发起人', width: 120, align: 'center', unresize: true }
                , { field: 'CategoryName', width: 120, title: '业务类型', sort: false, align: 'center' }
                , { field: 'TemplateName', width: 120, title: '模板名称', align: 'center', unresize: true }
                , { field: 'Name', width: 240, title: '节点名称', align: 'center' }
                , { field: 'Actor', title: '参与人', minWidth: 120, align: 'left' }
                , {
                    field: 'CreateTime', title: '创建时间', width: 150, align: 'center', templet: function (d) {
                        return layui.util.toDateString(d.CreateTime, 'yyyy.MM.dd HH:mm');
                    }
                }
            ]]
        });
    }

    Page.prototype.loadCategory = function () {
        var config = this.setting.config;
        var url = config.categoryUrl,
            id = '#' + config.categoryId;
        util.ajaxWFService({
            url: url,
            type: 'GET',
            success: function (serverData) {
                var htmlArray = [];
                htmlArray.push("<option value=\"\"></option>");
                $.each(serverData, function () {
                    htmlArray.push("<option value='" + this.Id + "'>" + this.Name + "</option>");
                });
                $(id).html(htmlArray.join(''));
                layui.form.render('select');
            }
        });
    }

    Page.prototype.search = function (searchCondition) {
        layui.table.reload(this.setting.config.id, searchCondition);
    }

    initialize(function (option) {
        return new Page(option);
    });
})(function (initialize) {
    initialize({
        config: {
            id: 'task-table',
            url: 'api/smf/task/list',
            categoryUrl: 'api/setting/category/list',
            categoryId: 'ddlType',
        },
        methods: {

        },
        event: {
            search: function () {
                let searchCondition = layui.form.val('form-search');
                this.search({ where: searchCondition });
            }
        }
    })
});
