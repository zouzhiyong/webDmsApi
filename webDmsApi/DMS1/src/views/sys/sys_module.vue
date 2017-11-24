<template> 
  <div class='body'>
    <el-row>
    <el-col class='tree'>
      <el-tree :data="treeData" :props="defaultProps" node-key="id" :current-node-key="0" :highlight-current=true :default-expanded-keys="[0]" @node-click="handleNodeClick"></el-tree>
    </el-col>
    <el-col class='table'>
<el-table
    :data="tableData"
    ref="table"
    border
    height="100%">
    <el-table-column
      type="index"
      width="100"      
      header-align="center"
      align="center">
      <template scope="scope">
        {{scope.$index + 1 + (pageSize * (currentPage - 1))}}
      </template>
    </el-table-column>
    <el-table-column
      prop="MenuName"
      label="模块名称"
      width="180"
      header-align="center">
    </el-table-column>
    <el-table-column
      prop="MenuPath"
      label="模块路径"
      width="180"
      header-align="center">
    </el-table-column>
    <el-table-column
      prop="MenuIcon"
      label="图标"
      header-align="center">
    </el-table-column>
    <el-table-column
      prop="IsValid"
      label="有效否"
      align="center"
      :formatter="function(row, column){return row.IsValid==1?'有效':'无效'}"
      header-align="center">
    </el-table-column>    
    <el-table-column
      label="操作"
      align="center"
      header-align="center">
      <template scope="scope">
        <el-button type="text" icon="edit" @click="handleEditClick(scope.row)"></el-button>
        <el-button type="text" icon="delete"></el-button>
      </template>
    </el-table-column>
  </el-table>
  <el-pagination
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      :current-page.sync="currentPage"
      :page-size="pageSize"
      layout="slot,->,prev, pager, next"
      :total="total">
      <span class="demonstration">显示第 {{pageSize * currentPage - pageSize + (total==0?0:1)}} 到第 {{((pageSize * currentPage) > total ? total : (pageSize * currentPage))}} 条记录，总共 {{total}} 条记录</span>
    </el-pagination>
    </el-col>
  </el-row>
  </div>
</template>

<script>
import { FindSysModule, FindSysMoudleRow } from "../../api/api";

export default {
  data() {
    return {
      treeData: [],

      defaultProps: {
        children: "children",
        label: "label"
      },
      currentPage: 1,
      pageSize: 0,
      total: 0,
      conditionData: {},
      tableData: []
    };
  },
  created() {
    var obj = [
      {
        id: 0,
        MenuID: 0,
        label: "所有模块",
        children: []
      }
    ];

    FindSysModule().then(res => {
      obj[0].children = res.data.data;
      this.treeData = obj;
      if (this.treeData.length > 0) {
        this.handleNodeClick(this.treeData[0]);
      }
    });
  },
  mounted() {},
  methods: {
    GetData() {
      var data = this.conditionData;
      this.pageSize = Math.floor(this.$refs.table.$el.clientHeight / 40);
      data.currentPage = this.currentPage;
      data.pageSize = this.pageSize;
      FindSysMoudleRow(data).then(res => {
        this.tableData = res.data.rows;
        this.total = res.data.total;
      });
    },
    handleNodeClick(data) {
      this.conditionData = data;
      this.GetData();
    },
    handleSizeChange(size) {},
    handleCurrentChange(currentPage) {
      this.currentPage = currentPage;
      this.GetData();
    },
    handleEditClick() {
      layui.use(["layer"], function() {
        var layer = layui.layer;

        var index = layer.open({
          area: ["500px", "300px"],
          title: "模块编辑",
          content: "<div>123</div>"
        });
      });
    }
  }
};
</script>

<style  scoped lang="scss">
.body {
  padding: 10px;
  height: 100%;
  background: #fff;
  .el-row {
    height: 100%;
    .tree {
      width: 200px;
      height: 100%;
      padding-right: 5px;
      .el-tree {
        height: 100%;
        overflow-y: auto;
      }
    }
    .table {
      height: 100%;
      width: calc(100% - 200px);
      .el-table {
        height: calc(100% - 40px);
      }
      .el-pagination {
        margin-top: 8px;
      }
    }
  }
}
</style>

