(function (initialize) {

    function Page(option) {
        this.setting = option;
        this.init();
    }

    Page.prototype.init = function () {
        var $this = this;
        this.bind();
        this.renderTable();
        util.ajaxWFService({
            url: $this.setting.config.select.url,
            type: 'GET',
            success: function (serverData) {
                $this.renderTree(serverData);
            }
        });
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
    Page.prototype.renderTable = function () {
        var $this = this;
        var config = this.setting.config;
        var selector = '#' + config.id;
        util.table({
            elem: selector
            , toolbar: '#list-bar'
            , url: config.url
            , page:false
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

    Page.prototype.renderTree = function (serverData) {
        var $this = this;
        var id = '#' + $this.setting.config.select.selector
        var treeObj = $.fn.zTree.init($(id), {
            callback: $this.setting.config.tree.callback,
            data: {
                key: {
                    name: 'Name'
                },
                simpleData: {
                    enable: true,
                    idKey: 'Id',
                    pIdKey: 'ParentId',
                    rootPId: 0
                }
            }
        }, serverData);
        var nodes = treeObj.getNodesByFilter(function (node) { return node.level == 0; });
        if (nodes.length > 0) {
            treeObj.expandNode(nodes[0]);
        }
    }
    Page.prototype.refresh = function () {
        var config = {
            page: false
        }
        this.search(config);
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
           templet: {
               checkbox: '#col-checkbox'
           },
            select: {
                url: 'api/setting/category/list',
                selector: 'ztree'
            },
            tree: {
                callback: {
                    beforeClick: function (id, node) {
                        return !node.isParent;
                    },
                    onClick: function (event, id, node) {
                        $("#hidCategoryCode").val(node.Id);
                        $("#txtCategoryName").val(node.Name);
                    },
                    onDblClick: function () {
                        $("#hidCategoryCode").val(node.Id);
                        $("#txtCategoryName").val(node.Name);
                        $("#zc").hide();
                    }
                }
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
        }
    })
});
