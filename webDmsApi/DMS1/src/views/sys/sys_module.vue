<template>
  <div class='body'>
    <el-row>
    <el-col class='tree'>
      <el-tree :data="treeData" :props="defaultProps" node-key="id" :default-expanded-keys="[0]" @node-click="handleNodeClick"></el-tree>
    </el-col>
    <el-col class='table'>
<el-table
    :data="tableData"
    border
    height="100%">
    <el-table-column
      type="index"
      width="100"
      header-align="center"
      align="center">
    </el-table-column>
    <el-table-column
      prop="date"
      label="日期"
      width="180"
      header-align="center">
    </el-table-column>
    <el-table-column
      prop="name"
      label="姓名"
      width="180"
      header-align="center">
    </el-table-column>
    <el-table-column
      prop="address"
      label="地址"
      header-align="center">
    </el-table-column>
  </el-table>
  <el-pagination
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      :current-page.sync="currentPage1"
      :page-size="100"
      layout="slot,->,prev, pager, next"
      :total="1000">
      <span class="demonstration">显示总数</span>
    </el-pagination>
    </el-col>
  </el-row>
  </div>
</template>


<script>
import { FindSysModule } from "../../api/api";

export default {
  data() {
    return {
      treeData: [],

      defaultProps: {
        children: "children",
        label: "label"
      },
      currentPage1: 5,
      currentPage2: 5,
      currentPage3: 5,
      currentPage4: 4,
      tableData: [
        {
          date: "2016-05-02",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1518 弄"
        },
        {
          date: "2016-05-04",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1517 弄"
        },
        {
          date: "2016-05-01",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1519 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          address: "上海市普陀区金沙江路 1516 弄"
        }
      ]
    };
  },
  created() {
    var obj = [
      {
        id: 0,
        label: "所有模块",
        children: []
      }
    ];

    FindSysModule().then(res => {
      obj[0].children = res.data.data;
      this.treeData = obj;
    });
  },
  methods: {
    handleNodeClick(data) {
      console.log(data);
    },
    handleSizeChange(size) {},
    handleCurrentChange(currentPage) {}
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
        height: calc(100% - 35px);
      }

      .el-icon-arrow-left {
        &:before {
          content: "上一页";
        }
      }
      .el-icon-arrow-right {
        &:before {
          content: "下一页";
        }
      }
    }
  }
}
</style>
