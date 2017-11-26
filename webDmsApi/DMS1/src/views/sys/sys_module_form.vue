<template>
  <el-form :inline="true" :model="formInline" label-width='80px' class="demo-form-inline">
  <el-form-item label="模块名称" required>
    <el-input v-model="formInline.MenuName" placeholder="模块名称"></el-input>
  </el-form-item>
  <el-form-item label="上级模块">
    <el-select v-model="formInline.MenuParentID" placeholder="上级模块">
      <el-option label="区域一" value="shanghai"></el-option>
      <el-option label="区域二" value="beijing"></el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="模块地址">
    <el-input v-model="formInline.MenuPath" placeholder="模块地址"></el-input>
  </el-form-item>
  <el-form-item label="图标">
    <el-input v-model="formInline.MenuIcon" placeholder="图标"></el-input>
  </el-form-item>
  <el-form-item label="有效否"">
    <el-select v-model="formInline.IsValid" placeholder="有效否"">
      <el-option label="有效" value="1"></el-option>
      <el-option label="无效" value="0"></el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="平台类型"">
    <el-select v-model="formInline.ApplicationNo" placeholder="平台类型"">
      <el-option label="有效" value="1"></el-option>
      <el-option label="无效" value="0"></el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="顺序号">
    <el-input v-model="formInline.Xh" placeholder="顺序号"></el-input>
  </el-form-item>
</el-form>
</template>

<script>
export default {
  data() {
    return {
      formInline: {
        user: "",
        region: ""
      }
    };
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

