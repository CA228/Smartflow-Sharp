var baseMixinPendingComponent = {
    props: {
        nodes:()=>[],
        category: Object
    },
    methods: {
        refreshParent: function () {
            this.$emit('refresh');
            this.init();
        }
    }
}