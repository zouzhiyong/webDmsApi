/**
 * 引用组件
 */
import login from './components/page/login' 
import home from './components/page/home' 
import index0 from './components/page/index0' 
import index2 from './components/page/index2'  

import step1 from './components/page/children/step1'  


export default[
	{
	    path : '/login',component : login
	},
    {
        path : '/home',component : home,
        children:[
            {
                name: '主页',
                path : '/main',component : index0
            },
            {
                name: '模块设置',
                path : '/index0',component : index0
            }
        ]
    }
//	{
//	    path : '/index0',component : index0,
//	},
//	{
//	    path : '/index1',component : index1,
//	},
//	{
//	    path : '/step1',component : step1,
//	},
//	{
//	    path : '/index2',component : index2
//	}
]




