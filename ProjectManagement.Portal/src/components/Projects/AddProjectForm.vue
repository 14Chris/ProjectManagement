<template>
  <div class="container">
    <h1 class="title">Add project</h1>
    <form v-on:submit.prevent="AddProject">
      <b-field label="Name">
        <b-input type="text" v-model="model.name"></b-input>
      </b-field>
      <div class="error" v-if="!$v.model.name.required && submitStatus=='ERROR'">Name is required</div>
      <b-button type="is-success" native-type="submit">Add</b-button>
    </form>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "../../services/api";

var api = new ApiService();
export default {
  name: "AddProjectForm",
  props:['ProjectAdded'],
  data() {
    return {
      model: {
        name: ""
      },
      submitStatus: ""
    };
  },
  validations: {
    model: {
      name: {
        required
      }
    }
  },
  methods: {
    AddProject() {
      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api.create("Projects", JSON.stringify(this.model))
        .then(reponse => {
          console.log("response", reponse)
          //If created
          if (reponse.status == 201) {
            this.submitStatus = "OK";
            console.log("Add project: Ok");
            this.ProjectAdded()
          } else {
            this.submitStatus = "ERROR";
            console.log("Add project: Error");
          }
        });
      }
    }
  }
};
</script>

<style>
</style>