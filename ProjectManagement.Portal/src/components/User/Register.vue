<template>
  <div class="container">
    <b-modal :active.sync="isCardModalActive" :width="800">
      <div class="card">
        <div class="card-content">
          <div>
            <h1 class="title">Account created</h1>
            <div class="message">An email has just been sent to you</div>
            <div class="message">Please click on the link in it to activate your account</div>
            <b-button @click="RegistrationSuccess">Ok</b-button>
          </div>
        </div>
      </div>
    </b-modal>
    <div class="card card-form">
      <div class="card-content">
        <p class="title">Register</p>
        <form v-on:submit.prevent="registerUser" class="ui form">
          <!-- First name -->
          <b-field
            v-if="$v.model.first_name.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="First name"
            :message="!$v.model.first_name.required ? 'First name is required' : ''"
          >
            <b-input v-model="model.first_name"></b-input>
          </b-field>

          <b-field v-else label="First name">
            <b-input v-model="model.first_name"></b-input>
          </b-field>

          <!-- Last name -->
          <b-field
            v-if="$v.model.last_name.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="Last Name"
            :message="!$v.model.last_name.required ? 'Last name is required' : ''"
          >
            <b-input v-model="model.last_name"></b-input>
          </b-field>

          <b-field v-else label="Last Name">
            <b-input v-model="model.last_name"></b-input>
          </b-field>

          <!-- Email -->
          <b-field
            v-if="$v.model.email.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="Email"
            :message="GetEmailErrors()"
          >
            <b-input v-model="model.email"></b-input>
          </b-field>

          <b-field v-else label="Email">
            <b-input type="email" v-model="model.email"></b-input>
          </b-field>

          <!-- Password -->
          <b-field
            v-if="$v.model.password.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="Password"
            password-reveal
            :message="GetPasswordErrors()"
          >
            <b-input v-model="model.password"></b-input>
          </b-field>

          <b-field v-else label="Password">
            <b-input password-reveal type="password" v-model="model.password"></b-input>
          </b-field>

          <!-- Confirm password -->

          <b-field
            v-if="$v.model.repeatPassword.$invalid && submitStatus=='ERROR'"
            type="is-danger"
            label="Confirm password"
            password-reveal
            :message="GetConfirmPasswordErrors()"
          >
            <b-input v-model="model.repeatPassword"></b-input>
          </b-field>

          <b-field v-else label="Confirm password">
            <b-input password-reveal type="password" v-model="model.repeatPassword"></b-input>
          </b-field>
          <b-button type="is-primary" expanded native-type="submit">Register</b-button>
          <div>
            <span>You already have an account ?</span>
            <b-button id="btn-login" tag="router-link" to="/login" type="is-text">Login</b-button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import RegisterUserModel from "../../models/RegisterUserModel";
import { required, sameAs, minLength, email } from "vuelidate/lib/validators";

var api = new ApiService();

export default {
  name: "Register",
  components: {},
  data() {
    return {
      submitStatus: "",
      model: new RegisterUserModel(),
      isCardModalActive: false,
      chkUsernameAvailabilityTimer: null
    };
  },
  methods: {
    registerUser: function() {
      this.submitStatus = "SUBMITTED";
      console.log(this.$v.model);
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api
          .create("Users", JSON.stringify(this.model))
          .then(response => {
            this.submitStatus = "OK";
            if (response.status == 201) {
              this.isCardModalActive = true;
              this.model = new RegisterUserModel();
            } else {
              response.json().then(data => {
                this.submitStatus = "ERROR";
                this.danger(data);
              });
            }
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    GetEmailErrors() {
      var errors = [];

      if (!this.$v.model.email.required) {
        errors.push("Email is required");
      } else {
        if (!this.$v.model.email.email) {
          errors.push("Please enter valid email address");
        }

        if (!this.$v.model.email.isUnique) {
          errors.push("Email is already taken");
        }
      }

      return errors;
    },
    GetPasswordErrors() {
      var errors = [];

      if (!this.$v.model.password.required) {
        errors.push("Password is required");
      } else {
        if (!this.$v.model.password.minLength) {
          errors.push(
            "Password must have at least " +
              this.$v.model.password.$params.minLength.min +
              " letters."
          );
        }

        if (!this.$v.model.password.oneNumber) {
          errors.push("Password must have at least one number");
        }
        if (!this.$v.model.password.oneUpperCase) {
          errors.push("Password must have at least one upper case character");
        }
        if (!this.$v.model.password.oneLowerCase) {
          errors.push("Password must have at least one lower case character");
        }
      }

      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];

      if (!this.$v.model.repeatPassword.required) {
        errors.push("Confirmation password is required");
      } else if (!this.$v.model.repeatPassword.sameAsPassword) {
        errors.push("Confirmation password has to be the same as password");
      }

      return errors;
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
    },
    RegistrationSuccess() {
      this.isCardModalActive = false;
      this.$router.push("/login");
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
        email,
        // simulate async call, fail for all logins with even length
        async isUnique(value) {
          if (value == null || value == "") return true;

          if (this.chkUsernameAvailabilityTimer) {
            clearTimeout(this.chkUsernameAvailabilityTimer);
            this.chkUsernameAvailabilityTimer = null;
          }
          this.chkUsernameAvailabilityTimer = setTimeout(() => {
            api.getData("Users/email_exists/" + value).then(response => {
              if (response.status == 200) {
                return true;
              } else {
                return false;
              }
            });
          }, 500);
        }
      },
      password: {
        required,
        minLength: minLength(8),
        oneNumber(password) {
          return /(?=.*\d)/.test(password);
        },
        oneUpperCase(password) {
          return /(?=.*[A-Z])/.test(password);
        },
        oneLowerCase(password) {
          return /(?=.*[a-z])/.test(password);
        }
      },
      repeatPassword: {
        required,
        sameAsPassword: sameAs("password")
      }
    }
  }
};
</script>

<style scoped>
.message {
  margin-bottom: 10px;
}

#btn-login {
  vertical-align: middle;
}
</style>

<style>
.form {
  margin-left: 100px;
  margin-right: 100px;
}

.error {
  color: red;
}
</style>