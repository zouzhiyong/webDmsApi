<template>
<div class="body">
<div class="login">
    <el-form :model="ruleForm" :rules="rules2" ref="ruleForm" label-position="left" label-width="0px" class="demo-ruleForm login-container">
        <h3 class="title">系统登录</h3>
        <el-form-item prop="account">
            <el-input type="text" v-model="ruleForm.account" auto-complete="off" placeholder="账号">
                <template slot="prepend"><i class="fa fa-user" aria-hidden="true"></i></template>
            </el-input>            
        </el-form-item>
        <el-form-item prop="checkPass">
            <el-input type="password" v-model="ruleForm.checkPass" auto-complete="off" placeholder="密码">
                <template slot="prepend"><i class="fa fa-lock" aria-hidden="true"></i></template>
            </el-input>
        </el-form-item>
        <el-checkbox v-model="checked" checked class="remember">记住密码</el-checkbox>
        <el-form-item style="width:100%;">
            <el-button type="primary" style="width:100%;" @click="handleSubmit2" :loading="logining">登录</el-button>
        </el-form-item>
    </el-form>
</div>
</div>
</template>

<script>
export default {
    props: {
    },
    data() {
        return {
            logining: false,
            ruleForm: {
                account: 'admin',
                checkPass: '12345678'
            },
            rules2: {
                account: [{
                        required: true,
                        message: '请输入账号',
                        trigger: 'blur'
                    },
                    //{ validator: validaePass }
                ],
                checkPass: [{
                        required: true,
                        message: '请输入密码',
                        trigger: 'blur'
                    },
                    //{ validator: validaePass2 }
                ]
            },
            checked: true
        };
    },
    methods: {
        handleSubmit2(ev) {
            var _this = this;
            _this.$refs.ruleForm.validate((valid) => {
                if (valid) {
                    _this.logining = true;
                    var loginParams = {
                        strUser: this.ruleForm.account,
                        strPwd: this.ruleForm.checkPass
                    };
                    
                    _this.$http.post('api/Login/Login',loginParams)
                            .then(function(res){                                
                                    if (res.data.bRes) {
                                        _this.logining = false;
                                        sessionStorage.setItem('user', JSON.stringify(_this.ruleForm.account));
                                        sessionStorage.setItem('Ticket', res.data.Ticket);
                                        _this.$router.push({ path: '/home' });
                               
                            } else {
                                _this.logining = false;
                                _this.$alert('用户名或密码错误！', '提示信息', {confirmButtonText: '确定'});　　　　　　
                            }
                            })
                            .catch(function(err){
                                _this.logining = false;
                            })

                } else {
                    console.log('提交错误!!');
                    return false;
                }
            });
        }
    }
}
</script>

<style>
.body {
  background: #131a48;
  height: 100%;
  margin: 0;
}

.login {
  width: 300px;
  padding: 50px;
  position: absolute;
  top: calc(50% - 200px);
  left: calc(50% - 200px);
  box-shadow: 0 0 25px #cac6c6;
  -webkit-border-radius: 5px;
  border-radius: 5px;
  -moz-border-radius: 5px;
  background-clip: padding-box;
  border: 1px solid #131a48;
}

.login .el-button {
  width: 100%;
  margin-top: 20px;
}

.login .el-row {
  margin: 20px;
}

.login .el-input__inner {
  background: transparent;
  border: 1px solid #5283be;
  color: #fff;
}
.login .title {
  color: #fff;
}
</style>