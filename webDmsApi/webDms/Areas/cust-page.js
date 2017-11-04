Vue.component('cust-page', {
    data: function () {
        return {
            tableData: {
                header: [
                    { code: 'name', name: '姓名' },
                    { code: 'date', name: '日期' },                    
                    { code: 'address', name: '地址' },
                ],
                data: [{
                    date: '2016-05-02',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-04',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1517 弄'
                }, {
                    date: '2016-05-01',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1519 弄'
                }, {
                    date: '2016-05-03',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1516 弄'
                }]
            }
        }
    },
    methods: {
        handleClick: function () {
            $(this).tableExport({ type: 'xlsx', escape: 'false', jsondata: this.tableData });
        }
    },
    render: function (_c) {
        return _c('div', [
            _c('el-button', { on: { click: this.handleClick } }, [this._v('测试')]),
           
            //_c('el-table', { attrs: { id:'table',data: this.tableData }, staticStyle: { width: '100%' } }, [
            //    _c('el-table-column', { attrs: { prop: 'date', label: '日期', width: '180' } }),
            //    _c('el-table-column', { attrs: { prop: 'name', label: '姓名', width: '180' } }),
            //    _c('el-table-column', { attrs: { prop: 'address', label: '地址', width: '180' } }),
            //])
        ]);
    }
})