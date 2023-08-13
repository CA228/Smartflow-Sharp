(function (initialize) {

    Configuration.controlSelectors = {
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
        node_category: {
            field: 'category',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            },
            invoke: function (nx) {
                const selector = '#node_category';
                $(selector).val(nx.category);
                layui.form.render('select');
            }
        },
        node_group: {
            field: 'group',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            },
            invoke: function (nx) {
                const selector = '#node_group';
                $(selector).val(null);
                $.each(nx.group, function () {
                    $(selector).append(new Option(this.name, this.id, true, true));
                });
                $(selector).trigger('change');
            }
        },
        node_organization: {
            field: 'organization',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            },
            invoke: function (nx) {
                const selector = '#node_organization';
                $(selector).val(null);
                $.each(nx.organization, function () {
                    $(selector).append(new Option(this.name, this.id, true, true));
                });
                $(selector).trigger('change');
            }
        },
        node_user: {
            field: 'actor',
            parse: function (id) {
                return $.SMF.getNodeById(id);
            },
            invoke: function (nx) {
                const selector = '#node_user';
                $(selector).val(null);
                $.each(nx.actor, function () {
                    $(selector).append(new Option(this.name, this.id, true, true));
                });
                $(selector).trigger('change');
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
        },
        line_expression: {
            field: 'expression',
            parse: function (id) {
                return $.SMF.getLineById(id);
            },
            invoke: function (nx) {
                $('#line_expression').val(nx.expression);
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

    function Configuration(option) {
        this.option = $.extend({}, option);
        this.init();
        this.bind();
    }

    Configuration.prototype.init = function () {
        const $this = this, sl = [];
        let id = util.doQuery('id');
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
                    const instance = $.SMF.getInstanceComponent();
                    instance.import(serverData.Source);
                }
            });
        } else {
            $.each(['start', 'end'], function (i, value) {
                const instance = $.SMF.getInstanceComponent();
                instance.create(value, false);
            });
        }

        sl.push({
            field:'group',
            id: 'node_group',
            url: "api/setting/group/list"
        });

        sl.push({
            field: 'actor',
            id: 'node_user',
            url: "api/setting/user/list",
            templateResult: function (item) {
                if (item.loading) {
                    return item.text;
                }
                return item.store.OrganizationName + '/' + item.text;
            }
        });

        sl.push({
            field: 'organization',
            id: 'node_organization',
            url: "api/setting/organization/list"
        });

        $.each(sl, function () {
            $this.initSelect($.extend({
                handleResult: function (data, params) {
                    var r = [];
                    if (data) {
                        $.each(data, function () {
                            r.push({ id: this.Id, text: this.Name, store: this })
                        });
                    }
                    return {
                        results: r
                    };
                }
            }, this));
        });

        //渲染所有表单
        layui.form.render();
    };

    Configuration.prototype.initSelect = function (group) {
        const $this = this, url = util.smf + group.url;
        const dropdownSelector = '#' + group.id;
        const ajaxOption = {
            url: url,
            delay: 250,
            method: 'get',
            data: function (params) {
                return {
                    searchKey: params.term
                };
            },
            processResults: group.handleResult,
            cache: true
        };
        $(dropdownSelector).select2({
            language: "zh-CN",
            ajax: ajaxOption,
            multiple: true,
            placeholder: '请选择',
            minimumInputLength: 1,
            minimumResultsForSearch: Infinity,
            templateResult: !!group.templateResult ? group.templateResult : function (item) {
                if (item.loading) {
                    return item.text;
                }
                return item.text;
            },
            templateSelection: function(item) {
                return item.text;
            } 
        });

        $(dropdownSelector).on("select2:select select2:unselect", function (e) {
            const nx = $this.element, VK = [];
            const sdata = $(this).select2('data');
            if (sdata && sdata.length > 0) {
                $.each(sdata, function () {
                    VK.push({ id: this.id, name: this.text });
                });
            }
            nx[group.field] = VK;
        });
    }

    Configuration.prototype.bind = function () {
        const $this = this;
        for (let propertyName in Configuration.controlSelectors) {
            let selector = '#' + propertyName;
            if (propertyName === 'node_category') {
                layui.form.on('select(node_category)', function (data) {
                    var el = data.elem; 
                    var value = data.value; 
                    var result = Configuration.findElementById.call($this, el);
                    var obj = Configuration.controlSelectors[el.id];
                    result.element[obj.field] = value;
                });
            }
            else if ($.inArray(propertyName, ['node_group', 'node_organization', 'node_user']) === -1) {
                $(selector).keyup(function () {
                    var result = Configuration.findElementById.call($this, this);
                    var obj = Configuration.controlSelectors[this.id];
                    var text = $(this).val();
                    result.element[obj.field] = text;
                    if (result.element.brush && obj.field === 'name') {
                        result.element.brush.text(text);
                    } else if (obj.field === 'name' && this.id === 'line_name') {
                        result.element.updateContent(text);
                    }
                });
            } 
        }
    };

    Configuration.prototype.selectTab = function (nx) {
        const $this = this,
            category = nx.category.toLowerCase();
        if (category === 'node' || category === 'collaboration') {
            Configuration.show([$this.option.node]);
            const controls = ['node_name', 'node_id', 'node_group', 'node_user', 'node_organization','node_category'];
            $.each(controls, function (i, propertyName) {
                if (Configuration.controlSelectors[propertyName].invoke) {
                    Configuration.controlSelectors[propertyName].invoke(nx, $this);
                }
            });
        }
        else if (category === 'line') {
            Configuration.show([$this.option.line]);
            const controls = ['line_url', 'line_name', 'line_id', 'line_order','line_expression'];
            $.each(controls, function (i, propertyName) {
                if (Configuration.controlSelectors[propertyName].invoke) {
                    Configuration.controlSelectors[propertyName].invoke(nx, $this);
                }
            });
        }
        else {
            Configuration.show([$this.option.help]);
        }
    };

    Configuration.prototype.prompt = function (elementId, instance, method) {
        const ht = $("#" + elementId).html(),
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
            success: function () {
                var form = layui.form;
                $this.loadCategory(function () {
                    form.render(null, 'layui_flow_info');
                });
            },
            yes: function () {
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
            var template = id ? 'smartflow_e_info' : 'smartflow_s_info';
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