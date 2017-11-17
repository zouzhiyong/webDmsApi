// 引入文件
import Vue from 'vue';
import App from './App';
import routerConfig from './router-config';
import VueRouter from 'vue-router';
import 'font-awesome/css/font-awesome.min.css';
import 'element-ui/lib/theme-chalk/index.css';
import ElementUI from 'element-ui';
import axios from 'axios';

Vue.prototype.$http = axios;

Vue.use(VueRouter)
Vue.use(ElementUI);

// 创建路由
const router = new VueRouter({
  routes: routerConfig
})

router.beforeEach((to, from, next) => {
    if (to.path == '/login') {
        sessionStorage.removeItem('user');
    }
    let user = JSON.parse(sessionStorage.getItem('user'));
    if (!user && to.path != '/login') {
        next({
            path: '/login'
        });
    } else {
        next();
    }
});


// 实例
new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
