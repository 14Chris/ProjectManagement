<template>
  <div class="container">
    <form v-on:submit.prevent="AddTask">
      <h3 class="title is-3">Add task</h3>
      <b-field label="Name">
        <b-input v-model="model.name"></b-input>
      </b-field>
      <div class="error" v-if="!$v.model.name.required && submitStatus=='ERROR'">Name is required</div>

      <b-field label="Description">
        <b-input maxlength="150" type="textarea"></b-input>
      </b-field>
   
      <b-button type="is-primary" native-type="submit" expanded>Add</b-button>
    </form>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "../../../services/api";

var api = new ApiService();
export default {
  name: "AddTaskForm",
  components: {},
  props: ["TaskAdded"],
  data() {
    return {
      submitStatus: "",
      model: {
        name: "",
        idProject: Number
      }
    };
  },
  mounted() {
    this.model.idProject = this.$route.params.id;
  },
  methods: {
    AddTask() {
      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api.create("Tasks", JSON.stringify(this.model)).then(reponse => {
          //If created
          if (reponse.status == 201) {
            this.submitStatus = "OK";
            this.TaskAdded();
          } else {
            this.submitStatus = "ERROR";
          }
        });
      }
    }
  },
  validations: {
    model: {
      name: {
        required
      }
    }
  }
};
</script>

<style>
</style>