﻿(function (initialize) {

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
        var $this = this;
        $.each($this.setting.event, function (propertyName) {
            var selector = '#' + propertyName;
            $(selector).click(function () {
                $this.setting.event[propertyName].call($this);
            });
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

    Page.prototype.renderTable = function () {
        var $this = this;
        var config = this.setting.config;
        var selector = '#' + config.id;
        util.table({
            elem: selector
            , toolbar: '#list-bar'
            , url: config.url
            , page: true
            , cols: [[
                { type: 'radio' }
                , { width: 60, type: 'numbers', sort: false, title: '序号', align: 'center', unresize: true }
                , {
                    field: 'Name', width: 240, title: '名称', align: 'left', templet: function (d) {
                        return d.Name + '-' + d.Version;
                    }
                }
                , { field: 'CategoryName', width: 120, title: '业务类型', sort: false, align: 'center' }
                , {
                    field: 'CreateTime', title: '创建时间', width: 150, align: 'left', templet: function (d) {
                        return layui.util.toDateString(d.CreateTime, 'yyyy.MM.dd HH:mm');
                    }
                }
                , {
                    field: 'UpdateTime', title: '更新时间', width: 150, align: 'left', templet: function (d) {
                        return layui.util.toDateString(d.UpdateTime, 'yyyy.MM.dd HH:mm');
                    }
                }
                , { field: 'Status', width: 120, title: '状态', align: 'center', templet: config.templet.checkbox, unresize: true }
                , { field: 'Memo', title: '备注', minWidth: 120, align: 'left' }
            ]]
        });

        layui.form.on(config.checkbox, function (obj) {
            var id = $(obj.elem).attr('key');
            var useState = (obj.elem.checked ? 1 : 0);
            var url = util.buildArgs("api/wf/template/{id}/change/{status}", { id: id, status: useState });
            util.ajaxWFService({
                type: 'post',
                url: url,
                dataType: 'text',
                success: function () {
                    layui.table.reload(config.id);
                }
            });
        });

        layui.table.on('toolbar(' + config.id + ')', function (obj) {
            var eventName = obj.event;
            $this.setting.methods[eventName].call($this);
        });
    }

    Page.prototype.search = function (searchCondition) {
        layui.table.reload(this.setting.config.id, searchCondition);
    }

    Page.prototype.check = function (id, callback) {
        var checkStatus = layui.table.checkStatus(id);
        var dataArray = checkStatus.data;
        if (dataArray.length == 0) {
            layer.msg('请选择记录');
        } else {
            callback && callback(dataArray[0]);
        }
    }

    Page.prototype.open = function (url) {
        var h = window.screen.availHeight;
        var w = window.screen.availWidth;
        window.open(url, '流程设计器', "width=" + w + ", height=" + h + ",top=0,left=0,titlebar=no,menubar=no,scrollbars=yes,resizable=yes,status=yes,toolbar=no,location=no");
    }

    initialize(function (option) {
        return new Page(option);
    });

})(function (initialize) {
   initialize({
        config: {
           id: 'template-table',
           checkbox: 'checkbox(form-status)',
           url: 'api/wf/template/list',
           categoryUrl: 'api/setting/category/list',
           categoryId: 'ddlType',
           templet: {
               checkbox: '#col-checkbox'
           }
        },
        methods: {
            add: function () {
                this.open('./design.html');
            },
            edit: function () {
                var $this = this;
                $this.check('template-table', function (data) {
                    $this.open('./design.html?id=' + data.Id);
                });
            }
        },
       event: {
           search: function () {
               let searchCondition = layui.form.val('form-search');
               this.search({ where: searchCondition });
           }
        }
    })
});
