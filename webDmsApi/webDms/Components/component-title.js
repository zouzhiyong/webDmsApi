Vue.component('component-title', {
    props: {
        title: { type: String },
        button: {
            type: Array,
            default: [{ icon: 'fa fa-file-text-o', text: '新建',on:'click' }]
        }
    },
    mounted: function () {

    },
    methods: {
    },
    render: function (_c) {
        var _self = this;

        return _c('el-row', { staticClass: 'content-title' }, [
                _c('el-col', { attrs: { span: '12' }, staticStyle: { 'font-weight': 'bold' } }, [
                    _c('div', [
                        _c('span', _self.title)
                    ])
                ]),
                _c('el-col', { attrs: { span: '12' } }, [
                    _c('div', { staticStyle: { "text-align": "right", "padding-right": "20px" } }, [
                        _self._l(_self.button, function (item) {
                            return _c('el-button', { attrs: { type: "text" }, on: { 'click': function ($event) { _self.$emit(item.on); } } }, [
                                _c('i', { staticClass: item.icon }),
                                _self._v(" " + item.text)
                            ])
                        })
                    ])
                ])
        ])
    }
})
