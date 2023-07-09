; (function (initialize) {

    function Transfer(option) {
        this.option = option;
        this.init();
        this.bind();
    }

    Transfer.prototype.set = function (nx) {
        var table = layui.table,
            cacheData = table.cache.right;
        var carbonArray = [];

        $(cacheData).each(function () {
            var self = this;
            carbonArray.push({
                id: self.ID,
                name: self.Name
            });
        });

        nx.carbon = carbonArray;
    }
    Transfer.prototype.load = function (nx) {
        var table = layui.table,
            $this = this,
            $opt = $this.option;

        var carbons = [];
        $.each(nx.carbon, function () {
            carbons.push(this.id);
        });

        util.table({
            elem: $opt.left.el
            , url: $opt.left.url
            , height: 'full-90'
            , page: { layout: ['prev', 'page', 'next', 'count'] }
            , where: {
                arg: JSON.stringify({ carbon: carbons.join(',') })
            }
            , cols: [[
                { checkbox: true, fixed: true }
                , { field: 'ID', title: 'ID', hide: true }
                , { field: 'Name', title: '用户名', width: 120, align: 'left' }
                , {
                    title: '部门名称', templet: function (d) {
                        return d.Data.OrgName;
                    }
                }
            ]]
        });
        util.table({
            elem: $opt.right.el
            , url: $opt.right.url
            , height: 'full-90'
            , where: {
                arg: JSON.stringify({ carbon: carbons.join(',') })
            }
            , page: false
            , cols: [[
                { checkbox: true, fixed: true }
                , { field: 'ID', title: 'ID', hide: true }
                , { field: 'Name', title: '用户名', width: 120, align: 'center' }
                , {
                    title: '部门名称', templet: function (d) {
                        return d.Data.OrgName;
                    }
                }
            ]]
        });
        var ls = 'checkbox(' + $opt.left.filter + ')',
            rs = 'checkbox(' + $opt.right.filter + ')';
        var mulitSelector = [{ selector: ls, filter: 'left', id: 'div.arrow-right' }, { selector: rs, filter: 'right', id: 'div.arrow-left' }];
        $.each(mulitSelector, function () {
            var info = this;
            table.on(info.selector, function (obj) {
                var checkStatus = table.checkStatus(info.filter), data = checkStatus.data;
                var methodName = data.length > 0 ? 'removeClass' : 'addClass';
                $(info.id)[methodName]('layui-btn-disabled');
            });
        });
    }
    Transfer.prototype.init = function () {
        var $this = this;
        util.ajaxWFService({
            type: 'get',
            url: $this.option.tree.url,
            success: function (serverData) {
                $this.renderTree(serverData);
            }
        });
    }
    Transfer.prototype.renderTree = function (data) {
        var $this = this,
            $opt = $this.option;
        $.fn.zTree.init($($opt.tree.el), {
            callback: $opt.tree.callback,
            data: {
                key: {
                    name: 'Name'
                },
                simpleData: {
                    enable: true,
                    idKey: 'ID',
                    pIdKey: 'ParentID',
                    rootPId: 0
                }
            }
        }, data);
    }
    Transfer.prototype.bind = function () {
        var $this = this,
            $opt = $this.option;
        for (let propertyName in $opt.event) {
            const selector = '#' + propertyName;
            $(selector).click(function (e) {
                $opt.event[propertyName].call($this, this, e);
            });
        }
    }

    initialize(function (option) {
        return new Transfer(option);
    });

})(function (createInstance) {
    var instance = createInstance({
        left: {
            el: 'table.left',
            url: 'api/setting/carbon/page',
            filter: 'left'
        },
        right: {
            el: 'table.right',
            url: 'api/setting/assign-carbon/page',
            filter: 'right'
        },
        tree: {
            el: 'div.ztree',
            url: 'api/setting/organization/list',
            callback: {
                onClick: function (event, treeId, node) {
                    $("input.node-text").val(node.Name);
                    $("input.node-value").val(node.ID);
                }
            }
        },
        event: {
            toLeft: function (o) {
                var $this = $(o),
                    $opt = this.option,
                    key = $("input.input-key").val();
                if (!$this.hasClass('layui-btn-disabled')) {
                    var checkStatus = layui.table.checkStatus($opt.right.filter);
                    removeData(checkStatus.data);

                    var cacheData = layui.table.cache.right;
                    var carbons = [];
                    $.each(cacheData, function () {
                        carbons.push(this.ID);
                    });

                    var config = {
                        page: { curr: 1 },
                        where: {
                            arg: JSON.stringify({
                                searchKey: key,
                                carbon: carbons.join(',')
                            })
                        }
                    };

                    layui.table.reload($opt.left.filter, config);
                    layui.table.reload($opt.right.filter, {
                        page: false,
                        where: {
                            arg: JSON.stringify({ carbon: carbons.join(',') })
                        }
                    });

                    $this.addClass('layui-btn-disabled');
                }

                function removeData(selectDataArray) {
                    var cacheData = layui.table.cache.right;
                    $.each(selectDataArray, function () {
                        for (var i = 0, len = cacheData.length; i < len; i++) {
                            var c = cacheData[i];
                            if (this.ID == c.ID) {
                                cacheData.splice(i, 1);
                                break;
                            }
                        }
                    });
                }
            },
            toRight: function (o) {
                var $this = $(o),
                    $opt = this.option,
                    key = $("input.input-key").val();
                if (!$this.hasClass('layui-btn-disabled')) {
                    var checkStatus = layui.table.checkStatus($opt.left.filter);
                    if (checkStatus.data.length > 0) {
                        var cacheData = layui.table.cache.right;
                        var carbons = [];
                        $.each(cacheData, function () {
                            carbons.push(this.ID);
                        });
                        if (checkStatus.data.length > 0) {
                            $.each(checkStatus.data, function () {
                                carbons.push(this.ID);
                            });
                        }

                        var config = {
                            page: { curr: 1 },
                            where: { arg: JSON.stringify({ searchKey: key, carbon: carbons.join(',') }) }
                        };

                        layui.table.reload($opt.left.filter, config);
                        layui.table.reload($opt.right.filter, {
                            page: false,
                            where: { arg: JSON.stringify({ carbon: carbons.join(',') }) }
                        });
                    }
                    $this.addClass('layui-btn-disabled');
                }
            },
            reload: function () {
                var $this = this,
                    key = $("input.input-key").val(),
                    code = $("input.node-value").val(),
                    cacheData = layui.table.cache.right,
                    carbons = [];

                $.each(cacheData, function () {
                    carbons.push(this.ID);
                });
                var searchCondition = {
                    searchKey: key,
                    orgCode: code,
                    carbon: carbons.join(',')
                };

                $('div.arrow-right').addClass('layui-btn-disabled');
                layui.table.reload($this.option.left.filter, {
                    page: { curr: 1 },
                    where: {
                        arg: JSON.stringify(searchCondition)
                    }
                });
            }
        }
    });

    window.setting = instance;

    $('div.zc-ztree').hover(function () { }, function () {
        $(this).hide();
    });
});
