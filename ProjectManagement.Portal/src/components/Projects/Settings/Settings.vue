<template>
  <div class="container">
    <h1 class="title is-4">Settings</h1>
    <div v-if="project != null">
      <form v-on:submit.prevent="UpdateProject">
        <b-field label="Name">
          <b-input v-model="project.name"></b-input>
        </b-field>
        <div
          class="error"
          v-if="!$v.project.name.required && submitStatus=='ERROR'"
        >Name is required</div>
        <b-field label="Description">
          <b-input type="textarea" v-model="project.description"></b-input>
        </b-field>
        <b-button type="is-primary" native-type="submit">Save</b-button>
      </form>
      <b-button type="is-danger" v-on:click="isRemoveModalActive = true">Remove</b-button>
      <b-modal :active.sync="isRemoveModalActive">
        <div class="card">
          <div class="card-content">
            <RemoveProjectForm :projectId="project.id" :projectName="project.name"></RemoveProjectForm>
          </div>
        </div>
      </b-modal>
    </div>
    <div v-else></div>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "../../../services/api";
import RemoveProjectForm from "../RemoveProjectForm";

var api = new ApiService();
export default {
  name: "ProjectSettings",
  components: { RemoveProjectForm },
  props: ["UpdateProjectName"],
  data() {
    return {
      isRemoveModalActive:false,
      submitStatus: "",
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
          console.log("error get project");
        }
      });
    },
    UpdateProject() {
      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api
          .update("Projects/" + this.idProject, JSON.stringify(this.project))
          .then(response => {
            if (response.status == 204) {
              this.submitStatus = "OK";
              this.$emit(
                "updateProject",
                JSON.parse(JSON.stringify(this.project))
              );
              this.success("Project modified");
            } else {
              this.submitStatus = "ERROR";
              this.danger("Error while modifying project");
            }
          });
      }
    },
    success(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-success"
      });
    },
    danger(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-danger"
      });
    }
  },
  validations: {
    project: {
      name: {
        required
      }
    }
  }
};
</script>

<style>
</style>