<template>
  <div class="container">
    <b-loading :is-full-page="isFullPage" :active.sync="isLoading" :can-cancel="true"></b-loading>
    <form v-on:submit.prevent="Login">
      <h1>Login</h1>
      <b-field label="Email">
        <b-input v-model="login.email"></b-input>
      </b-field>
      <div class="error" v-if="!$v.login.email.required && submitStatus=='ERROR'">Email is required</div>

      <b-field label="Password">
        <b-input type="password" v-model="login.password"></b-input>
      </b-field>
      <div
        class="error"
        v-if="!$v.login.password.required && submitStatus=='ERROR'"
      >Password is required</div>

      <b-button native-type="submit">Login</b-button>
      <b-button tag="router-link" to="/register" type="is-link">Register</b-button>
      <b-button type="is-text" tag="router-link" to="/forgot_password">Forgot password ?</b-button>
    </form>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import { required } from "vuelidate/lib/validators";
// var _this = this;
var api = new ApiService();
export default {
  name: "Login",
  components: {},
  data() {
    return {
      isLoading: false,
      isFullPage: true,
      submitStatus: "",
      login: {
        email: "",
        password: ""
      }
    };
  },
  mounted() {},
  methods: {
    Login: function() {
      var router = this.$router;
      var _this = this;
      if (this.$v.$invalid) {
        _this.submitStatus = "ERROR";
      } else {
        _this.isLoading = true;
        _this.submitStatus = "PENDING";
        api
          .create("Login", JSON.stringify(this.login))
          .then(resp => {
            if (resp.status == 401) {
              _this.isLoading = false;
              resp.json().then(data => {
                this.danger("Authentication failed : " + data);
              });
         
              return;
            } else {
              _this.submitStatus = "OK";

              resp.json().then(function(data) {
                console.log(data);
   
                var token = data.token;
                localStorage.setItem("userToken", token);
                _this.isLoading = false;
                router.push("/");
              });
            }
          })
          .catch(error => {
            _this.isLoading = false;
            console.log("error", error);
          });
      }
    },
    danger(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-danger"
      });
    }
  },
  validations: {
    login: {
      email: {
        required
      },
      password: {
        required
      }
    }
  }
};
</script>

<style>
</style>
