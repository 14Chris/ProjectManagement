<template>
  <div>
    <form v-on:submit.prevent="Login" class="ui form">
      <h1>Login</h1>
      <b-field label="Email">
        <b-input v-model="login.email"></b-input>
      </b-field>

      <b-field label="Password">
        <b-input type="password" v-model="login.password"></b-input>
      </b-field>

      <b-button native-type="submit">Login</b-button>
      <b-button tag="router-link" to="/register" type="is-link">Register</b-button>
    </form>
  </div>
</template>

<script>
import ApiService from "../../services/api";
// var _this = this;
var api = new ApiService();
export default {
  name: "Login",
  components: {},
  data() {
    return {
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

      api
        .create("Login", JSON.stringify(this.login))
        .then(resp => {
          if (resp.status == 401) {
            this.danger("Authentication failed");
            return;
          } else {
            resp.json().then(function(data) {
              console.log(data);
              // Create and append the li's to the ul
              var token = data.token;
              localStorage.setItem("userToken", token);
              router.push("/");
            });
          }
        }) // Transform the data into json

        .catch(error => {
          console.log("error", error);
        });
    },
    danger(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-danger"
      });
    }
  }
};
</script>

<style>
</style>
