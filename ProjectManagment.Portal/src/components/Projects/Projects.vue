<template>
  <div class="container">
    <h1>Projects</h1>
    <b-button type="is-primary" @click="isCardModalActive=true">Add</b-button>
    <b-modal :active.sync="isCardModalActive" :width="800">
      <div class="card">
        <div class="card-content">
          <AddProjectForm v-bind:ProjectAdded="ProjectAdded"></AddProjectForm>
        </div>
      </div>
    </b-modal>
    <div v-if="projects == null || projects.length == 0">
      <p>Aucun projet</p>
    </div>
    <div v-else>
      <p>{{projects.length}} projects</p>
      <b-table :data="projects" @click="RowCliked">
        <template slot-scope="props">
          <b-table-column field="name" label="Name">{{ props.row.name }}</b-table-column>

          <b-table-column field="description" label="Description">{{ props.row.description }}</b-table-column>
          <b-table-column field="creation_date" label="Creation date">{{ props.row.creation_date }}</b-table-column>
          <b-table-column
            field="creator"
            label="Creator"
          >{{ props.row.creator.first_name + " " + props.row.creator.last_name }}</b-table-column>
        </template>
      </b-table>
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
    ShowAddProjectModal() {},
    RowCliked(row) {
      // var router = this.$router;
      this.$router.push("/projects/" + row.id);
      console.log(row);
    },
    ProjectAdded() {
      this.isCardModalActive = false;
      api.getData("Projects/User").then(response => {
        response.json().then(data => {
          this.projects = data;
          console.log(this.projects);
        });
      });
    }
  }
};
</script>

<style scoped>
  
</style>