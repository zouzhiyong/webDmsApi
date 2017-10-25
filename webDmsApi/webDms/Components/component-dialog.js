Vue.component('component-dialog', {
    data: function () {
        return {
            size: 'cust',
        }
    },
    props: {
        title: { type: String },
        value: { type: Boolean },
        control: { type: Object },
        condition: { type: Object },
    },
    mounted: function () {

    },
    methods: {
        handleSaveForm: function () {           
            var _self = this;

            _self.$refs.form.$refs.form.validate(function (valid) {
                if (valid) {
                    var data = JSON.parse(JSON.stringify(_self.$refs.form.data));
                    ajaxData(_self.control.SaveUrl, {
                        async: false,
                        data: data
                    }).then(function (result) {
                        if (result.result == true) {
                            _self.$emit('input', false);
                            var obj = JSON.parse(JSON.stringify(_self.$parent.$parent.tableCondition));
                            obj.currentPage = 1;
                            _self.$parent.$parent.tableCondition = obj;
                        }
                    });
                } else {

                    return false;
                }
            });


            
        },
        handleCancleForm: function () {
            this.$emit('input', false);            
        },
        handleCloseForm: function () {
            this.$emit('input', false);
        }
    },
    render: function (_c) {
        var _self = this;
        return _c('el-dialog', { ref: 'dialog', attrs: { "title": _self.title, size: _self.size, "visible": _self.value, 'close-on-click-modal': false, 'close-on-press-escape': false, 'show-close': false }, on: { close: _self.handleCloseForm, "update:visible": function ($event) { _self.value = $event } } }, [
            _c('el-row', { slot: "title", staticStyle: { margin: '10px' } }, [
                _c('el-col', { attrs: { span: '12' }, staticStyle: { 'text-align': 'left', 'font-weight': 'bold' } }, [_self._v(_self.title)]),
                _c('el-col', { attrs: { span: '12' }, staticStyle: { 'text-align': 'right' } }, [
                    _c('button', { attrs: { type: 'button' }, staticClass: 'el-dialog__headerbtn', staticStyle: { margin: 'auto 10px' }, on: { "click": _self.handleCloseForm } }, [
                        _c('i', { staticClass: 'el-dialog__close el-icon-close' })
                    ]),
                    _c('button', { attrs: { type: 'button' }, staticClass: 'el-dialog__headerbtn', staticStyle: { margin: 'auto 10px' }, on: { "click": function ($event) { if (_self.size != 'full') { _self.size = 'full' } else { _self.size = 'cust' } } } }, [
                        _c('i', { staticClass: 'el-dialog__close ' + (_self.size == 'full' ? 'fa fa-window-restore' : 'fa fa-window-maximize') })
                    ])
                ])
            ]),
            //_self._t("form"),
            _c('component-form', { attrs: { control: _self.control, condition: _self.condition }, ref: 'form' }),
            _c('div', { staticClass: "dialog-footer", attrs: { "slot": "footer" }, slot: "footer" }, [
                _c('el-button', { on: { "click": _self.handleCancleForm } }, [_self._v("取 消")]),
                _c('el-button', { attrs: { "type": "primary" }, on: { "click": _self.handleSaveForm } }, [_self._v("保存")])
            ])
        ])
    }
})