<template lang="html">
<el-container style="height:100%">
  <el-header height="60px">
    <el-row class="container">
      <el-col :span="24" class="header">
        <el-col class="logo">{{sysName}}</el-col>
        <el-col class="tools" @click.prevent="collapseFun">
          <i class="fa fa-align-justify"></i>
        </el-col>
      </el-col>
    </el-row>
  </el-header>
  <el-container>
    <el-aside width="200px">
      <el-menu
      default-active="2"
      unique-opened=true
      class="el-menu-vertical-demo"
      @open="handleOpen"
      @close="handleClose"
      text-color="#fff"
      active-text-color="#fff">
      <el-menu-item index="0">
        <i class="el-icon-menu"></i>
        <span slot="title">主页</span>
      </el-menu-item>
      <el-submenu index="1">
        <template slot="title">
          <i class="el-icon-location"></i>
          <span>导航一</span>
        </template>        
        <el-menu-item index="1-1">选项1</el-menu-item>
      </el-submenu>
      <el-submenu index="2">
        <template slot="title">
          <i class="el-icon-location"></i>
          <span>导航二</span>
        </template>        
        <el-menu-item index="2-1">选项1</el-menu-item>
        <el-menu-item index="2-2">选项2</el-menu-item>
      </el-submenu>
    </el-menu>
    </el-aside>
    <el-container>
      <el-main>Main</el-main>
      <el-footer height="40px">© 2016 XXX.com 版权所有 ICP证：湘ICP备XXXXXXX</el-footer>
    </el-container>
  </el-container>
</el-container>
</template>

<script>
export default {
    components: {},
    data() {
        return {
            menu:[{
        path: '/login',
        name: '',
        hidden: true
    },
    {
        path: '/404',
        name: '',
        hidden: true
    },
    {
        path: '/',
        name: '导航一',
        iconCls: 'el-icon-message', //图标样式class
        children: [{
            path: '/menutab',
            name: 'Tab'
        }]
    },
    {
        path: '/',
        name: '导航二',
        iconCls: 'fa fa-id-card-o',
        children: [{
                path: '/menutable',
                name: 'Table'
            }
        ]
    }
],
            sysName: 'VueDemo',
            sysUserName: '',
            collapsed: false
        }
    },
    methods: {
        //折叠导航栏
        collapseFun: function() {
            this.collapsed = !this.collapsed;
        },
        showMenu(i, status){
          this.$refs.menuCollapsed.getElementsByClassName('submenu-hook-' + i)[0].style.display = status ? 'block' : 'none';
        },
        //退出登录
        logoutFun: function() {
            var _this = this;
            this.$confirm('确认退出吗?', '提示', {
                //type: 'warning'
            }).then(() => {
                sessionStorage.removeItem('user');
                sessionStorage.removeItem('Ticket');
                _this.$router.push('/login');
            }).catch(() => {

            });


        },
    },
    mounted() {
        var user = sessionStorage.getItem('user');
        if (user) {
            user = JSON.parse(user);
            this.sysUserName = user.username || '';
        }
    }
}
</script>

<style>
.el-header {
  background-color: #538bfa;
  color: #fff;
  text-align: center;
  line-height: 60px;
}

.logo {
  width: 160px;
  font-size: 22px;
}
.tools {
  padding: 0 23px;
  width: 14px;
  height: 60px;
  line-height: 60px;
  cursor: pointer;
}
.el-footer {
  background-color: #fff;
  color: #333;
  text-align: right;
  line-height: 40px;
}

.el-aside {
  background-color: #080d16;
  color: #333;
  text-align: center;
  line-height: 100%;
}
.el-menu,
.el-submenu__title,
.el-submenu .el-menu-item,
.el-menu-item {
  background: #080d16;
  text-align: left;
  padding-right: 0;
}

.el-menu-item.is-active {
  background: #0f1829;
}
.el-submenu__title:hover,
.el-menu-item:focus,
.el-menu-item:hover {
  background: #0f1829;
}
.el-main {
  background-color: #e9eef3;
  color: #333;
  text-align: center;
  line-height: 160px;
}

body > .el-container {
  margin-bottom: 40px;
}

.el-container:nth-child(5) .el-aside,
.el-container:nth-child(6) .el-aside {
  line-height: 260px;
}

.el-container:nth-child(7) .el-aside {
  line-height: 320px;
}
</style>