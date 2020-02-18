<template>
  <form v-on:submit.prevent="registerUser" class="ui form">
    <h1>Register</h1>

    <b-field label="First name">
      <b-input v-model="model.first_name"></b-input>
    </b-field>
    <div
      class="error"
      v-if="!$v.model.first_name.required && submitStatus=='ERROR'"
    >First name is required</div>

    <b-field label="Last Name">
      <b-input v-model="model.last_name"></b-input>
    </b-field>
    <div
      class="error"
      v-if="!$v.model.last_name.required && submitStatus=='ERROR'"
    >Last name is required</div>

    <b-field label="Email">
      <b-input v-model="model.email"></b-input>
    </b-field>
    <div class="error" v-if="!$v.model.email.required && submitStatus=='ERROR'">Email is required</div>
    <div
      class="error"
      v-if="!$v.model.email.isUnique && submitStatus=='ERROR'"
    >Email is already taken</div>
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
    <b-button native-type="submit">Register</b-button>
  </form>
</template>

<script>
import ApiService from "../../services/api";
import RegisterUserModel from "../../models/RegisterUserModel";
import { required, sameAs, minLength } from "vuelidate/lib/validators";

var api = new ApiService();

export default {
  name: "Register",
  components: {},
  data() {
    return {
      submitStatus: "",
      model: new RegisterUserModel()
    };
  },
  methods: {
    registerUser: function() {
      this.submitStatus = "SUBMITTED";
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api
          .create("Users", JSON.stringify(this.model))
          .then(response => {
            this.submitStatus = "OK";
            console.log("response", response);
            this.model = new RegisterUserModel();
          })
          .catch(error => {
            console.log("error", error);
          });
      }
    }
  },
  validations: {
    model: {
      first_name: {
        required
      },
      last_name: {
        required
      },
      email: {
        required,
        // simulate async call, fail for all logins with even length
        async isUnique(value) {
          const response = await api.getData("Users/email_exists/" + value);
          if (response.status == 200) {
            return true;
          } else {
            return false;
          }
        }
      },
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
.form {
  margin-left: 100px;
  margin-right: 100px;
}

.error {
  color: red;
}
</style>
