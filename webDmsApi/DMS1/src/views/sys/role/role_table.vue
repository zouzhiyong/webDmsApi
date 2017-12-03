<template>
  <div style="height:100%">
    <el-table :data="tableData" ref="table" border height="100%">
      <el-table-column type="index" width="100" header-align="center" align="center">
      </el-table-column>
      <el-table-column label="一级模块" width="180" header-align="center">
        <template slot-scope="scope">
        <i :class="scope.row.MenuIcon"></i>
        <el-checkbox :indeterminate="true" v-model="scope.row.MenuName">{{scope.row.MenuName}}</el-checkbox>
      </template>
      </el-table-column>
      <el-table-column label="二级模块" width="180" header-align="center">
        <template slot-scope="scope">
        <el-checkbox-group v-model="scope.row.chilDren">
            <el-checkbox v-for="item in scope.row.chilDren" :label="item.MenuName" :key="item.MenuID">{{item.MenuName}}</el-checkbox>
        </el-checkbox-group>
      </template>
      </el-table-column>
      <el-table-column label="说明" prop="Comment" header-align="center">      
      </el-table-column>
    </el-table>   
  </div>
</template>

<script>
import { FindSysMenuTable, DeleteSysMoudleRow } from "../../../api/api";
export default {
  data() {
    return {
      tableData: [],
      conditionData: {}
    };
  },
  methods: {
    GetData() {
      FindSysMenuTable().then(result => {
        this.tableData = result.data;
      });
    }
  }
};
</script>
<style scoped lang="scss">
.el-table {
  height: 100%;
}
</style>