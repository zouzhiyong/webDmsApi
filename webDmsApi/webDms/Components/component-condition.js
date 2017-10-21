Vue.component('component-condition', {
    data: function () {
        return {
            data: {}
        }
    },
    props: {
        control: { type: Object }
    },
    created: function () {
        var _self = this;
        _self.control.SubControls.map(function (item) {
            if (item.ListUrl != null && item.ListUrl != "") {
                ajaxData(item.ListUrl, {
                    async: false
                })
                .then(function (result) {
                    if (result.result) {
                        _self.data[item.Prop + 'List'] = result.data;
                    }
                });                
            }
            _self.data[item.Prop] = "";
        })
    },
    methods: {
        handleClick: function () {
            var _self = this;
            for (var field in _self.data) {
                if (field.length - field.indexOf("Arr") == 3) {
                    _self.data[field.replace("Arr", '')] = _self.data[field].toString();
                }
            }
            this.$emit('click',this.data)
        }
    },
    render: function (_c) {
        var _self = this;
        return _c('div', { staticStyle: { padding: '0' } }, [
            _c('el-form', {
                ref: 'condition',
                attrs: { 'label-width': '80px', inline: true, model: _self.data }
            }, [
                _self._l(_self.control.SubControls, function (item) {
                    var TempateControls = item.TempateControls === null ? '' : item.TempateControls.split("|");
                    var TempateIcons = item.TempateIcons === null ? '' : item.TempateIcons.split("|");
                    var TempateEvents = item.TempateEvents === null ? '' : item.TempateEvents.split("|");

                    return _c('el-form-item', { attrs: { label: item.Label, prop: item.Prop } }, [
                        _self._l(TempateControls, function (_item, index) {
                            if (_item === 'el-select') {
                                return _c(_item, { attrs: { placeholder: "请选择" }, model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop } },
                                    _self._l(_self.data[item.Prop + 'List'], function (_item_) {
                                        return _c('el-option', { attrs: { key: _item_.value, "label": _item_.label, "value": _item_.value } })
                                    }))
                            }
                            if (_item === 'el-cascader') {
                                var val = _self.data[item.Prop].toString().split(",");
                                return _c(_item, { attrs: { clearable: true, placeholder: "请选择", options: _self.data[item.Prop + 'List'], props: { value: 'RegionNo', children: 'children' } }, model: { value: (val), callback: function ($$v) { _self.data[item.Prop] = $$v.toString() }, expression: item.Prop }, on: { 'active-item-change': _self.handleItemChange } })
                            }
                            else {
                                return _c(_item, { model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop } })
                            }
                        })
                    ])
                }),
                _c('el-button', { attrs: { type: 'primary' }, on: {'click':_self.handleClick} }, [
                    _c('i', { staticClass: "el-icon-search" })
                ])
            ]),
            
        ])
    }
})