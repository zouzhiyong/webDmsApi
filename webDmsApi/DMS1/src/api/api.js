import axios from 'axios';
import Vue from 'vue'
import VueProgressBar from 'vue-progressbar'
Vue.use(VueProgressBar, {
    color: '#20a0ff',
    failedColor: 'red',
    thickness: '2px',
    location: "bottom"
})

let base = 'http://localhost/webDmsApi';

// 添加请求拦截器
axios.interceptors.request.use(function(config) {
    // 在发送请求之前做些什么
    new Vue().$Progress.start()
    return config;
}, function(error) {
    // 对请求错误做些什么
    this.$Progress.fail()
    return Promise.reject(error);
});

// 添加响应拦截器
axios.interceptors.response.use(function(response) {
    // 对响应数据做点什么
    new Vue().$Progress.finish();
    return response;
}, function(error) {
    // 对响应错误做点什么
    this.$Progress.fail()
    return Promise.reject(error);
});


export const requestLogin = params => { return axios.post(`${base}/api/Login/Login`, params).then(res => res.data); };

export const getMenu = params => { return axios.post(`${base}/api/Menu/FindMenu`).then(res => res.data); };

export const FindSysModule = params => { return axios.post(`${base}/api/Menu/FindSysModule`).then(res => res.data); };

export const FindSysMoudleRow = params => { return axios.post(`${base}/api/Menu/FindSysMoudleRow`, params).then(res => res.data); };




export const getUserListPage = params => { return axios.get(`${base}/user/listpage`, { params: params }); };

export const removeUser = params => { return axios.get(`${base}/user/remove`, { params: params }); };

export const batchRemoveUser = params => { return axios.get(`${base}/user/batchremove`, { params: params }); };

export const editUser = params => { return axios.get(`${base}/user/edit`, { params: params }); };

export const addUser = params => { return axios.get(`${base}/user/add`, { params: params }); };