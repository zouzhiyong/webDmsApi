<template>
  <div style="height:100%">
    <el-table :data="tableData" ref="table" border height="100%">
      <el-table-column type="index" width="100" header-align="center" align="center">
        <template slot-scope="scope">
          {{scope.$index + 1 + (pageSize * (currentPage - 1))}}
        </template>
      </el-table-column>
      <el-table-column prop="MenuName" label="模块名称" width="180" header-align="center">
      </el-table-column>
      <el-table-column prop="MenuPath" label="模块路径" width="180" header-align="center">
      </el-table-column>
      <el-table-column prop="MenuIcon" label="图标" header-align="center">
      </el-table-column>
      <el-table-column prop="IsValid" label="有效否" align="center" :formatter="function(row, column){return row.IsValid==0?'无效':'有效'}" header-align="center">
      </el-table-column>
      <el-table-column label="操作" align="center" header-align="center">
        <template slot-scope="scope">
          <el-button type="text" icon="el-icon-edit" @click="handleEditClick(scope.row)"></el-button>
          <el-button type="text" icon="el-icon-delete" @click="handleDeleteClick(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination background prev-text="上一页" next-text="下一页" @current-change="handleCurrentChange" :current-page.sync="currentPage" :page-size="pageSize" layout="slot,->,prev, pager, next" :total="total">
      <span class="demonstration">显示第 {{pageSize * currentPage - pageSize + (total==0?0:1)}} 到第 {{((pageSize * currentPage) > total ? total : (pageSize * currentPage))}} 条记录，总共 {{total}} 条记录</span>
    </el-pagination>
  </div>
</template>

<script>
import { FindSysMoudleTable, DeleteSysMoudleRow } from "../../../api/api";
export default {
  data() {
    return {
      currentPage: 1,
      pageSize: 0,
      total: 0,
      tableData: [],
      conditionData: {}
    };
  },
  methods: {
    GetData() {
      var data = this.conditionData;
      this.pageSize = Math.floor(this.$refs.table.$el.clientHeight / 40);
      data.currentPage = this.currentPage;
      data.pageSize = this.pageSize;
      FindSysMoudleTable(data).then(result => {
        this.tableData = result.rows;
        this.total = result.total;
      });
    },
    handleCurrentChange(currentPage) {
      this.currentPage = currentPage;
      this.GetData();
    },
    handleEditClick(row) {
      this.$parent.$parent.$parent.$parent.$refs.form.GetData(row);
    },
    handleDeleteClick(row) {
      this.$confirm("是否确认删除?", "提示", {
        type: "warning"
      })
        .then(() => {
          DeleteSysMoudleRow(row).then(result => {
            this.GetData();
          });
        })
        .catch(() => {});
    }
  }
};
</script>
<style scoped lang="scss">
.el-table {
  height: calc(100% - 40px);
}

.el-pagination {
  margin-top: 8px;
}
</style>