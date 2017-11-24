import axios from 'axios';

let base = 'http://localhost/webDmsApi';

export const requestLogin = params => { return axios.post(`${base}/api/Login/Login`, params).then(res => res.data); };

export const getMenu = params => { return axios.post(`${base}/api/Menu/FindMenu`); };

export const FindSysModule = params => { return axios.post(`${base}/api/Menu/FindSysModule`); };

export const FindSysMoudleRow = params => { return axios.post(`${base}/api/Menu/FindSysMoudleRow`, params); };




export const getUserListPage = params => { return axios.get(`${base}/user/listpage`, { params: params }); };

export const removeUser = params => { return axios.get(`${base}/user/remove`, { params: params }); };

export const batchRemoveUser = params => { return axios.get(`${base}/user/batchremove`, { params: params }); };

export const editUser = params => { return axios.get(`${base}/user/edit`, { params: params }); };

export const addUser = params => { return axios.get(`${base}/user/add`, { params: params }); };