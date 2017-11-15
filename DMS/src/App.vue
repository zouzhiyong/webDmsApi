<template>
  <div id="app">

    <!-- 头部 -->
    <div class="head">
      <span>Vue后台系统模板</span>
      <el-menu  theme="dark" class="el-menu-demo" mode="horizontal" >
        <el-submenu index="2">
          <template slot="title">选项</template>
          <el-menu-item index="2-3">退出登录</el-menu-item>
        </el-submenu>
      </el-menu>
    </div>

    <div class="wrap">
      <!-- 左侧导航 -->
      <div class="sidebar">
       <el-row class="tac">
          <el-col>
            <el-menu :unique-opened="false" theme="dark" :router="true" default-active="/index1"  class="el-menu-vertical-demo">
              <el-submenu index="1">
                <template slot="title"><i class="el-icon-message"></i>导航一</template>
                <el-menu-item-group>
                  <el-menu-item index="/index1">数据列表</el-menu-item>
                </el-menu-item-group>
                <el-submenu index="1-3">
                  <template slot="title">选项3</template>
                  <el-menu-item index="/step1">表单</el-menu-item>
                </el-submenu>
              </el-submenu>
              <el-menu-item index="/index2"><i class="el-icon-menu"></i>导航二</el-menu-item>
            </el-menu>
          </el-col>
        </el-row>
      </div>
      <!-- 右侧主要 -->
      <div class="main">
        <transition name="fade">
          <router-view></router-view>
        </transition>
      </div>
    </div>
   
  </div>
</template>


<script>

  import Vue from 'vue'
  import Element from 'element-ui'
  import 'element-ui/lib/theme-default/index.css'
  Vue.use(Element)

  export default{
    data() {
      return {
        
      }
    }
  }

</script>



<style>
  *{
    margin: 0;
    padding: 0;
  }
  body{
    margin: 0;
    height: 100%;
    overflow: hidden;
  }
  .head{
    position: fixed;
    top: 0;
    right: 0;
    left: 0;
    z-index: 10000;
    height: 60px;
    margin-bottom: 2px;
    background: #324157;
  }
  .head span{
    color: #bfcbd9;
    font-size: 20px;
    padding-left: 10px;
    float: left;
    line-height: 60px;
  }

  .sidebar{
    position: fixed;
    top: 62px;
    left: 0;
    bottom: 0;
    width: 200px;
    background: #000;
    background: #324157;
    z-index: 10000;
  }
  .main {
      position: absolute;
      top: 62px;
      left: 203px;
      right: 0;
      bottom: 0;
      overflow: auto;
      background: #efefef;
      padding: 20px;
  }
  .el-menu-demo {
    float: right;
  }
</style>











