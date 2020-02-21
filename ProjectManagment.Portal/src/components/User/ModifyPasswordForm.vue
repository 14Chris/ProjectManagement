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
      <b-field label="New password">
        <b-input type="password" v-model="model.newPassword"></b-input>
      </b-field>
      <div
        class="error"
        v-if="!$v.model.newPassword.required && submitStatus=='ERROR'"
      >Password is required</div>
      <div
        class="error"
        v-if="!$v.model.newPassword.minLength && submitStatus=='ERROR'"
      >Password must have at least {{$v.model.newPassword.$params.minLength.min}} letters.</div>
      <b-field label="Confirm new password">
        <b-input type="password" v-model="model.repeatNewPassword"></b-input>
      </b-field>
      <div
        class="error"
        v-if="!$v.model.repeatNewPassword.sameAsPassword && submitStatus=='ERROR'"
      >Confirmation password has to be the same as new password</div>
      <div
        class="error"
        v-if="!$v.model.repeatNewPassword.required && submitStatus=='ERROR'"
      >Confirmation password is required</div>
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