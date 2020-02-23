<template>
  <div class="container">
    <h1>Projects</h1>
    <b-button type="is-primary" @click="isCardModalActive=true">Add</b-button>
    <b-modal :active.sync="isCardModalActive" :width="800">
      <div class="card">
        <div class="card-content">
          <AddProjectForm></AddProjectForm>
        </div>
      </div>
    </b-modal>
    <div v-if="projects == null || projects.length == 0">
      <p>Aucun projet</p>
    </div>
    <div v-else>
      <p>{{projects.length}} projects</p>
    </div>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import AddProjectForm from "./AddProjectForm";
var api = new ApiService();

export default {
  name: "Projects",
  components: {
    AddProjectForm
  },
  data() {
    return {
      projects: null,
      isCardModalActive: false
    };
  },
  mounted() {
    api.getData("Projects/User").then(response => {
      response.json().then(data => {
        this.projects = data;
        console.log(this.projects);
      });
    });
  },
  methods: {
    ShowAddProjectModal() {}
  }
};
</script>

<style>
</style>