<template> 
  <div class='body'>
    <el-row class="toolbar">
      <el-col class="condition"><slot name="condition"></slot></el-col>
      <el-col class="button"><slot name="toolbar"></slot></el-col>
    </el-row>
    <el-row class="content">
    <el-col class='left'>
      <slot name="tree"></slot>      
    </el-col>
    <el-col class='right'>
        <slot name="table"></slot>      
    </el-col>
  </el-row>
  <slot name="form"></slot>      
  </div>
</template>

<style  scoped lang="scss">
.body {
  padding: 10px;
  height: 100%;
  background: #fff;
  /deep/ .cust-el-dialog {
    width: 650px;
    height: 400px;
  }
  .button {
    text-align: right;
  }
  .content {
    height: calc(100% - 40px);
    .left {
      width: 200px;
      height: 100%;
      padding-right: 5px;
    }
    .right {
      height: 100%;
      width: calc(100% - 200px);
    }
  }
}
</style>