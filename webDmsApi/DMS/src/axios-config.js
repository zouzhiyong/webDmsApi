import Vue from 'vue';
import 'element-ui/lib/theme-chalk/index.css';
import ElementUI from 'element-ui';
import axios from 'axios';

Vue.use(ElementUI);

var instance = axios.create({
    baseURL: 'http://localhost/webDmsApi/',
    timeout: 10000,
    headers: {'Authorization': sessionStorage.getItem('Ticket') || ''},
});

// 添加请求拦截器
instance.interceptors.request.use(function (config) {
    // 在发送请求之前做些什么
    
    return config;
}, function (error) {
    // 对请求错误做些什么    
    return Promise.reject(error);
});

// 添加响应拦截器
instance.interceptors.response.use(function (response) {
    // 对响应数据做点什么
    var msg=response.data.message || '';
    if(msg !== ""){
        new Vue().$message.error(JSON.stringify(msg));
    }
    return response;
}, function (error) {
    // 对响应错误做点什么
    if (error.response) {
        // 请求已发出，但服务器响应的状态码不在 2xx 范围内
        new Vue().$message.error(JSON.stringify(error.response.data));
    } else {
        // Something happened in setting up the request that triggered an Error
        new Vue().$message.error(JSON.stringify(error.message));
    }
    return Promise.reject(error);
});

export default instance;
