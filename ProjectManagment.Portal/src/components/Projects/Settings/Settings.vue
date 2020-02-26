<template>
  <div class="container">
    <h1 class="title is-4">Settings</h1>
    <div v-if="project != null">
      <form v-on:submit.prevent="UpdateProject">
        <b-field label="Name">
          <b-input v-model="project.name"></b-input>
        </b-field>
        <b-field label="Description">
          <b-input type="textarea" v-model="project.description"></b-input>
        </b-field>
        <b-button type="is-success" native-type="submit">Add</b-button>
      </form>
    </div>
    <div v-else></div>
  </div>
</template>

<script>
// import { required } from "vuelidate/lib/validators";
import ApiService from "../../../services/api";

var api = new ApiService();
export default {
  name: "ProjectSettings",
  data() {
    return {
      idProject: Number,
      project: null
    };
  },
  mounted() {
    this.idProject = this.$route.params.id;
    this.GetProject();
  },
  methods: {
    GetProject() {
      api.getData("Projects/" + this.idProject).then(response => {
        if (response.status == 200) {
          response.json().then(data => {
            this.project = data;
          });
        } else {
              console.log("error get project")
        }
      });
    },
    UpdateProject() {
      api
        .update("Projects/" + this.idProject, JSON.stringify(this.project))
        .then(response => {
          console.log(response)
          if (response.status == 200) {
            console.log("ok update")
          } 
          else {
            console.log("error update")
          }
        });
    }
  }
};
</script>

<style>
</style>