<template>
<div class="login">
    <el-form :model="ruleForm2" :rules="rules2" ref="ruleForm2" label-position="left" label-width="0px" class="demo-ruleForm login-container">
        <h3 class="title">系统登录</h3>
        <el-form-item prop="account">
            <el-input type="text" v-model="ruleForm2.account" auto-complete="off" placeholder="账号"></el-input>
        </el-form-item>
        <el-form-item prop="checkPass">
            <el-input type="password" v-model="ruleForm2.checkPass" auto-complete="off" placeholder="密码"></el-input>
        </el-form-item>
        <el-checkbox v-model="checked" checked class="remember">记住密码</el-checkbox>
        <el-form-item style="width:100%;">
            <el-button type="primary" style="width:100%;" @click="handleSubmit2" :loading="logining">登录</el-button>
        </el-form-item>
    </el-form>
</div>
</template>

<script>
export default {
    props: {
    },
    data() {
        return {
            logining: false,
            ruleForm2: {
                account: 'admin',
                checkPass: '123456'
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
            _this.$refs.ruleForm2.validate((valid) => {
                if (valid) {
                    _this.logining = true;
                    var loginParams = {
                        username: this.ruleForm2.account,
                        password: this.ruleForm2.checkPass
                    };
                    if (loginParams.username == "admin" && loginParams.password == "123456") {
                        _this.logining = false;
                        sessionStorage.setItem('user', JSON.stringify(loginParams));
                        _this.$router.push({ path: '/home' });
                    } else {
                        _this.logining = false;
                        _this.$alert('用户名或密码错误！', '提示信息', {
                            confirmButtonText: '确定'
                        });
                    }
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        }
    }
}
</script>

<style scoped>
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
.login .title{color: #fff}
</style>