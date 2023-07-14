(function (initialize) {

    Configuration.controlSelectors = {
        node_actor: {
            title: '参与者',
            type: 'box',
            width: '900px',
            height: '680px',
            url: './actorSelect.html',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            }
        },
        node_name: {
            field: 'name',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            },
            invoke: function (nx) {
                $('#node_name').val(nx.name);
            }
        },
        node_id: {
            field: 'id',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            },
            invoke: function (nx) {
                $('#node_id').val(nx.id);
            }
        },
        line_name: {
            field: 'name',
            parse: function (id) {
                return $.SMF.getLineById(id);
            },
            invoke: function (nx) {
                $('#line_name').val(nx.name);
            }
        },
        line_id: {
            field: 'id',
            parse: function (id) {
                return $.SMF.getLineById(id);
            },
            invoke: function (nx) {
                $('#line_id').val(nx.id);
            }
        },
        line_order: {
            field: 'order',
            parse: function (id) {
                return $.SMF.getLineById(id);
            },
            invoke: function (nx) {
                $('#line_order').val(nx.order);
            }
        },
        line_url: {
            field: 'url',
            parse: function (id) {
                return $.SMF.getLineById(id);
            },
            invoke: function (nx) {
                $('#line_url').val(nx.url);
            }
        }
    };

    Configuration.findElementById = function (o) {
        var id = $(o).attr('id');
        var controlSelector = Configuration.controlSelectors[id];
        return {
            element: this.element,
            descriptor: controlSelector
        };
    };

    Configuration.show = function (settings) {
        $.each(settings, function (i, propertyName) {
            var selector = '#' + propertyName;
            $(selector).show();
            $(selector).siblings().each(function () {
                $(this).hide();
            });
        });
    };

    Configuration.getDOMFrame = function (dom) {
        var frameId = dom.find("iframe").attr('id');
        return document.getElementById(frameId).contentWindow;
    };

    Configuration.open = function (nx, descriptor) {
        var settings = {
            type: 2,
            title: descriptor.title,
            area: [descriptor.width, descriptor.height],
            shade: 0.8,
            closeBtn: 1,
            shadeClose: false,
            content: [descriptor.url, 'no']
        };

        if (!!descriptor.btn) {
            settings.btn = descriptor.btn;
        }

        if (!!descriptor.yes) {
            settings.yes = function (index,dom) {
                descriptor.yes.call(this, nx, index, dom);
            }
        }

        settings.cancel = function (index, dom) {
            var frameContent = Configuration.getDOMFrame(dom);
            frameContent.setting.set(nx);
            if (descriptor.invoke) {
                descriptor.invoke(nx);
            }
        };

        settings.success = function (dom, index) {
            var frameContent = Configuration.getDOMFrame(dom);
            frameContent.setting.load(nx);
        };

        layer.open(settings);
    };

    function Configuration(option) {
        this.option = $.extend({}, option);
        this.init();
        this.bind();
    }

    Configuration.prototype.init = function () {
        var $this = this;
        var id = util.doQuery('id');
        $("#drawing").SMF({
            container: this.option.container,
            dblClick: function (nx) {
                $this.element = nx;
                $this.selectTab.call($this, nx);
            }
        });

        if (id) {
            var url = util.buildArgs($this.option.url, { id: id});
            util.ajaxWFService({
                url: url,
                type: 'Get',
                success: function (serverData) {
                    var instance = $.SMF.getInstanceComponent();
                    instance.import(serverData.Source);
                }
            });
        } else {
            $.each(['start', 'end'], function (i, value) {
                var instance = $.SMF.getInstanceComponent();
                instance.create(value, false);
            });
        }

        //渲染所有表单
        layui.form.render();
    };

    Configuration.prototype.bind = function () {
        var $this = this;
        for (var propertyName in Configuration.controlSelectors) {
            var selector = '#' + propertyName,
                sel = Configuration.controlSelectors[propertyName];
            if (sel.type === 'box') {
                $(selector).click(function () {
                    var result = Configuration.findElementById.call($this, this);
                    Configuration.open(result.element, result.descriptor);
                });
            }
            else {
                $(selector).keyup(function () {
                    var result = Configuration.findElementById.call($this, this);
                    var obj = Configuration.controlSelectors[this.id];
                    var text = $(this).val();
                    result.element[obj.field] = text;
                    if (result.element.brush && obj.field==='name') {
                        result.element.brush.text(text);
                    }
                });
            }
        }
    };

    Configuration.prototype.selectTab = function (nx) {
        var $this = this,
            category = nx.category.toLowerCase();
        if (category === 'node') {
            Configuration.show([$this.option.node]);
            var controls = ['node_name','node_id'];
            $.each(controls, function (i, propertyName) {
                if (Configuration.controlSelectors[propertyName].invoke) {
                    Configuration.controlSelectors[propertyName].invoke(nx, $this);
                }
            });
        }
        else if (category === 'line') {
            Configuration.show([$this.option.line]);
            var controls = ['line_url', 'line_name', 'line_id', 'line_order'];
            $.each(controls, function (i, propertyName) {
                if (Configuration.controlSelectors[propertyName].invoke) {
                    Configuration.controlSelectors[propertyName].invoke(nx, $this);
                }
            });
        }
        else {
            Configuration.show([$this.option.help]);
            if (category === 'decision') {
                Configuration.open(nx, {
                    title: '属性',
                    width: '900px',
                    height: '680px',
                    url: './condition.html'
                });
            }
        }
    };

    Configuration.prototype.prompt = function (elementId, instance,method) {
        var ht = $("#" + elementId).html(),
            $this = this;
        var id = util.doQuery('id');
        if (id) {
            $this.set(id);
        }
        layer.open({
            title: '流程信息',
            type: 1,
            closeBtn: 1,
            area: ['520px', '350px'],
            anim: 2,
            shadeClose: false,
            content: ht,
            btnAlign: 'c',
            btn: ['确定'],
            success: function (dom, index) {
                var form = layui.form;
                $this.loadCategory(function () {
                    form.render(null, 'layui_flow_info');
                });
            },
            yes: function (index, dom) {
                var form = layui.form,
                    formData = form.val('layui_flow_info'),
                    id = util.doQuery('id');
                if (id) {
                    formData.Id = id;
                }
                formData.Source = instance.export();
                util.ajaxWFService({
                    dataType:'text',
                    url: $this.option[method],
                    data: JSON.stringify(formData),
                    success: function () {
                        layer.closeAll();
                        window.opener.top.util.refreshTabPage();
                        window.close();
                    }
                });
            }
        });
    }

    Configuration.prototype.set = function (id) {
        var url = util.buildArgs(this.option.url, { id: id });
        var $this = this;
        util.ajaxWFService({
            url: url,
            type: 'Get',
            success: function (serverData) {
                var form = layui.form;
                $this.select(serverData.CategoryCode, 'ztree');
                form.val('layui_flow_info', serverData);
            }
        });
    }

    Configuration.prototype.select = function (code, treeId) {
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        if (!!treeObj) {
            var nodes = treeObj.getNodesByParam("Code", code, null);
            if (nodes.length > 0) {
                var n = nodes[0];
                treeObj.selectNode(n);
                $('#txtCateName').val(n.Name);
            }
        }
    }

    Configuration.prototype.loadCategory = function (callback) {
        var url = this.option.categoryUrl,
            id = '#' + this.option.categoryId;
        util.ajaxWFService({
            url: url,
            type: 'GET',
            success: function (serverData) {
                var treeObj = $.fn.zTree.init($(id), {
                    beforeClick: function (id, node) {
                        return !node.isParent;
                    },
                    callback: {
                        onClick: function (event, id, node) {
                            $("#hidCategoryCode").val(node.Code);
                            $("#txtCategoryName").val(node.Name);
                        },
                        onDblClick: function (event, id, node) {
                            $("#hidCategoryCode").val(node.Code);
                            $("#txtCategoryName").val(node.Name);
                            $("#zc").hide();
                        }
                    },
                    data: {
                        key: {
                            name: 'Name'
                        },
                        simpleData: {
                            enable: true,
                            idKey: 'Code',
                            pIdKey: 'ParentCode',
                            rootPId: 0
                        }
                    }
                }, serverData);
                var nodes = treeObj.getNodesByFilter(function (node) { return node.level == 0; });
                if (nodes.length > 0) {
                    treeObj.expandNode(nodes[0]);
                }
                callback && callback();
            }
        });
    }

    Configuration.prototype.save = function () {
        var instance = $.SMF.getInstanceComponent();
        if (instance.validate()) {
            var id = util.doQuery('id');
            var template = id == null?'smartflow_s_info': 'smartflow_e_info';
            this.prompt(template, instance,'save');
        } else {
            alert('流程图不符合流程定义规则');
        }
    }

    Configuration.prototype.saveas = function () {
        var instance = $.SMF.getInstanceComponent();
        if (instance.validate()) {
            this.prompt('smartflow_s_info', instance,'saveAs');
        } else {
            alert('流程图不符合流程定义规则');
        }
    }

    initialize(function (option) {
        return new Configuration(option);
    });

})(function (initialize) {
    window.design = {
        Configuration: initialize
    };
});