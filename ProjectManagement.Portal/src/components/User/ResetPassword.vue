<template>
  <div class="container">
    <h1>Reset password</h1>
    <form v-on:submit.prevent="SubmitPasswordChange">
      <!-- <div>{{this.model.token}}</div> -->
      <b-field label="Password">
        <b-input type="password" v-model="model.password"></b-input>
      </b-field>
      <div
        class="error"
        v-if="!$v.model.password.required && submitStatus=='ERROR'"
      >Password is required</div>
      <div
        class="error"
        v-if="!$v.model.password.minLength && submitStatus=='ERROR'"
      >Password must have at least {{$v.model.password.$params.minLength.min}} letters.</div>
      <b-field label="Confirm password">
        <b-input type="password" v-model="model.repeatPassword"></b-input>
      </b-field>
      <div
        class="error"
        v-if="!$v.model.repeatPassword.sameAsPassword && submitStatus=='ERROR'"
      >Confirmation password has to be the same as password</div>
      <div
        class="error"
        v-if="!$v.model.repeatPassword.required && submitStatus=='ERROR'"
      >Confirmation password is required</div>
      <b-button type="is-primary" expanded native-type="submit">Submit</b-button>
    </form>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import { required, sameAs, minLength } from "vuelidate/lib/validators";
var api = new ApiService();

export default {
  name: "ForgotPassword",
  components: {},
  data() {
    return {
      token: "",
      model: {
        password: "",
        repeatPassword: ""
      }
    };
  },
  mounted() {
    var router = this.$router;
    api.getData("Users/reset_password/" + this.$route.params.token).then(response => {

      if (response.status == 200) {
        this.token = this.$route.params.token;
      } else {
        response.json().then(async data => {
          this.danger(data);
          await(3000);
          router.push("/login");
        });
      }
    });
  },
  methods: {
    SubmitPasswordChange: function() {
        var router = this.$router;
      api
        .create(
          "Users/reset_password/" + this.token,
          JSON.stringify(this.model.password)
        )
        .then(resp => {
          if (resp.status == 200) {
            this.success("Password changed");
            router.push("/login");
          } else {
            this.danger("Error in changing password");
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
    },
    success(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-success"
      });
    }
  },
  validations: {
    model: {
      password: {
        required,
        minLength: minLength(8)
      },
      repeatPassword: {
        required,
        sameAsPassword: sameAs("password")
      }
    }
  }
};
</script>

<style>
</style>