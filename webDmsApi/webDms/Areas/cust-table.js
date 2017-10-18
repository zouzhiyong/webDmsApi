define(['Components/component-table.js', 'Components/component-form.js', 'Components/component-condition.js', 'Components/component-dialog.js','Components/component-title.js'], function (tree, table, form) {
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
            console.log(this.control);
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
            handleRowDelete: function (row) {
                var _self = this;

                this.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    ajaxData(_self.table.DeleteUrl, {
                        async: false,
                        data: row
                    }).then(function (result) {
                        if (result.result == true) {
                            var obj = JSON.parse(JSON.stringify(_self.tableCondition));
                            obj.currentPage = 1;
                            _self.tableCondition = obj;
                        }
                    });                    
                });
            },
            handleSearch: function (data) {
                this.tableCondition = JSON.parse(JSON.stringify(data));
            },
            handleNewForm: function () {
                this.formCondition = null;
                this.dialogFormVisible = true;
            },
        },
        render: function (_c) {
            var _self = this;

            return _c('el-row', { staticStyle: { height: '100%' } }, [
                _c('component-title', { attrs: { title: _self.title }, on: {'click':_self.handleNewForm} }),
                _c('el-row', { staticClass: 'content-outbox' }, [
                    _c('el-col', { staticStyle: { height: '100%', width: '100%' } }, [
                        _c('div', { staticClass: 'content-condition' }, [
                            _c('component-condition', { attrs: { control: _self.condition }, ref: 'condition', on: { 'click': _self.handleSearch} }),
                        ]),
                        _c('div', { staticStyle: { height: 'calc(100% - 50px)' } }, [
                            _c('component-table', { attrs: { control: _self.table, condition: _self.tableCondition }, on: { 'edit': _self.handleRowClick, 'delete': _self.handleRowDelete } })
                        ])
                    ])
                ]),
                _self.dialogFormVisible?_c('component-dialog', { attrs: { control: _self.form, condition: _self.formCondition, "title": _self.title }, model: { value: (_self.dialogFormVisible), callback: function ($$v) { _self.dialogFormVisible = $$v }, expression: '_self.dialogFormVisible' } }):_self._e()
            ])
        }
    })
})