(function () {

    function Main(option) {
        this.settings = $.extend({}, option);
        this._initEvent();
    }

    Main.prototype._initEvent = function () {

        var $this = this;

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

        //默认展开节点
        $("#menu div.menu_title:eq(0)").trigger('click');

        $(".menu_sub_items li span").click(function () {
            var url = $(this).attr("url"),
                text = $(this).text();
            $($this.settings.iframeContent).attr("src", url);
        });

        $("#smartflow_menu_items li i").click(function () {
            var command = $(this).attr("command");
            switch (command) {
                case "exit":
                    window.localStorage.clear();
                    window.top.location.href = $this.settings.login;
                    break;
                case "pending":
                    var user = util.getUser();
                    var url = util.pending + user.ID;
                    document.getElementById("ifrmContent").setAttribute('src', url);
                    break;
                default:
                    break;
            }
        });

        $("#smartflow_menu_items li i[command=pending]").trigger('click');
    }

    $.Main = function (option) {
        return new Main(option);
    }

})();
