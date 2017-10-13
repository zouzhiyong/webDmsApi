Vue.component('component-tree', {
    data: function() {
        return {
            data: {}
        }
    },
    props: {
        control: { type: Object }
    },
    created: function() {
        var _self = this;

        this.GetData();
    },
    methods: {
        GetData: function() {
            var _self = this;
            ajaxData(_self.control.FindUrl, { async: false })
                .then(function(result) {
                    if (result) {
                        _self.data = result.data.tree;
                        _self.handleNodeClick(_self.data[0]);
                    }
                });
        },
        handleNodeClick: function(data) {
            this.$emit('node-click', data);
        }
    },
    render: function(_c) {
        var _self = this;
        return _c('el-tree',
            {
                ref: 'tree', attrs: {
                    data: _self.data,
                    props: { children: 'children', label: 'label' }, 'expand-on-click-node': false, 'default-expanded-keys': [0], 'node-key': 'MenuID', 'highlight-current': true, 'default-checked-keys': [0]
                }, staticStyle: { height: '100%', 'overflow-y': 'auto' }, on: { 'node-click': _self.handleNodeClick }
            }
        );
    }
})