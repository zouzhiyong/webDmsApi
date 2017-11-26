import axios from 'axios';
import Vue from 'vue'
import { Loading } from "element-ui";

let options = {
    text: '正在加载',
    //target: document.querySelector('.content-container')
}
let loadingInstance;
// import VueProgressBar from 'vue-progressbar'
// Vue.use(VueProgressBar, {
//     color: '#20a0ff',
//     failedColor: 'red',
//     thickness: '2px',
//     location: "bottom"
// })

let base = 'http://localhost/webDmsApi';

// 添加请求拦截器
axios.interceptors.request.use(function(config) {
    // 在发送请求之前做些什么
    // new Vue().$Progress.start()
    loadingInstance = Loading.service(options);
    return config;
}, function(error) {
    // 对请求错误做些什么
    // this.$Progress.fail()

    return Promise.reject(error);
});

// 添加响应拦截器
axios.interceptors.response.use(function(response) {
    // 对响应数据做点什么
    // new Vue().$Progress.finish();
    loadingInstance.close();
    if (response.data.result === true) {
        if (response.data.message !== "" && response.data.message !== null) {
            new Vue().$message({
                message: response.data.message,
                type: 'success'
            });
        }
    }
    //判断返回状态是否为否
    if (response.data.result === false) {
        new Vue().$message({
            message: response.data.message,
            type: 'error'
        });
    }
    return response;
}, function(error) {
    // 对响应错误做点什么
    // this.$Progress.fail()
    new Vue().$message({
        message: error,
        type: 'error'
    });
    return Promise.reject(error);
});


//登录
export const requestLogin = params => { return axios.post(`${base}/api/Login/Login`, params).then(res => res.data); };
//模块设置页面
export const getMenu = params => { return axios.post(`${base}/api/Menu/FindMenu`).then(res => res.data); };
export const FindSysModule = params => { return axios.post(`${base}/api/Menu/FindSysModule`).then(res => res.data); };
export const FindSysMoudleRow = params => { return axios.post(`${base}/api/Menu/FindSysMoudleRow`, params).then(res => res.data); };
export const FindMoudleForm = params => { return axios.post(`${base}/api/Menu/FindMoudleForm`, params).then(res => res.data); };
export const SaveSysMoudleForm = params => { return axios.post(`${base}/api/Menu/SaveSysMoudleForm`, params).then(res => res.data); };