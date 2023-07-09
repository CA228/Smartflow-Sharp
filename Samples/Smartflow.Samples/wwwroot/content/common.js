(function () {
    window.util = {
        prefix: 'http://localhost:8083/',
        smf: 'http://localhost:8097/',
        pending: 'http://localhost:8083/pages/task.html?id=',
        ajaxService: function (settings) {
            var url = util.prefix + settings.url;
            var defaultSettings = $.extend({
                dataType: 'json',
                type: 'post',
                contentType: 'application/json',
                cache: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('流程发生异常,请与系统管理员联系');
                }
            }, settings, { url: url });
            $.ajax(defaultSettings);
        },
        ajaxWFService: function (settings) {
            var url = util.smf + settings.url;
            var defaultSettings = $.extend({
                dataType: 'json',
                type: 'post',
                contentType: 'application/json',
                cache: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('流程发生异常,请与系统管理员联系');
                }
            }, settings, { url: url });
            $.ajax(defaultSettings);
        },
        confirm: function (message, callback) {
            window.top.layer.confirm(message, {
                title: util.title,
                btn: ['确定', '取消']
            }, function (index) {
                callback & callback();
                window.top.layer.close(index);
            }, function (index) {
                window.top.layer.close(index);
            });
        },
        message: {
            record: '请选择记录',
            running: '请选择正在运行中流程',
            success: '操作成功',
            kill: '确定终止流程',
            delete: '确定删除'
        },
        refreshTabPage: function () {
            var content = window.top.$('#ifrmContent');
            if (content.length > 0) {
                var r = content.get(0).contentWindow.location;
                var url = util.refreshUrl(r.origin + r.pathname, r);
                content.attr('src', url);
            }
        },
        formatSearch: function (search) {
            var obj = {};
            search = search.slice("1").split("&");
            for (var key in search) {
                var strs = search[key].split("=");
                if (strs[0] in obj == false) {
                    obj[strs[0]] = strs[1];
                } else if (Array.isArray(obj[strs[0]])) {
                    obj[strs[0]].push(strs[1]);
                } else {
                    obj[strs[0]] = obj[strs[0]].split(" ");
                    obj[strs[0]].push(strs[1]);
                }
            }
            return obj;
        },
        refreshUrl: function (url, r) {
            var timespan = (new Date()).getTime();
            if (!!r.search) {
                var search = util.formatSearch(r.search);
                var arr = [];
                if (search['r']) {
                    search.r = timespan;
                }
                else {
                    arr.push('r=' + timespan);
                }
                for (var p in search) {
                    arr.push(p + '=' + search[p]);
                }

                url = url + '?' + arr.join('&');
            } else {
                url = url + '?r=' + timespan;
            }
            return url;
        },
        serialize: function (obj) {
            var arr = [];
            for (var p in obj) {
                arr.push(p + '=' + obj[p]);
            }
            return arr.join('&');
        },
        create: function (option) {
            var url = util.prefix + option.url;
            var defaultSettings = {
                type: 2,
                content: url,
                title: false,
                closeBtn: 1,
                shade: [0.6],
                shadeClose: false,
                area: [option.width + 'px', option.height + 'px'],
                offset: 'auto'
            };

            var setting = $.extend({}, option);
            $.each(['width', 'height', 'url'], function (i, name) {
                if (setting[name]) {
                    delete setting[name];
                }
            });
            var index = 0;
            if (setting.host === 'window') {
                delete setting[host];
                index = layer.open($.extend(defaultSettings, setting));
            } else {
                index = window.top.layer.open($.extend(defaultSettings, setting));
            }
            return index;
        },
        openLayer: function (wnd, args) {
            if ($.isPlainObject(wnd)) {
                var url = !!args ? wnd.url + '?' + util.serialize(args) : wnd.url;
                util.create($.extend({}, wnd, { url: url }));
            } else {
                util.create({
                    title: false,
                    width: 500,
                    url: wnd,
                    height: 420
                });
            }
        },
        openFullLayer: function (name, args, title) {
            var wnd = window.top.util.windows[name];
            var url = !!args ? wnd.url + '?' + util.serialize(args) : wnd.url;
            var option = $.extend({}, wnd, { url: util.prefix + url });
            var defaultSettings = {
                type: 2,
                content: url,
                title: false,
                closeBtn: 1,
                shade: [0.6],
                shadeClose: false,
                offset: 'auto'
            };
            var setting = $.extend({}, option);
            $.each(['width', 'height', 'url'], function (i, name) {
                if (setting[name]) {
                    delete setting[name];
                }
            });
            var index = window.top.layer.open($.extend(defaultSettings, setting, { title: title }));
            window.top.layer.full(index);
        },
        openDetailFullLayer: function (url, args, title) {
            var url = !!args ? url + '?' + util.serialize(args) : url;
            var option = $.extend({}, { url: util.prefix + url });
            var defaultSettings = {
                type: 2,
                content: url,
                title: false,
                closeBtn: 1,
                shade: [0.6],
                shadeClose: false,
                offset: 'auto'
            };
            var setting = $.extend({}, option);
            var index = window.top.layer.open($.extend(defaultSettings, setting, { title: title }));
            window.top.layer.full(index);
        },
        isEmpty: function (value) {
            return (value == '' || !value) ? false : true;
        },
        openWin: function (url, title, width, height) {
            var h = height || 720;
            var w = width || 1000;
            var top = (window.screen.availHeight - h) / 2;
            var left = (window.screen.availWidth - w) / 2;
            window.open(url, title, "width=" + w + ", height=" + h + ",top=" + top + ",left=" + left + ",titlebar=no,menubar=no,scrollbars=yes,resizable=yes,status=yes,toolbar=no,location=no");
        },
        doQuery: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var rw = window.location.search.substr(1).match(reg);
            return (rw != null) ? rw[2] : false;
        },
        getUser: function () {
            return JSON.parse(Base64.fromBase64(window.localStorage.getItem('ticket')));
        },
        table: function (option) {
            var url = util.smf + option.url;
            var setting = {
                page: true
                , defaultToolbar: false
                , method: 'post'
                , contentType: 'application/json'
                , cellMinWidth: 80
                , parseData: function (res) {
                    return {
                        "code": (res.Code == 200 ? 0 : res.Code),
                        "msg": (res.Code == 200 ? '' : res.Data),
                        "count": res.Total,
                        "data": res.Data
                    };
                }
            };
            layui.table.render($.extend(setting, option, { url: url }));
        },
        getArgs:function(){
            var args = {};
            var query = window.location.search.substring(1); 
            var pairs = query.split("&"); 
            for (var i = 0; i < pairs.length; i++) {
                var pos = pairs[i].indexOf('='); 
                if (pos == -1) continue; 
                var argname = pairs[i].substring(0, pos); 
                var value = pairs[i].substring(pos + 1); 
                value = decodeURIComponent(value); 
                args[argname] = value; 
            }
            return args; 
        },
        closePage: function () {
            window.top.layer.closeAll();
        },
        buildArgs: function (url, params) {
            var r = url;
            for (var propertyName in params) {
                r = r.replace("{" + propertyName + "}", params[propertyName]);
            }
            return r;
        }
    };
})();