<template>
  <div class="container">
    <b-loading :is-full-page="isFullPage" :active.sync="isLoading" :can-cancel="true"></b-loading>
    <div class="card">
      <div class="card-content">
        <p class="title">Login</p>

        <form v-on:submit.prevent="Login">
          <b-field
            v-if="$v.login.email.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="Email"
            :message="!$v.login.email.required ? 'Email is required' : ''"
          >
            <b-input v-model="login.email"></b-input>
          </b-field>

          <b-field v-else label="Email">
            <b-input v-model="login.email"></b-input>
          </b-field>

          <b-field
            v-if="$v.login.password.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="Password"
            :message="!$v.login.password.required ? 'Password is required' : ''"
          >
                <b-input type="password" v-model="login.password"></b-input>
          </b-field>

          <b-field v-else label="Password">
            <b-input type="password" v-model="login.password"></b-input>
          </b-field>

          <b-button type="is-primary" expanded native-type="submit">Login</b-button>

          <b-button type="is-text" tag="router-link" to="/forgot_password">Forgot password ?</b-button>
          <div class="div-register">
            Don't have an account ?
            <b-button id="btn-register" tag="router-link" to="/register" type="is-text">Register</b-button>
          </div>
        </form>
      </div>
    </div>
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
      console.log(this.$v)
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
                let errorMessage = "";
                switch (data) {
                  case "NOT_ACTIVATED":
                    errorMessage =
                      "Your account is not active yet. Click on the link in the email we sent you";
                    break;
                  case "BAD_CREDENTIALS":
                    errorMessage = "Invalid email or password";
                    break;
                  default:
                    errorMessage = "Error connecting to the application";
                }

                this.danger(errorMessage);
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
          .catch(() => {
            _this.isLoading = false;
            let errorMessage = "Error connecting to the application";
            this.danger(errorMessage);
          });
      }
    },
    danger(message) {
      this.$buefy.notification.open({
        duration: 5000,
        message: message,
        type: "is-danger"
        // hasIcon: true
      });
    },
    success(message) {
      this.$buefy.notification.open({
        duration: 5000,
        message: message,
        type: "is-success"
        // hasIcon: true
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

<style scoped>
#btn-register {
  vertical-align: middle;
}


</style>
