(function (initialize) {

    function Page(option) {
        this.setting = option;
        this.init();
    }

    Page.prototype.init = function () {
        this.renderTable();
    }

    Page.prototype.renderTable = function () {
        var config = this.setting.config;
        var selector = '#' + config.id;
        layui.table.render({
            elem: selector
            , url: util.prefix+config.url
            , page: false
            , defaultToolbar: false
            , method: 'get'
            , contentType: 'application/json'
            , cellMinWidth: 80
            , parseData: function (res) {
                return {
                    "code":0,
                    "msg": '',
                    "count": res.length,
                    "data": res
                };
            }
            , cols: [[
                { width: 60, type: 'numbers', sort: false, title: '序号', align: 'center', unresize: true }
                , { field: 'Name', width: 160, title: '姓名', align: 'center' }
                , { field: 'OrganizationName', width: 180, title: '所属单位', sort: false, align: 'center' }
                , { field: 'UID', title: '账号', width: 140, align: 'center' }
                , { field: 'RoleGroup', title: '所属角色组', minWidth: 120, align: 'left' }
            ]]
        });
    }

    initialize(function (option) {
        return new Page(option);
    });

})(function (initialize) {
   initialize({
        config: {
           id: 'user-table',
           url: 'api/user/list',
        }
    })
});
