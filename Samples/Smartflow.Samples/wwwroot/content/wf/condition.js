; (function (initialize) {

    function Condition(option) {
        this.option = option;
    }

    Condition.prototype.load = function (nx) {
        Condition.dynamicGenControl(nx);
    }

    Condition.prototype.set = function (nx) {
        Condition.dynamicGetControl(nx);
    }

    Condition.dynamicGenControl = function (nx) {
        var LC = nx.getTransitions();
        if (LC.length > 0) {
            var template = document.getElementById("common_expression").innerHTML,
                ele = [];
            $.each(LC, function (i) {
                ele.push(template.replace(/{{name}}/, this.name)
                    .replace(/{{expression}}/, this.expression)
                    .replace(/{{id}}/, this.$id)
                );
            });
            $("#form_expression").html(ele.join(''));
            layui.form.render(null, 'form_expression');
        }
    }

    Condition.dynamicGetControl = function (nx) {
        var controls = $("#form_expression").find("textarea");
        $.each(controls, function () {
            var input = $(this);
            nx.set({
                id: input.attr("name"),
                expression: input.val()
                    .replace(/\r\n/g, ' ')
                    .replace(/\n/g, ' ')
                    .replace(/\s/g, ' ')
            });
        });
    }

    initialize(function (option) {
        return new Condition(option);
    });

})(function (initialize) {
    window.setting = initialize();
});