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
    // mode: 'history',
    saveScrollPosition: true,
    routes
})

// let data = JSON.parse(sessionStorage.menuData || "[]");
// if (data.length > 0) {
//     data.map(item => {
//         // delete item.name;
//         item.component = resolve => require([`./views/Home.vue`], resolve);
//         item.children.map(_item => {
//             _item.meta = _item.Button;
//             if (_item.MenuPath != null && _item.MenuPath != "") {
//                 let path = "./views/" + _item.MenuPath + "/" + _item.MenuPath;
//                 _item.component = resolve => require([path + `.vue`], resolve);
//             }
//         });

//         // router.options.routes.push(item);
//     });
//     router.addRoutes(data);
// }
//检测本地路由
let localRoutes = sessionStorage.routes;
let data = sessionStorage.menuData;
if (localRoutes && data) {
    let data = JSON.parse(sessionStorage.menuData);

    data.map(item => {
        let temObj = {
            meta: { name: item.name, button: [] },
            path: item.path,
            component: resolve => require([`./views/Home.vue`], resolve)
        };
        if (item.children) {
            temObj.children = [];
            item.children.map(_item => {
                let path = "./views/" + _item.MenuPath;
                temObj.children.push({
                    meta: { name: _item.name, button: _item.Button },
                    name: _item.name,
                    path: _item.path,
                    component: resolve => require([path + `.vue`], resolve)
                });
            });
        }

        router.options.routes.push(temObj);
    });

    router.options.routes.push({
        path: "*",
        hidden: true,
        redirect: { path: "/404" }
    });
    sessionStorage.routes = router.options.routes;
    router.addRoutes(router.options.routes);
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