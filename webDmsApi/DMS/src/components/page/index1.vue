<template>
	<div class="index1">
		<!-- 条件查询 -->
		<el-row align="middle" type="flex">
			<el-col :span="10">
				<el-form>
					  <el-form-item label="请输入查询的姓名和地址" label-width="200px">
					    <el-input size="large" v-model="search"></el-input>
					  </el-form-item>
				</el-form>
			</el-col>
			<el-col :span="14">
				<el-button class="add_btn" @click="dialogFormVisible = true">添加</el-button>
			</el-col>
		</el-row>
		<div>
			<!-- 列表 -->
			<el-table :data="pageData" border style="width: 100%">
	            <el-table-column prop="date"  label="日期"  width="180"></el-table-column>
	            <el-table-column  prop="name"  label="姓名"  width="180"></el-table-column>
	            <el-table-column  prop="address"  label="地址"></el-table-column>
	            <el-table-column  operation="操作"  label="操作">
	                <template scope="scope">
	                    <el-button  size="small"  @click="modify(scope.$index, scope.row)">编辑 </el-button>
	                    <el-button  size="small"  type="danger"  @click="handleDelete(scope.$index, scope.row)">删除</el-button>
	                </template>
	            </el-table-column>
	        </el-table>

	        <!-- 分页 -->
	        <el-row :gutter="20">
			   <el-col :span="12" :offset="9">
				    <div class="block">
					  <el-pagination
					    layout="prev, pager, next"
					    :total="total"
					    :page-size="10"
					    :current-page="currentPager"
					    @size-change="handleSizeChange"
                        @current-change="handleCurrentChange"
					    >
					  </el-pagination>
					</div>
			   </el-col>
			</el-row>
	        
			<!-- 添加数据的弹框 -->
	        <el-dialog title="添加" v-model="dialogFormVisible">
	            <el-form :model="form">
	                <el-form-item label="日期" :label-width="formLabelWidth">
	                     <el-date-picker v-model="form.date" format="yyyy-MM-dd" type="date" placeholder="选择日期"></el-date-picker>

	                </el-form-item>
	                <el-form-item label="姓名" :label-width="formLabelWidth">
	                    <el-input v-model="form.name" auto-complete="off"></el-input>
	                </el-form-item>
	                <el-form-item label="地址" :label-width="formLabelWidth">
	                    <el-input v-model="form.address" auto-complete="off"></el-input>
	                </el-form-item>
	            </el-form>
	            <div slot="footer" class="dialog-footer">
	                <el-button @click="dialogFormVisible = false">取 消</el-button>
	                <el-button type="primary" @click="add">确 定</el-button>
	            </div>
	        </el-dialog>
		</div>

	</div>
</template>


<script>

	var Time; 	
    //实例
	export default{
		name:'index1',
		data() {
            return {
            	search:'', //搜索框的初始值
                form: {   //添加列表时,初始化数据
                    date: '',
                    name: '',
                    address: ''
                },
                tableData:[ //初始化列表数据
		            { 
		                date: '2016-05-02',
		                name: '彭大牛',
		                address: '上海市普陀 1518 弄',
		            },{ 
		                date: '2016-05-02',
		                name: '徒弟',
		                address: '伟',
		            }, {
		                date: '2016-05-04',
		                name: '可大神',
		                address: '上海市普陀区金沙江路 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '震男神',
		                address: '上海市普 1519 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '强女神',
		                address: '上海市普陀区金沙江路 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '王大浪2',
		                address: '44444444 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '王小小3',
		                address: '55555555 1519 弄',
		            }, {
		                date: '2016-05-01',
		                name: '王小小4',
		                address: '55555555 1519 弄',
		            }, { //初始化列表数据
		                date: '2016-05-02',
		                name: '马云1',
		                address: '上海 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '马化腾',
		                address: '上江路 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '李彦宏',
		                address: '上海市普陀区金沙江路 1519 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '李嘉诚',
		                address: '上海市普陀区金沙江路 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '陈珂珂',
		                address: '上海市普陀区金沙江路 1517 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '彭大牛',
		                address: '上海市普陀 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '可大神',
		                address: '上海市普陀区金沙江路 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '震男神',
		                address: '上海市普 1519 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '强女神',
		                address: '上海市普陀区金沙江路 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '王大浪2',
		                address: '44444444 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '王小小3',
		                address: '55555555 1519 弄',
		            }, {
		                date: '2016-05-01',
		                name: '王小小4',
		                address: '55555555 1519 弄',
		            }, { //初始化列表数据
		                date: '2016-05-02',
		                name: '马云1',
		                address: '上海 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '马化腾',
		                address: '上江路 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '李彦宏',
		                address: '上海市普陀区金沙江路 1519 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '李嘉诚',
		                address: '上海市普陀区金沙江路 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '陈珂珂',
		                address: '上海市普陀区金沙江路 1517 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '彭大牛',
		                address: '上海市普陀 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '可大神',
		                address: '上海市普陀区金沙江路 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '震男神',
		                address: '上海市普 1519 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '强女神',
		                address: '上海市普陀区金沙江路 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '王大浪2',
		                address: '44444444 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '王小小3',
		                address: '55555555 1519 弄',
		            }, {
		                date: '2016-05-01',
		                name: '王小小5',
		                address: '55555555 1519 弄',
		            }, { //初始化列表数据
		                date: '2016-05-02',
		                name: '马云1',
		                address: '上海 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '马化腾',
		                address: '上江路 1517 弄',
		            }, {
		                date: '2016-05-01',
		                name: '李彦宏',
		                address: '上海市普陀区金沙江路 1519 弄',
		            },
		            { //初始化列表数据
		                date: '2016-05-02',
		                name: '李嘉诚',
		                address: '上海市普陀区金沙江路 1518 弄',
		            }, {
		                date: '2016-05-04',
		                name: '陈珂珂',
		                address: '上海市普陀区金沙江路 1517 弄',
		            } 
		        ],
            	filterData:[],
            	pageData:[],
                formLabelWidth: '50px', //添加数据弹框label标签宽度
                dialogFormVisible: false, //控制添加数据的弹框显示关闭
                total:0,
                pageNum:1,
                currentPager:1,
                pageSize:10 
            }
        },
        methods: {  //方法
            //添加
            add(){
            	this.form.date = this.mosaicData(this.form.date.toLocaleDateString().replace(/\//g,'-'));
                this.tableData.unshift(Object.assign({},this.form));
                this.dialogFormVisible = false;
                this.handleCurrentChange(1);
                this.form = {   //添加列表时,初始化数据
                    date: '',
                    name: '',
                    address: ''
                }
            },
            //删除
            handleDelete(index, row){
                this.tableData.splice(index, 1);
            },
            //编辑
            modify(index, row) {
                var that = this;
                this.$prompt('修改姓名', '编辑', {
                    confirmButtonText: '保存',
                    cancelButtonText: '取消',
                    inputValue: row.name
                }).then(function ({value}) {//编辑保存
                    row.name = value;
                }).catch(function () {  //编辑取消
                    that.$message({
                        type: 'info',
                        message: '取消编辑'
                    });
                });
            },
            handleSizeChange(page){
                this.pageNum = page;
            },
            handleCurrentChange(page){
                var num = page * this.pageSize;
                this.currentPager = page;
        		this.total = this.filterData.length;
                this.pageData = this.filterData.slice(num-this.pageSize,num);
            },
            // 搜索
           	handlersearch(val){
           		clearInterval(Time);
        		var that = this;
        		var reg = new RegExp('['+val+']{2,}');
        		Time = setTimeout(function(){
        			if(val == '' || val.length == 1){
        				that.filterData = that.tableData;
        			}else{
		        		that.filterData = that.tableData.filter(function(item,i){
		        			return reg.test(item.name) || reg.test(item.address)
		        		});
	        		}
        		},500)
           	},
           	//判断月和日小于两位数，添加0
	        mosaicData(data){
	        	var date = data.split('-');
	        		date[1] = date[1].length == 1 ? '0' + date[1] : date[1]; 
	        		date[2] = date[2].length == 1 ? '0' + date[2] : date[2]; 
	        	return date.join('-');
	        }
           	
        },
        //监听数据
        watch:{
        	search:function(val){
        		this.handlersearch(val);
        	},
        	filterData(){
        		this.handleCurrentChange(this.currentPager);

        	},
        	tableData(){
        		this.handlersearch(this.search);
        	}

        },
        //初始化显示第一页数据
        created(){
        	this.filterData = this.tableData;
        	this.handleCurrentChange(1);

        }
	}



</script>



<style>

	.index1{
	}
	.box{
		width: 500px;
	}
	.el-form-item__label{
		line-height: 20px;
		font-size: 16px;
	}

	.add_btn {
        float: right;
    }

    .el-dialog--small {
        max-width: 500px;
    }

    .el-dialog__body {
        padding-bottom: 0;
    }
    .el-form-item{
    	margin-bottom: 0;
    }
    .el-row{
    	margin-bottom: 20px;
    }
    .el-pagination{
    	margin-top: 10px;
    }


</style>



