Vue.component('component-tree', {
    data: function () {
        return {
            data: {},
            isEdit: false,
            ID: "",
            Label:"",
        }
    },
    props: {
        control: { type: Object }
    },
    created: function () {
        var _self = this;

        this.GetData();
    },
    methods: {
        GetData: function () {
            var _self = this;
            ajaxData(_self.control.FindUrl, { async: false })
                .then(function (result) {
                    if (result) {
                        _self.data = result.data.tree;
                        _self.$emit('node-click', _self.data[0]);
                        _self.ID = result.data.ID;
                        _self.Label = result.data.Label;
                    }
                });
        },
        handleNodeClick: function (data) {
            this.$emit('node-click', data);
        },
        handleSave: function () {
            var _self = this;
            if (_self.isEdit == true) {
                ajaxData(_self.control.SaveUrl, {
                    async: false,
                    data: _self.data
                }).then(function (result) {
                    if (result.result) {
                        _self.GetData();//重新获取数据
                        _self.isEdit = false;
                    }
                });
            } else {
                _self.isEdit = true;
            }

        },
        handleAdd: function () {
        },
        handleDel: function (data) {            
            data[this.ID] = -1;
        },
        renderContent: function (_c, obj) {
            var _self = this;
            //如果ID等于-1不显示，状态标示为删除
            return _c('span', [
                _self.isEdit == false ? _c('span', [_self._v(obj.data.label)]) : _self._e(),
                _self.isEdit == true ? _c('span', [
                    _c('input', { staticStyle: { border: 'none', height: '30px', 'line-height': '36px', width: 'calc(100% - 90px)', background: '#f8f3daee' }, staticClass: 'el-input__inner', directives: [{ name: "model", rawName: "v-model", value: (obj.data[_self.Label]), expression: "obj.node.label" }], attrs: { "placeholder": "请输入内容" }, domProps: { "value": (obj.data[_self.Label]) }, on: { "input": function ($event) { if ($event.target.composing) return; obj.data[_self.Label] = $event.target.value; obj.data.label = $event.target.value } } }),
                _c('el-button', { attrs: { type: "text", icon: 'plus', size: "small" }, staticStyle: { width: '25px' }, on: { 'click': _self.handleAdd } }),
                _c('el-button', { attrs: { type: "text", icon: 'delete', size: "small" }, staticStyle: { width: '25px' }, on: { 'click': function () { _self.handleDel(obj.data) } } }),
                ]) : _self._e()
            ])
        }
    },
    render: function (_c) {
        var _self = this;
        return _c('div', { staticStyle: { height: '100%' } }, [
            _c('el-tree', {
                ref: 'tree', attrs: {
                    data: _self.data,
                    'render-content': _self.renderContent,
                    props: { children: 'children', label: 'label' }, 'expand-on-click-node': false, 'default-expanded-keys': [0], 'node-key': _self.ID, 'highlight-current': true, 'default-checked-keys': [0]
                }, staticStyle: { height: 'calc(100% - 35px)', 'overflow-y': 'auto' }, on: { 'node-click': _self.handleNodeClick }
            }),
            _c('div', { staticStyle: { height: '30px' } }, [
                _c('el-button', { attrs: { type: "text", size: 'small' }, staticStyle: { height: '30px', 'padding-left': '10px' }, on: { 'click': _self.handleSave } }, [_self.isEdit == false ? '编辑' : '完成编辑']),
            ])
        ])
    }
})