<template>
<div id="app">
    <router-view></router-view>
</div>
</template>

<script>
export default {}
</script>

<style>
* {
  font-family: Helvetica Neue, Helvetica, PingFang SC, Hiragino Sans GB,
    Microsoft YaHei, SimSun, sans-serif;
  font-size: 13px;
}
body {
  margin: 0;
  padding: 0;
  font-size: 14px;
  -webkit-font-smoothing: antialiased;
  overflow: scroll;
  overflow-y: hidden;
  height: 100%;
}

#app {
  position: absolute;
  top: 0;
  bottom: 0;
  width: 100%;
  height: 100%;
}

.el-submenu [class^="fa"] {
  vertical-align: baseline;
  margin-right: 10px;
}

.el-menu-item [class^="fa"] {
  vertical-align: baseline;
  margin-right: 10px;
}

.toolbar {
  background: #f2f2f2;
  padding: 10px;
  margin: 10px 0;
}

.toolbar .el-form-item {
  margin-bottom: 10px;
}
</style>