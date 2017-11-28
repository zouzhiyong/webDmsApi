import babelpolyfill from 'babel-polyfill'
import Vue from 'vue'
import App from './App'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-default/index.css'
//import './assets/theme/theme-green/index.css'
import VueRouter from 'vue-router'
import store from './vuex/store'
import Vuex from 'vuex'
// import VueProgressBar from 'vue-progressbar'
//import NProgress from 'nprogress'
//import 'nprogress/nprogress.css'
import routes from './routes'
// import Mock from './mock'
// Mock.bootstrap();
import 'font-awesome/css/font-awesome.min.css'

Vue.use(ElementUI)
Vue.use(VueRouter)
Vue.use(Vuex)

// Vue.prototype.ExportJsonExcel = ExportJsonExcel;
// Vue.use(VueProgressBar, {
//     color: 'rgb(143, 255, 199)',
//     failedColor: 'red',
//     height: '2px'
// })

Vue.config.productionTip = false;


//NProgress.configure({ showSpinner: false });

const router = new VueRouter({
    routes
})

let data = JSON.parse(sessionStorage.menuData || "[]");
if (data.length > 0) {
    data.map(item => {
        delete item.name;
        item.component = resolve => require([`./views/Home.vue`], resolve);
        item.children.map(_item => {
            _item.meta = _item.Button;
            if (_item.MenuPath != null && _item.MenuPath != "") {
                let str = JSON.stringify(_item.MenuPath)
                    .replace(/\"/g, "")
                    .split("_")[0];
                let path = "./views/" + str + "/" + _item.MenuPath;
                _item.component = resolve => require([path + `.vue`], resolve);
            }
        });

        // router.options.routes.push(item);
    });
    router.addRoutes(data);
}

router.beforeEach((to, from, next) => {
    //NProgress.start();
    if (to.path == '/login') {
        sessionStorage.removeItem('user');
        sessionStorage.removeItem('menuData');
    }
    let user = JSON.parse(sessionStorage.getItem('user'));
    if (!user && to.path != '/login') {
        sessionStorage.removeItem('user');
        sessionStorage.removeItem('menuData');
        next({ path: '/login' })
    } else {
        next()
    }
})



//router.afterEach(transition => {
//NProgress.done();
//});

new Vue({
    //el: '#app',
    //template: '<App/>',
    router,
    store,
    //components: { App }
    render: h => h(App)
}).$mount('#app')