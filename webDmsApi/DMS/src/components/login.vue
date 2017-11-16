<template>
    <div class='login'>
        <el-form :model="ruleForm" ref="ruleForm" :rules="loginRules">
            <el-form-item prop="userName">
                <el-input placeholder="请输入帐号" v-model="ruleForm.userName" auto-complete="on">
                    <template slot="prepend">
                        <i class="fa fa-user" aria-hidden="true"></i>
                    </template>
                </el-input>
            </el-form-item>
            <el-form-item prop="passWord">
                <el-input type="password" placeholder="请输入密码" v-model="ruleForm.passWord" auto-complete="off" @keyup.enter.native="handleLogin">
                    <template slot="prepend">
                        <i class="fa fa-lock" aria-hidden="true"></i>
                    </template>
                </el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="handleLogin" :loading="logining">登录</el-button>
            </el-form-item>
        </el-form>       
    </div>
</template>


<script>
import axios from 'axios';
    export default{
        data(){
            return {
                logining: false,
                ruleForm: {
                    userName: 'admin',
                    passWord: '12345678'
                },
                loginRules: {
                    userName: [{
                        required: true,
                        message: '帐号不能为空',
                        trigger: 'blur'
                    }],
                    passWord: [{
                        required: true,
                        message: '密码不能为空',
                        trigger: 'blur'
                    }]
                }
            }        
        },
        methods: {
            handleLogin() {
                var _self = this;
                this.$refs.ruleForm.validate(function(valid) {
                    if (valid) {
                        _self.logining = true;
                        axios.post('http://localhost/webDmsApi/api/Login/Login',{
                                strUser: _self.ruleForm.userName,
                                strPwd: _self.ruleForm.passWord
                        })
                            .then(function(res){
                                _self.logining = false;
                                    if (res.data.bRes) {
                                        sessionStorage.setItem('user', JSON.stringify(_self.ruleForm.userName));
                                        _self.$router.push({ path: '/home' });
                                //登录成功之后将用户名和用户票据带到主界面
                                //$.cookie('Ticket', result.Ticket);
                                //window.location.href = "index.html";
                            } else {
                                _self.logining = false;
                                _self.$message(res.data.message);
                                return;　　　　　　　
                            }
                            })
                            .catch(function(err){
                                _self.$message(err);
                            })
                        // $.cookie('Ticket', '', {
                        //     expires: -1
                        // });
                        // ajaxData('api/Login/Login', {
                        //     data: {
                        //         strUser: _self.ruleForm.userName,
                        //         strPwd: _self.ruleForm.passWord
                        //     }
                        // }).then(function(result) {
                        //     if (result.bRes) {　　　　
                        //         //登录成功之后将用户名和用户票据带到主界面
                        //         $.cookie('Ticket', result.Ticket);
                        //         window.location.href = "index.html";
                        //     } else {
                        //         _self.$message(result.message);
                        //         return;　　　　　　　
                        //     }
                        // }, function() {

                        // });
                    } else {
                        return false;
                    }
                });
            }
    }
    }

</script>


<style>
body {
  background: #131a48;
  height: 100%;
  margin: 0;
}

.login {
  width: 300px;
  position: absolute;
  top: calc(50% - 90px);
  left: calc(50% - 150px);
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
</style>

