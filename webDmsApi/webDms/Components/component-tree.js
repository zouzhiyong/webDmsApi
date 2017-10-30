Vue.component('component-tree', {
    data: function () {
        return {
            data: {},
            radio:"1",
            isEdit: false,
            ID: "",
            ParentID:"",
            Label: "",
            newData: { ID: -1, Label: '' },
            idIndex:1000
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
                        //增加是否编辑标志
                        _self.data = result.data.tree;
                        _self.$emit('node-click', _self.data[0]);
                        _self.ID = result.data.ID || "ID";
                        _self.ParentID = result.data.ParentID || "ParentID";
                        _self.Label = result.data.Label;
                        _self.newData.Label = "";                        
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
        handleAdd: function (obj) {
            if (this.newData.Label == "") return;
            var objTemp = {};
            
            //获取新增数据
            objTemp[this.ID] = this.idIndex++;
            objTemp.Status = -1;//状态
            objTemp[this.Label] = this.newData.Label;
            objTemp.label = this.newData.Label;
            objTemp.isEdit = false;
            objTemp.children = [];

            //同级
            if (this.radio == "1") {
                objTemp[this.ParentID] = obj.node.parent.data[this.ID];
                obj.node.parent.data.children.push(objTemp);
            }
            //子级
            if (this.radio == "2") {
                if (!obj.data.children) {
                    this.$set(obj.data, 'children', []);
                }
                objTemp[this.ParentID] = obj.node.data[this.ID];
                obj.data.children.push(objTemp);
                obj.node.expanded = true;
            }
            obj.data.isEdit = false;
            
        },
        handleDel: function (obj) {
            var _self = this;
            var parent = obj.node.parent;
            var children = parent.data.children || parent.data;
            var index = children.findIndex(function (d) {
                return d[_self.ID] === obj.data[_self.ID];
            });
            children.splice(index, 1);
        },
        handleCancel: function () {
            this.isEdit = false;
        },
        handlePopoverShow: function () {
            this.radio = "1";
            this.newData.Label = "";
        },
        handlePopoverHide: function () {
            
        },
        renderContent: function (_c, obj) {
            var _self = this;
            
            //如果ID等于-1不显示，状态标示为删除
            return _c('span', [
                (obj.node.key == 0 || _self.isEdit == false) ? _c('span', { on: { click: function (event) { obj.node.data.isEdit = true; } } }, [_self._v(obj.node.data.label)]) : _self._e(),
                (obj.node.key != 0 && _self.isEdit==true) ? _c('span', [
                    _c('el-popover', { ref: "popover", attrs: { "placement": "right", "width": "160" }, model: { value: (obj.node.data.isEdit), callback: function ($$v) { obj.node.data.isEdit = $$v}, expression: "obj.node.data.isEdit" }, staticStyle: {}, on: { show: _self.handlePopoverShow, hide: _self.handlePopoverHide } }, [
                        _c('el-radio', { staticClass: "radio", attrs: { "label": "1" }, model: { value: (_self.radio), callback: function ($$v) { _self.radio = $$v }, expression: "_self.radio" } }, [_self._v("同级")]),
                        _c('el-radio', { staticClass: "radio", attrs: { "label": "2" }, model: { value: (_self.radio), callback: function ($$v) { _self.radio = $$v }, expression: "_self.radio" } }, [_self._v("子级")]),
                        _c('el-input', { attrs: { "placeholder": "请输入内容" }, model: { value: (_self.newData.Label), callback: function ($$v) { _self.newData.Label = $$v; }, expression: "_self.newData.Label" } }),
                        _c('el-button', { attrs: { "type": "primary", size: 'small' }, staticStyle: { 'margin-top': '5px' }, on: { click: function () { _self.handleAdd(obj) } } }, [_self._v("添加")])
                    ]),
                    _c('input', { staticStyle: { border: 'none', height: '30px', 'line-height': '36px', width: 'calc(100% - 90px)', border: '1px solid #eee' }, staticClass: 'el-input__inner', directives: [{ name: "model", rawName: "v-model", value: (obj.node.data[_self.Label]), expression: "obj.node.label" }], attrs: { "placeholder": "请输入内容" }, domProps: { "value": (obj.node.data[_self.Label]) }, on: { "input": function ($event) { if ($event.target.composing) return; obj.node.data[_self.Label] = $event.target.value; obj.node.data.label = $event.target.value } } }),
                _c('el-button', { attrs: { type: "text", icon: 'plus', size: "small" }, directives: [{ name: "popover", rawName: "v-popover:popover", arg: "popover" }], staticStyle: { width: '25px' }}),
                _c('el-button', { attrs: { type: "text", icon: 'delete', size: "small" }, staticStyle: { width: '25px' }, on: { 'click': function () { _self.handleDel(obj) } } }),
                ]) : _self._e(),
            ])
        },
    },
    render: function (_c) {
        var _self = this;
        return _c('div', { staticStyle: { height: '100%' } }, [
            _c('el-tree', {
                ref: 'tree', attrs: {
                    data: _self.data,
                    'render-content': _self.renderContent,
                    'filter-node-method':_self.filterNodeMethod,
                    props: { children: 'children', label: 'label' }, 'expand-on-click-node': false, 'default-expanded-keys': [0], 'node-key': _self.ID, 'highlight-current': true, 'default-checked-keys': [0]
                }, staticStyle: { height: 'calc(100% - 35px)', 'overflow-y': 'auto' }, on: { 'node-click': _self.handleNodeClick }
            }),
            _c('div', { staticStyle: { height: '30px', 'margin-top': '5px' } }, [
                _c('el-button', { attrs: { type: "", size: 'small' }, staticStyle: { 'padding': '7px 19px' }, on: { 'click': _self.handleSave } }, [_self.isEdit == false ? '编辑' : '完成编辑']),
                 _self.isEdit == true ? _c('el-button', { attrs: { type: "", size: 'small' }, staticStyle: { 'padding': '7px 19px' }, on: { 'click': _self.handleCancel } }, ['取消编辑']) : _self._e(),
            ]),
        ])
    }
})