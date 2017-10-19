Vue.component('component-form', {
    data: function () {
        return {
            data: {}
        }
    },
    props: {
        condition: { type: Object },
        control: { type: Object }
    },
    created: function () {
        this.GetData();
    },
    methods: {
        GetData: function () {
            var _self = this;
            ajaxData(_self.control.FormUrl, {
                async: false,
                data: _self.condition
            })
                .then(function (result) {
                    if (result.result) {
                        _self.data = result.data;
                        for (var field in _self.data) {
                            if (field.indexOf("Arr")>0 && field.length - field.indexOf("Arr") == 3) {
                                _self.data[field] = _self.data[field].toString().split(",");
                            }
                        }
                    }
                });
        }
    },
    //watch: {
    //    condition: function (newVal, oldVal) {
    //        this.GetData();
    //    }
    //},
    render: function (_c) {
        var _self = this;
        return _c('div', { staticStyle: { padding: '0' } }, [
            _c('el-form', {
                ref: 'form',
                attrs: { 'label-width': '80px', inline: true, model: _self.data }
            }, [
                _self._l(_self.control.SubControls, function (item) {
                    var TempateControls = item.TempateControls === null ? '' : item.TempateControls.split("|");
                    var TempateIcons = item.TempateIcons === null ? '' : item.TempateIcons.split("|");
                    var TempateEvents = item.TempateEvents === null ? '' : item.TempateEvents.split("|");
                    var Rules = [];
                    if (item.IsMust == 1) {
                        Rules.push({ required: true, message: '请输入' + item.Label, trigger: 'blur' });
                    }
                    
                    if (item.Rules != null && item.Rules != "") {
                        Rules.push(JSON.parse(item.Rules));
                    }

                    return _c('el-form-item', { attrs: { label: item.Label, prop: item.Prop, rules: Rules } }, [
                        _self._l(TempateControls, function (_item, index) {
                            if (_item === 'el-select') {
                                return _c(_item, { attrs: { placeholder: "请选择" }, model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop } },
                                    _self._l(_self.data[item.Prop + 'List'], function (_item_) {
                                        return _c('el-option', { attrs: { key: _item_.value, "label": _item_.label, "value": _item_.value } })
                                    }))
                            }
                            if (_item === 'el-cascader') {
                                return _c(_item, { attrs: { placeholder: "请选择", options: _self.data[item.Prop + 'List'], props: { value: 'RegionNo', children: 'children' } }, model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop }, on: { 'active-item-change': _self.handleItemChange } })
                            }
                            else {
                                return _c(_item, { model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop } })
                            }
                        })
                    ])
                })
            ])
        ])
    }
})