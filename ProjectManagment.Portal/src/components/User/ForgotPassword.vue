<template>
  <div>
    <h1>Forgot password</h1>
    <form v-on:submit.prevent="SubmitPasswordChange">
      <b-field label="Email">
        <b-input type="text" v-model="model.email"></b-input>
      </b-field>

      <b-button native-type="submit">Submit</b-button>
    </form>
  </div>
</template>

<script>
import ApiService from "../../services/api";
var api = new ApiService();

export default {
  name: "ForgotPassword",
  components: {},
  data() {
    return {
      model: {
        email: ""
      }
    };
  },
  mounted() {},
  methods: {
    SubmitPasswordChange: function() {
      var router = this.$router;
      api
        .create("Users/forgot_password", JSON.stringify(this.model.email))
        .then(resp => {
          if (resp.status == 400) {
            this.danger("No account with this login");
            return;
          } else {
            resp.json().then(function(data) {
                console.log(data)
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