<template lang="html">
<el-container style="height:100%">
  <el-header height="60px">
    <el-row class="container">
      <el-col :span="24" class="header">
        <el-col class="logo">{{sysName}}</el-col>
        
      </el-col>
    </el-row>
  </el-header>
  <el-container>
    <el-aside width="auto" :class="collapsed?'collapsed':''">
      <div class="tools" @click.prevent="collapseFun">
          <i class="fa fa-bars"></i>
        </div>
      <el-menu
      :collapse="collapsed"
      default-active="2"
      unique-opened
      class="el-menu-vertical-demo"      
      text-color="#fff"
      active-text-color="#fff">
      <el-menu-item index="0">
        <i class="fa fa-home"></i>
        <span slot="title">主页</span>
      </el-menu-item>
      <el-submenu :index="index.toString()" v-for="(menuItem,index) in menuData" :key="menuItem.MenuID" v-if="menuItem.MenuParentID=='0'">
        <template slot="title">
          <i :class="menuItem.MenuIcon"></i>
          <span>{{menuItem.MenuName}}</span>
        </template>
        <el-menu-item :index="index.toString()+'-'+_index.toString()" v-for="(subMenuItem,_index) in menuData"  :key="subMenuItem.MenuID" v-if="subMenuItem.MenuParentID==menuItem.MenuID">{{subMenuItem.MenuName}}</el-menu-item>
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
          collapsed:false,
          menuData:[],
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
    created(){
      var _this=this;
      _this.$http.post('api/Menu/FindSysMenu')
                            .then(function(res){ 
                              if (res.data.result) {
                                res.data.data.map(function (item) {
                                  if (item.MenuParentID == null) {
                                    item.MenuParentID = 0;
                                  }
                                  if (item.MenuIcon == null) {
                                      item.MenuIcon = "";
                                  }
                                  if (item.MenuUrl == null) {
                                      item.MenuUrl = "";
                                  }
                                  })
							              _this.menuData = res.data.data;
                            
                            }
                })  
    },
    mounted() {
        // var user = sessionStorage.getItem('user');
        // if (user) {
        //     user = JSON.parse(user);
        //     this.sysUserName = user.username || '';
        // }
    }
}
</script>

<style>
.el-container .el-header {
  background-color: #538bfa;
  color: #fff;
  text-align: center;
  line-height: 60px;
}

.el-container .logo {
  width: 160px;
  font-size: 22px;
}
.el-container .tools {
  height: 40px;
  line-height: 40px;
  cursor: pointer;
  background: #0f1829;
  color: #fff;
  text-align: center;
}
.el-container .el-footer {
  background-color: #fff;
  color: #333;
  text-align: right;
  line-height: 40px;
}

.el-container .el-aside {
  background-color: #080d16;
  color: #333;
  line-height: 100%;
}

.el-container .el-aside.collapsed {
  overflow: visible;
}

.el-container .el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
}
.el-container .el-menu-item [class^="fa"],
.el-container .el-submenu [class^="fa"] {
  vertical-align: baseline;
  margin-right: 5px;
  margin-left: 5px;
}
.el-container .el-menu,
.el-container .el-submenu__title,
.el-container .el-submenu .el-menu-item,
.el-container .el-menu-item {
  background: #080d16;
  border: 0;
}

.el-container .el-menu--collapse .el-submenu .el-menu-item {
  padding-left: 30px !important;
  padding-right: 30px !important;
  min-width: 130px;
}
.el-container
  .el-menu-vertical-demo:not(.el-menu--collapse)
  .el-submenu
  .el-menu-item {
  padding-left: 60px !important;
}

.el-container .el-menu-item,
.el-container .el-submenu__title,
.el-container .el-submenu .el-menu-item {
  height: 40px;
  line-height: 40px;
}

.el-container .el-menu-item,
.el-container .el-submenu {
  border-bottom-color: #0f1829;
  border-bottom-style: solid;
  border-bottom-width: 1px;
}

.el-container .el-menu-item i,
.el-container .el-submenu__title i {
  font-size: 13px;
  color: #fff;
}

.el-container .el-submenu.is-opened .el-submenu__title {
  padding-left: 16px !important;
}

.el-container
  .el-menu-vertical-demo:not(.el-menu--collapse)
  .el-submenu.is-opened
  .el-menu-item {
  padding-left: 56px !important;
}

.el-container .el-submenu.is-opened {
  border-left-color: #538bfa;
  border-bottom-color: #0f1829;
  border-left-style: solid;
  border-left-width: 4px;
}

.el-container .el-menu-item.is-active,
.el-container .el-submenu__title:hover,
.el-container .el-menu-item:focus,
.el-container .el-menu-item:hover {
  background: #0f1829;
}
.el-container .el-main {
  background-color: #e9eef3;
  color: #333;
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