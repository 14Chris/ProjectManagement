<template>
  <div>
    <h1>Login</h1>
    <form v-on:submit.prevent="Login" class="ui form">
      <div class="field">
        <label>Email</label>
        <input type="text" v-model="login.email" name="email" placeholder="Email" />
      </div>
      <div class="field">
        <label>Password</label>
        <input type="password" v-model="login.password" name="password" placeholder="Password" />
      </div>

      <button class="ui button" type="submit">Login</button>
    </form>
  </div>
</template>

<script>
import ApiService from "../../services/api";
var _this = this;
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
      api
        .create("Login", JSON.stringify(this.login))
        .then(resp => resp.json()) // Transform the data into json
        .then(function(data) {
          // Create and append the li's to the ul
          var token = data.token;
          localStorage.setItem('userToken',token);
          _this.$router.push('/')
        })
        .catch(error => {
          console.log("error", error);
        });
    }
  }
};
</script>

<style>
</style>
