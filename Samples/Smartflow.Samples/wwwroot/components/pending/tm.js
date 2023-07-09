; (function (factory) {
    var text = "<div class=\"list-box\"><div class=\"list-item\" v-for=\"itm in data\" >";
    text += "      <div class=\"layui-row\">";
    text += "       <div class=\"layui-col-sm2\">";
    text += "           <div class=\"pending-label\">业务类型</div>";
    text += "           <div class=\"pending-block\">请假流程</div>";
    text += "        </div>";
    text += "        <div class=\"layui-col-sm2\">";
    text += "             <div class=\"pending-label\">填写人</div>";
    text += "             <div class=\"pending-block\">{{itm.Data.Name}}</div>";
    text += "         </div>";
    text += "         <div class=\"layui-col-sm3\">";
    text += "              <div class=\"pending-label\">填写时间</div>";
    text += "              <div class=\"pending-block date-font\">{{itm.Data.CreateTime|formatShortDate}}</div>";
    text += "          </div>";
    text += "          <div class=\"layui-col-sm2\">";
    text += "              <div class=\"pending-label\">请假天数</div>";
    text += "              <div class=\"pending-block\">{{itm.Data.Day}}</div>";
    text += "          </div>";
    text += "          <div class=\"layui-col-sm3 ar\">";
    text += "              <transition-group :key=\"itm.Data.NID\" v-on:refreshParent=\"refreshParent\" :id=\"itm.Data.NID\" :task=\"itm.Task\" :nodes=\"nodes[itm.Data.TemplateId]\" :url=\"url\"></transition-group>";
    text += "          </div>";
    text += "        </div>";
    text += "       <div class=\"layui-form-item\">";
    text += "         <div class=\"pending-label\">请假事由</div>";
    text += "         <div class=\"pending-block\">{{itm.Data.Reason}}</div>";
    text += "       </div>";
    text += "  </div></div>";

    factory(text);

})(function (template) {
    Vue.component('pending-vacation', {
        template: template,
        mixins: [baseMixinPendingComponent],
        data: function () {
            return {
                url: '/pages/vacation/detail.html',
                nl:[],
                data: []
            };
        },
        created: function () {
            this.init();
        },
        methods: {
            init: function () {
                const $this = this;
                const user = util.getUser();
                let url = util.buildArgs("api/pending/{userId}/vacation/list", { userId: user.ID });
                util.ajaxService({
                    type: 'get',
                    url: url,
                    success: function (serverData) {
                        $this.data = serverData;
                    }
                });
            }
        }
    });
});
