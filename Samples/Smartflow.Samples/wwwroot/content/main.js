(function (initialize) {

    function Page(option) {
        this.setting = $.extend({}, option);
        this.user = util.getUser();
        this.init();
    }

    Page.prototype.init = function () {
        this.bind();
        this.loadMenu();
        this.loadTab();
    }

    Page.prototype.loadMenu = function () {
        var self = this;
        $(".menu_title").click(function () {
            var element = $(this).parent().children('ul').first();
            if (element.is(":hidden")) {
                element.show('fast');
            } else {
                element.hide('slow');
            }

            $(this).parent().siblings().each(function () {
                var ele = $(this).find("ul.menu_sub_items");
                if (!ele.is(":hidden")) {
                    ele.hide('slow');
                }
            });
        });
        $("#menu div.menu_title:eq(0)").trigger('click');
        $(".menu_sub_items li span").click(function () {
            var url = $(this).attr("url"),
                id = $(this).attr("id"),
                text = $(this).text();
            self.addTab({ url: url, id: id, name: text });
        });
    }

    Page.prototype.bind = function () {
        var $this = this;
        $.each($this.setting.event, function (propertyName, fn) {
            var selector = '#' + propertyName;
            $(selector).click(function () {
                $this.setting.event[propertyName].call($this);
            });
        });
    }

    Page.prototype.loadTab = function () {
        var $this = this;
        var url = util.pending + $this.user.ID;
        $this.addTab({
            id: 'smf-home',
            name: '<i class="layui-icon"></i>&nbsp;首页',
            url: url
        });
        layui.element.tabChange('smf-tabs', 'ztt-home');
    }

    Page.prototype.addTab = function (o) {
        var element = layui.element;
        var selector = "li[lay-id=" + o.id + "]";
        var total = $("li[lay-id]").length;
        if (total < 10) {
            if ($(selector).length == 0) {
                var template = $('#iframe-template-content').html();
                // var contact = o.Url.lastIndexOf('?') == -1 ? '?' : '&';
                // var url = o.Url + contact + 'categoryId=' + o.ID;
                var url = o.url;
                var content = template.replace(/{{url}}/igm, url);
                element.tabAdd('smf-tabs', {
                    title: o.name
                    , content: content
                    , id: o.id
                });
            }
            element.tabChange('smf-tabs', o.id);
        } 
    }

    Page.prototype.delTab = function (id) {
        var element = layui.element;
        element.tabDelete('smf-tabs', id);
    }

    initialize(function (option) {
        return new Page(option);
    });

})(function (initialize) {
    initialize({
        event: {
            logout: function () {
                util.cleanCache();
                window.top.location.href = '/pages/login.html';
            },
            refresh: function () {
                util.refreshTabPage();
            }
        }
    });
})