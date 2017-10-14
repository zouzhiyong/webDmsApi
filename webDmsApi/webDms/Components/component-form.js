Vue.component('component-form', {
    data: function () {
        return {
            //Attributes: {},
            ItemAttributes: [],
            data: {},
        }
    },
    props: {
        condition: { type: Object },
        control: { type: Object }
    },
    created: function () {
        //获取attrs        
        //this.ItemAttributes = eval('(' + this.control.ItemAttributes + ')');
        this.GetData();
    },
    methods: {
        GetData: function () {
            var _self = this;
            ajaxData(_self.control.FormUrl, {
                async: false,
                data: { MenuID: _self.condition.MenuID }
            })
                .then(function (result) {
                    if (result) {
                        _self.data = result.data[0];
                    }
                });
        },
    },
    watch: {
        condition: function (newVal, oldVal) {
            this.GetData();
        }
    },
    render: function (_c) {
        var _self = this;
        return _c('div', { staticStyle: { padding: '0' } }, [
            _c('el-form', {
                ref: 'form',
                attrs: { 'label-width': '80px', inline: true, model: _self.data }
            }, [
                _self._l(_self.control.SubControls, function (item) {
                    var TempateControls = item.TempateControls===null?'':item.TempateControls.split("|");
                    var TempateIcons = item.TempateIcons === null ? '' : item.TempateIcons.split("|");
                    var TempateEvents = item.TempateEvents === null ? '' : item.TempateEvents.split("|");
                    return _c('el-form-item', { attrs: { label: item.Label, prop: item.Prop } }, [
                        _self._l(TempateControls, function (_item,index) {
                            if (_item === 'el-select') {
                                return _c(_item, { attrs: { placeholder: "请选择" }, model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop } },
                                    _self._l(_self.data[item.Prop + 'List'], function (_item_) {
                                        return _c('el-option', { attrs: { key: _item_.value, "label": _item_.label, "value": _item_.value } })
                                    }))
                            } else {
                                return _c(_item, { model: { value: (_self.data[item.Prop]), callback: function ($$v) { _self.data[item.Prop] = $$v }, expression: item.Prop } })
                            }
                        })
                    ])
                })
            ])
        ])
    }
})