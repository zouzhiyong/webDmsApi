<template>
  <el-form :inline="true" ref="ruleForm" :model="value" label-width='80px' class="demo-form-inline">
  <el-form-item label="模块名称" prop="MenuName" :rules="[{required:true, message: '模块名称不能为空'}]">
    <el-input v-model="value.MenuName" placeholder="模块名称"></el-input>
  </el-form-item>
  <el-form-item label="上级模块" prop="MenuParentID">
    <el-select v-model="value.MenuParentID" placeholder="上级模块">
      <el-option
      v-for="item in value.MenuParentIDList"
      :key="item.value"
      :label="item.label"
      :value="item.value">
    </el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="模块地址" prop="MenuPath">
    <el-input v-model="value.MenuPath" placeholder="模块地址"></el-input>
  </el-form-item>
  <el-form-item label="图标" prop="MenuIcon">
    <el-input v-model="value.MenuIcon" placeholder="图标"></el-input>
  </el-form-item>
  <el-form-item label="有效否" prop="IsValid">
    <el-select v-model="value.IsValid" placeholder="有效否">
      <el-option
      v-for="item in value.IsValidList"
      :key="item.value"
      :label="item.label"
      :value="item.value">
    </el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="平台类型" prop="ApplicationNo">
    <el-select v-model="value.ApplicationNo" placeholder="平台类型">
      <el-option
      v-for="item in value.ApplicationNoList"
      :key="item.value"
      :label="item.label"
      :value="item.value">
    </el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="顺序号" prop="Xh">
    <el-input v-model="value.Xh" placeholder="顺序号"></el-input>
  </el-form-item>
</el-form>
</template>

<script>
export default {
  props: {
    value: { type: Object }
  },
  methods: {
    onSubmit() {
      console.log("submit!");
    }
  }
};
</script>

<style scoped>
.el-input,
.el-input__inner {
  width: 200px;
}
</style>

