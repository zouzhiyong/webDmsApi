define(['Components/component-title.js'], function (title) {
    Vue.component('cust-config', {
        data: function () {
            return {
                menuData: [],
                controlData: {},
                errMsg: ''
            }
        },
        props: {
            title: { type: String }
        },
        created: function () {
            this.iniData();
        },
        mounted: function () {
            var _self = this;
            _self.$refs.textarea.$refs.textarea.style.height = '100%';

        },
        methods: {
            iniData: function () {
                var _self = this;
                ajaxData("api/Config/FindLeftMenu", {
                    async: false
                }).then(function (result) {
                    if (result.result == true) {
                        _self.menuData = result.data;
                        _self.handleSelect(_self.menuData[0].TemplateID);
                    }
                });
            },
            handleSelect: function (index) {
                var _self = this;
                _self.menuData.map(function (item) {
                    if (index == item.TemplateID) {
                        _self.controlData = JSON.stringify(JSON.parse(item.Controls), null, 4);
                    }
                })
                _self.handleFormat();
            },
            handleClick: function () {
                console.log(JSON.stringify(JSON.parse(this.controlData)))
            },
            handleFormat: function () {
                var _self = this;
                try {
                    _self.controlData = JSON.stringify(JSON.parse(_self.controlData), null, 4);
                    _self.errMsg = '';
                } catch (error) {
                    _self.errMsg = error.message;
                }
            }
        },
        render: function (_c) {
            var _self = this;

            return _c('el-row', { staticStyle: { height: '100%' } }, [
                _c('component-title', { attrs: { title: _self.title }, on: { 'click': _self.handleNewForm } }),
                _c('el-row', { staticClass: 'content-outbox' }, [
                    _c('el-col', { staticStyle: { height: '100%', width: '200px', 'padding-right': '5px' } }, [
                        _c('div', { staticStyle: { height: '100%' } }, [
                            _c('el-menu', { attrs: { 'default-active': _self.menuData[0].TemplateID }, staticStyle: { height: '100%', 'overflow-y': 'auto' }, on: { 'select': _self.handleSelect } }, [
                                _self._l(_self.menuData, function (item) {
                                    return _c('el-menu-item', { attrs: { index: item.TemplateID } }, [_self._v(item.Describe)])
                                })
                            ])
                        ])
                    ]),
                    _c('el-col', { staticStyle: { height: '100%', width: 'calc(100% - 200px)' } }, [                       
                        _c('div', { staticStyle: { height: _self.errMsg == '' ? 'calc(100% - 60px)' : 'calc(100% - 95px)' } }, [
                            _c('el-input', { ref: 'textarea', attrs: { type: 'textarea' }, staticStyle: { height: '100%' }, model: { value: (_self.controlData), callback: function ($$v) { _self.controlData = $$v }, expression: "_self.controlData" }, on: { change: _self.handleFormat } }),
                        ]),
                         _self.errMsg != '' ? _c('div', [
                            _c('el-alert', { attrs: { title: _self.errMsg, type: 'error', closable: false } }),
                         ]) : _self._e(),
                        _c('div', { staticStyle: { 'text-align': 'center', padding: '10px' } }, [
                            _c('el-button', { attrs: { type: 'primary' }, on: { click: _self.handleClick } }, '保存'),
                            _c('el-button', { attrs: { type: 'primary' }, on: { click: _self.handleFormat } }, '格式化')
                        ])
                    ])
                ]),
            ])
        }
    })
})