define(['Components/component-title.js'], function (title) {
    Vue.component('cust-config', {
        data: function () {
            return {
                menuData: [],
                tableData1: [],
                tableData2: [],
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
            //var _self = this;
            
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
                ajaxData("api/Config/FindRightTable1", {
                    async: false,
                    data: { TemplateID: index }
                }).then(function (result) {
                    if (result.result == true) {
                        _self.tableData1 = result.data;
                        _self.$nextTick(function () {
                            _self.$refs.table1.setCurrentRow(_self.tableData1[0]);
                        })
                    }
                });
            },
            handleCurrentChange: function (currentRow, oldCurrentRow) {
                if (currentRow == null) return;
                var _self = this;
                ajaxData("api/Config/FindRightTable2", {
                    async: false,
                    data: currentRow
                }).then(function (result) {
                    if (result.result == true) {
                        _self.tableData2 = result.data;
                    }
                });
            },
            //handleRowClick: function (row, event, column) {
            //    var _self = this;
            //    ajaxData("api/Config/FindRightTable2", {
            //        async: false,
            //        data: row
            //    }).then(function (result) {
            //        if (result.result == true) {
            //            _self.tableData2 = result.data;
            //        }
            //    });
            //}
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
                        _c('div', { staticStyle: { height: '100%' } }, [
                            _c('el-table', { ref: 'table1', attrs: { data: _self.tableData1, 'highlight-current-row': true, border: true, height: 250 }, on: { 'current-change': _self.handleCurrentChange } }, [
                                _c('el-table-column', { attrs: { type: 'index', width: '50', 'header-align': 'center' } }),
                                _c('el-table-column', { attrs: { property: 'TemplateName', 'header-align': 'center', label: '模板名称', width: '150' } }),
                                _c('el-table-column', { attrs: { property: 'ControlName', 'header-align': 'center', label: '组件名称', width: '150' } }),
                                _c('el-table-column', { attrs: { property: 'FindUrl', 'header-align': 'center', label: '表格查询地址', width: '250' } }),
                                _c('el-table-column', { attrs: { property: 'FormUrl', 'header-align': 'center', label: '表单数据获取地址', width: '250' } }),
                                _c('el-table-column', { attrs: { property: 'DeleteUrl', 'header-align': 'center', label: '删除记录地址', width: '250' } }),
                                _c('el-table-column', { attrs: { property: 'SaveUrl', 'header-align': 'center', label: '保存记录地址', width: '250' } }),
                                _c('el-table-column', { attrs: { property: 'Describe', 'header-align': 'center', label: '描述', width: '' } })
                            ]),
                            _c('el-table', { ref: 'table2', attrs: { data: _self.tableData2, 'highlight-current-row': true, border: true, height: 300 }, on: { 'row-click': _self.handleRowClick } }, [
                                _c('el-table-column', { attrs: { type: 'index', width: '50', 'header-align': 'center' } }),
                                _c('el-table-column', { attrs: { property: 'Type', 'header-align': 'center', label: '类型', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'Prop', 'header-align': 'center', label: '字段名', width: '150' } }),
                                _c('el-table-column', { attrs: { property: 'ListUrl', 'header-align': 'center', label: '下拉地址', width: '250' } }),
                                _c('el-table-column', { attrs: { property: 'Rules', 'header-align': 'center', label: '规则', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'Label', 'header-align': 'center', label: '字段名称', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'Width', 'header-align': 'center', label: '宽', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'Align', 'header-align': 'center', label: '对齐', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'IsMust', 'header-align': 'center', label: '是否必填', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'TempateControls', 'header-align': 'center', label: '模板控件', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'TempateIcons', 'header-align': 'center', label: '模板图标', width: '100' } }),
                                _c('el-table-column', { attrs: { property: 'TempateEvents', 'header-align': 'center', label: '模板事件', width: '150' } }),
                                _c('el-table-column', { attrs: { property: 'Describe', 'header-align': 'center', label: '描述', width: '' } })
                            ])
                        ])
                    ])
                ]),
                //_self.dialogFormVisible ? _c('component-dialog', { attrs: { control: _self.form, condition: _self.formCondition, "title": _self.title }, model: { value: (_self.dialogFormVisible), callback: function ($$v) { _self.dialogFormVisible = $$v }, expression: '_self.dialogFormVisible' } }) : _self._e()
            ])
        }
    })
})