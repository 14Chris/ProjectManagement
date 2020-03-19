<template>
  <div class="container">
    <div class="card">
      <div class="card-content">
        <p class="title">Forgot password</p>
        <form v-on:submit.prevent="SubmitPasswordChange">
          <b-field
            v-if="$v.model.email.$error && submitStatus=='ERROR'"
            type="is-danger"
            label="Email"
            :message="GetEmailErrors()"
          >
            <b-input v-model="model.email"></b-input>
          </b-field>

          <b-field v-else label="Email">
            <b-input type="email" v-model="model.email"></b-input>
          </b-field>
          <b-button type="is-primary" expanded native-type="submit">Submit</b-button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { required, email } from "vuelidate/lib/validators";
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
      this.submitStatus = "SUBMITTED";
      console.log(this.$v.model);
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api
          .create("Users/forgot_password", JSON.stringify(this.model.email))
          .then(resp => {
            this.submitStatus = "OK";
            if (resp.status == 400) {
              this.submitStatus = "ERROR";
              this.danger("No account with this login");
              return;
            } else {
              resp.json().then(function() {
                router.push("/");
              });
            }
          }) // Transform the data into json
          .catch(error => {
            this.submitStatus = "ERROR";
            console.log("error", error);
          });
      }
    },
    danger(message) {
      this.$buefy.notification.open({
        duration: 5000,
        message: message,
        type: "is-danger",
        hasIcon: true
      });
    },
    GetEmailErrors() {
      var errors = "";
      if (!this.$v.model.email.email) {
        errors += "Please enter valid email address ";
      }

      if (!this.$v.model.email.required) {
        errors += "Email is required ";
      }

      return errors;
    }
  },
  validations: {
    model: {
      email: {
        required,
        email
      }
    }
  }
};
</script>

<style>
</style>