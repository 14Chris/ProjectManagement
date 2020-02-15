<template>
  <div>
    <h1>Login</h1>
    <form class="ui form">
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
import ApiService from "../../services/api.service";
var api = new ApiService();
export default {
  name: "Login",
  components: {},
  data() {
    return {
      login:{
        email:"",
        password:""
      }
    };
  },
  mounted() {},
  methods:{
    login: function() {

    }
  }
};
</script>

<style>
</style>
