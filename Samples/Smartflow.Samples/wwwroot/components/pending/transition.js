; (function (factory) {
    var text = "";
    text += "<div class=\"button-sel\">";
    text += "<div class=\"button-select\"><div class=\"button-arrow layui-icon layui-icon-up\" @click=\"switchButton($event)\"></div></div>";
    text += "<ul class=\"button-list\">";
    text += "  <li class=\"button-item\" :key=\"itm.Id\" v-for=\"itm in trans\" @click=\"openAuditWindow(itm)\"><span>{{itm.Name}}</span></li>";
    text += "  <li class=\"button-item\" @click=\"details\"><span>查看详情</span></li>";
    text += "</ul></div >";
    factory(text);
})(function (template) {
    Vue.component('transition-group', {
        props: {
            id: String,
            url: String,
            nodes: () => [],
            task: Object
        },
        template: template,
        data: function () {
            return {
                visible: true,
                trans: []
            }
        },
        created: function () {
            const $this = this, code = $this.task.Code;
            for (let i = 0; i < $this.nodes.length; i++) {
                let n = $this.nodes[i];
                if (n.Id === code) {
                    $this.trans = n.Transitions;
                    break;
                }
            }
        },
        methods: {
            switchButton: function ($event) {
                const target = $event.target;
                var next = $(target).parent().next();
                var s = next.children().first();
                var cur = $(target);
                if (cur.hasClass('layui-icon-up')) {
                    cur.removeClass('layui-icon-up');
                    cur.addClass('layui-icon-down');
                    s.siblings().show();
                    next.css('z-index', 9999);
                } else {
                    cur.removeClass('layui-icon-down')
                    cur.addClass('layui-icon-up');
                    s.siblings().hide();
                    next.css('z-index', 888);
                }
            },
            openAuditWindow: function (item) {
                const $this = this;
                util.openLayer({ url: item.Url, width: 500, height: 360, title: item.Name }, {
                    code: $this.task.Code,
                    taskId: $this.task.Id,
                    instanceId: $this.task.InstanceId,
                    id: $this.id,
                    lineId: item.Id
                });
            },
            details: function () {
                const url = this.url + '?id=' + this.id + '&instanceId=' + this.task.InstanceId;
                window.top.document.getElementById("ifrmContent").setAttribute('src', url);
            }
        }
    });
});