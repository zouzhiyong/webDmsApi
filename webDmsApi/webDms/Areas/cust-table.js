define(['Components/component-table.js', 'Components/component-form.js', 'Components/component-condition.js', 'Components/component-dialog.js'], function (tree, table, form) {
    Vue.component('cust-table', {
        data: function () {
            return {
                tableCondition: {},
                table: {},
                form: {},
                condition:{},
                formCondition: {},
                dialogFormVisible: false
            }
        },
        props: {
            title: { type: String },
            control: { type: Array }
        },
        created: function () {
            this.iniData();

        },
        mounted: function () {
            this.$refs.condition.handleClick();
        },
        methods: {
            iniData: function () {
                var _self = this;
                this.control.map(function (item) {
                    if (item.ControlName === 'component-table') {
                        _self.table = item;
                    }

                    if (item.ControlName === 'component-form') {
                        _self.form = item;
                    }

                    if (item.ControlName === 'component-condition') {
                        _self.condition = item;
                    }
                })
            },
            handleRowClick: function (row) {
                this.formCondition = row;
                this.dialogFormVisible = true;
            },
            handleSearch: function (data) {
                this.tableCondition = JSON.parse(JSON.stringify(data));
            }
        },
        render: function (_c) {
            var _self = this;

            return _c('el-row', { staticStyle: { height: '100%' } }, [
                _c('el-row', { staticClass: 'content-title' }, [
                    _c('el-col', [
                        _c('div', [
                            _c('span', _self.title)
                        ])
                    ])
                ]),                
                _c('el-row', { staticClass: 'content-outbox' }, [
                    _c('el-col', { staticStyle: { height: '100%', width: '100%' } }, [
                        _c('div', { staticClass: 'content-condition' }, [
                            _c('component-condition', { attrs: { control: _self.condition }, ref: 'condition', on: { 'click': _self.handleSearch} }),
                        ]),
                        _c('div', { staticStyle: { height: 'calc(100% - 50px)' } }, [
                            _c('component-table', { attrs: { control: _self.table, condition: _self.tableCondition }, on: { 'edit': _self.handleRowClick } })
                        ])
                    ])
                ]),
                _c('component-dialog', { attrs: { control: _self.form, condition: _self.formCondition, "title": _self.title }, model: { value: (_self.dialogFormVisible), callback: function ($$v) { _self.dialogFormVisible = $$v }, expression: '_self.dialogFormVisible' } })
            ])
        }
    })
})