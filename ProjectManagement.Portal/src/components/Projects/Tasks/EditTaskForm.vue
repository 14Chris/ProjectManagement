<template>
  <div>
    <form v-on:submit.prevent="UpdateTask">
      <h1>Edit task</h1>
      <b-field label="Name">
        <b-input v-model="task.name"></b-input>
      </b-field>
      <div class="error" v-if="!$v.task.name.required && submitStatus=='ERROR'">Name is required</div>
      <b-field label="State">
        <b-select v-model="task.state" placeholder="Select a state">
          <option
            v-for="option in taskStates"
            :value="option.id"
            :key="option.id"
          >{{ option.libelle }}</option>
        </b-select>
      </b-field>

      <!-- <b-loading v-if='submitStatus == "SUBMITTED"' :is-full-page="isFullPage" :can-cancel="true"></b-loading> -->

      <b-button type="is-success" native-type="submit">Edit</b-button>
    </form>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "../../../services/api";

var api = new ApiService();
export default {
  name: "EditTaskForm",
  props: ["taskId", "TaskUpdated"],
  mounted() {
    api.getData("Tasks/" + this.taskId).then(response => {
      response.json().then(data => {
        this.task = data;
      });
    });
  },
  data() {
    return {
      task: null,
      submitStatus: "",
      taskStates: [
        { id: 1, libelle: "To do" },
        { id: 2, libelle: "Done" }
      ]
    };
  },
  methods: {
    UpdateTask() {
      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api.update("Tasks/"+this.task.id, JSON.stringify(this.task)).then(response => {
          if (response.status == 204) {
            this.submitStatus = "OK";
            this.TaskUpdated();
          } else {
            this.submitStatus = "ERROR";
          }
        });
      }
    }
  },
  validations: {
    task: {
      name: {
        required
      }
    }
  }
};
</script>

<style>
</style>