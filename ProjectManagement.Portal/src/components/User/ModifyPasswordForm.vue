<template>
  <div>
    <h1>Modify password</h1>
    <form v-on:submit.prevent="ModifyPassword">
      <b-field label="Old password">
        <b-input type="password" v-model="model.oldPassword"></b-input>
      </b-field>
      <div
        class="error"
        v-if="!$v.model.oldPassword.required && submitStatus=='ERROR'"
      >Password is required</div>

      <!-- Password -->
      <b-field
        v-if="$v.model.newPassword.$error && submitStatus=='ERROR'"
        type="is-danger"
        label="¨New password"
        password-reveal
        :message="GetPasswordErrors()"
      >
        <b-input v-model="model.newPassword"></b-input>
      </b-field>

      <b-field v-else label="New password">
        <b-input password-reveal type="password" v-model="model.newPassword"></b-input>
      </b-field>

      <!-- Confirm password -->

      <b-field
        v-if="$v.model.repeatNewPassword.$error && submitStatus=='ERROR'"
        type="is-danger"
        label="Confirm password"
        password-reveal
        :message="GetConfirmPasswordErrors()"
      >
        <b-input v-model="model.repeatNewPassword"></b-input>
      </b-field>

      <b-field v-else label="Confirm new password">
        <b-input password-reveal type="password" v-model="model.repeatNewPassword"></b-input>
      </b-field>
      <b-button type="is-success" native-type="submit">Submit</b-button>
    </form>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import { required, sameAs, minLength } from "vuelidate/lib/validators";
var api = new ApiService();

export default {
  name: "ModifyPasswordForm",
  props: {
    idUser: Number
  },
  data() {
    return {
      submitStatus: "",
      model: {
        id: 0,
        oldPassword: "",
        newPassword: "",
        repeatNewPassword: ""
      }
    };
  },
  mounted() {
    this.model.id = this.idUser;
  },
  methods: {
    ModifyPassword() {
      var _this = this;

      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api
          .create("Users/password/", JSON.stringify(this.model))
          .then(response => {
            if (response.status == 200) {
              _this.submitStatus = "OK";
              this.success("Password modified");
            } else {
              _this.submitStatus = "ERROR";
              response.json().then(async data => {
                this.danger(data);
              });
            }
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
    },
    success(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-success"
      });
    },
    GetPasswordErrors() {
      var errors = "";

      if (!this.$v.model.newPassword.required) {
        errors += "Password is required ";
      }

      if (!this.$v.model.newPassword.minLength) {
        errors +=
          "Password must have at least " +
          this.$v.model.newPassword.$params.minLength.min +
          " letters.";
      }

      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = "";

      if (!this.$v.model.repeatNewPassword.required) {
        errors += "Confirmation password is required ";
      }

      if (!this.$v.model.repeatNewPassword.sameAsPassword) {
        errors += "Confirmation password has to be the same as password ";
      }

      return errors;
    }
  },
  validations: {
    model: {
      oldPassword: {
        required
      },
      newPassword: {
        required,
        minLength: minLength(8)
      },
      repeatNewPassword: {
        required,
        sameAsPassword: sameAs("newPassword")
      }
    }
  }
};
</script>

<style>
</style>