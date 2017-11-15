/**
 * 引用组件
 */
import index0 from './components/page/index0' 
import index1 from './components/page/index1' 
import index2 from './components/page/index2'  

import step1 from './components/page/children/step1'  


export default[
	{
	    path : '',component : index0
	},
	{
	    path : '/index0',component : index0,
	},
	{
	    path : '/index1',component : index1,
	},
	{
	    path : '/step1',component : step1,
	},
	{
	    path : '/index2',component : index2
	}
]




