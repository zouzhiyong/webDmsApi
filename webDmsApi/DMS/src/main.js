// 引入文件
import Vue from 'vue'
import App from './App'
import routerConfig from './router-config'
import VueRouter from 'vue-router'
Vue.use(VueRouter)

// 创建路由
const router = new VueRouter({
  routes: routerConfig
})
// 实例
new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
