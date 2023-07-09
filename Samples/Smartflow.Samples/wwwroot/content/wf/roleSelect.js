; (function (initialize) {

    function Transfer(option) {
        this.option = option;
    }

    Transfer.prototype.load = function (nx) {
        var $opt = this.option;
        var ajaxSettings = { url: $opt.url, type: 'get' };
        ajaxSettings.data = ajaxSettings.data || {};

        ajaxSettings.success = function (serverData) {

            var leftDataSource = [], rightDataSource = [];

            $.each(serverData, function () {
                leftDataSource.push({
                    value: this.Id,
                    title: this.Name,
                    disabled: '',
                    checked: ''
                });
            });

            $.each(nx.group, function () {
                rightDataSource.push(this.id);
            });

            var transfer = layui.transfer;

            //基础效果
            transfer.render({
                elem: $opt.el
                , title: ['待选择', '已选择']
                , data: leftDataSource
                , value: rightDataSource
                , height: 530
                , width: 391
                , id: $opt.right
            });
        };

        util.ajaxWFService(ajaxSettings);
    }

    Transfer.prototype.set = function (nx) {
        var transfer = layui.transfer,
            rightData = transfer.getData('rightGroup');

        var roleArray = [];

        $(rightData).each(function () {
            var self = this;
            roleArray.push({
                id: self.value,
                name: self.title
            });
        });

        nx.group = roleArray;
    }

    initialize(function (option) {
        return new Transfer(option);
    });

})(function (createInstance) {
    window.setting = createInstance({
        url:'api/setting/group/list',
        el: '#transfer',
        right:'rightGroup'
    });
});