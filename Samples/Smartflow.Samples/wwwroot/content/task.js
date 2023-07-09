(function (factory) {
    factory({
        el: "#body",
        data: function () {
            return {
                nodes:[],
                categories: []
            }
        },
        created: function () {
            this.init();
        },
        mounted: function () {
            const $this = this;
            this.$nextTick(function () {
                $this.init();
            });
        },
        methods: {
            init: function () {
                var $this = this;
                const user = util.getUser();
                let url = util.buildArgs("api/smf/report/{userId}/list", { userId: user.ID });
                util.ajaxWFService({
                    type: 'get',
                    url: url,
                    success: function (serverData) {
                        $this.nodes = serverData.Dict;
                        $this.categories = serverData.Instances;
                        $this.refreshChildren();
                    }
                });
            },
            reload: function () {
                this.refreshChildren();
            },
            refreshChildren: function () {
                var that = this;
                this.categories.forEach(s => {
                    var arr = that.$refs[s.code];
                    if (that.$refs[s.code]&&arr.length > 0) {
                        arr.forEach(e => {
                            e.init();
                        });
                    }
                });
            }
        }
    });
})(function (params) {
    return new Vue(params);
});

