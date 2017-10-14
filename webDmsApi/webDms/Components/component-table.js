Vue.component('component-table', {
    data: function () {
        return {
            height: 0,
            tableData: ""
        };
    },
    props: {
        condition: { type: Object },
        control: { type: Object }
    },
    created: function () {

    },
    mounted: function () {
        var height = $(this.$el).height() - 60;
        this.height = height;
        this.condition.currentPage = 1;
        this.condition.pageSize = Math.floor(this.height / 40);
    },
    methods: {
        GetData: function (condition) {
            var _self = this;
            ajaxData(_self.control.FindUrl, {
                async: false,
                data: condition
            })
                .then(function (result) {
                    if (result) {
                        _self.tableData = result;
                    }
                });
        },
        handleRowClick: function (row) {
            this.$emit('edit', row);
        },
        handleSizeChange: function (size) {

        },
        handleCurrentChange: function (currentPage) {
            this.condition.currentPage = currentPage;
            this.GetData(this.condition);
        }
    },
    watch: {
        condition: {
            handler: function (newVal, oldVal) {
                if (newVal.currentPage && newVal.pageSize) {
                    this.GetData(newVal);
                } else {
                    this.condition.currentPage = 1;
                    this.condition.pageSize = Math.floor(this.height / 40);
                    this.GetData(this.condition);
                }
            },
            deep: true
        }
    },
    render: function (_c) {
        var _self = this;

        return _c('div', { staticStyle: { height: "100%" } }, [
            _c('el-table', { attrs: { data: _self.tableData.rows, border: true, height: '100%' }, staticStyle: { width: '100%', height: 'calc(100% - 35px)' } }, [
                _self._l(_self.control.SubControls, function (item) {
                     return _c('el-table-column', {
                         attrs: { type: item.Type, label: item.Label, width: item.Width, align: item.Align, prop: item.Prop, 'header-align': 'center' },
                        staticStyle: {},
                        staticClass: '',
                        //作用域插槽的模板，重点**************
                        scopedSlots: (item.TempateControls !== null && (item.Type === 'template' || item.Type === "index")) ? ({
                            default: function (scope) {
                                var TempateControls = item.TempateControls === null ? '' : item.TempateControls.split("|");
                                var TempateIcons = item.TempateIcons === null ? '' : item.TempateIcons.split("|");
                                var TempateEvents = item.TempateEvents === null ? '' : item.TempateEvents.split("|");
                                
                                if (item.Type === "index") {
                                    return _self._v((scope.$index + 1 + (_self.tableData.pageSize * (_self.tableData.currentPage - 1))));
                                } else {
                                    return _c('div', [
                                        _self._l(TempateControls, function (_item, index) {                                            
                                            return _c(_item, {
                                                attrs: { type: 'text' },
                                                staticStyle: { width: '30px' },
                                                nativeOn: {
                                                    click: function ($event) {
                                                        $event.preventDefault();
                                                        var event = '_self.' + TempateEvents[index];
                                                        eval('(' + event + ')');
                                                    }
                                                }
                                            }, [
                                                _c('i', { staticClass: TempateIcons[index] })
                                            ]);
                                        })
                                    ]);
                                }
                            }
                        }) : (_self._e())
                    });
                })
            ]),
            _c('el-pagination', {
                ref: 'pagination',
                attrs: {
                    'page-size': _self.tableData.pageSize,
                    layout: " slot,->,prev, pager, next",
                    total: _self.tableData.total,
                    "current-page": _self.tableData.currentPage
                },
                staticStyle: { padding: '7px 8px' },
                on: { 'pageNumber': _self.handleSizeChange, 'current-change': _self.handleCurrentChange }
            }, [
                _c('span', '显示第 ' + (_self.tableData.pageSize * _self.tableData.currentPage - _self.tableData.pageSize + 1) + ' 到第 ' + ((_self.tableData.pageSize * _self.tableData.currentPage) > _self.tableData.total ? _self.tableData.total : (_self.tableData.pageSize * _self.tableData.currentPage)) + ' 条记录，总共 ' + _self.tableData.total + ' 条记录')
            ])
        ]);

    }
});