<template>
  <div>
    <form v-on:submit.prevent="UpdateTask">
      <h3 class="title is-3">Edit task</h3>
      <b-field label="Name">
        <b-input v-model="task.name"></b-input>
      </b-field>
      <div class="error" v-if="!$v.task.name.required && submitStatus=='ERROR'">Name is required</div>

      <b-field label="Description">
        <b-input v-model="task.description" maxlength="150" type="textarea"></b-input>
      </b-field>

      <b-field label="State">
        <b-select v-model="task.state" placeholder="Select a state" expanded>
          <option
            v-for="option in taskStates"
            :value="option.id"
            :key="option.id"
          >{{ option.libelle }}</option>
        </b-select>
      </b-field>

       <b-field label="Priority">
        <b-select v-model="task.priority" placeholder="Select a priority" expanded>
          <option
            v-for="option in taskPriority"
            :value="option.id"
            :key="option.id"
          >{{ option.libelle }}</option>
        </b-select>
      </b-field>

      <b-field label="Due date">
        <b-datepicker v-model="task.due_date"
                ref="datepicker"
                expanded
                placeholder="Select a date">
            </b-datepicker>
            <b-button
                @click="$refs.datepicker.toggle()"
                icon-left="calendar-today"
                type="is-primary" />
      </b-field>


      <b-button type="is-primary" native-type="submit" expanded>Edit</b-button>
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
      ], 
       taskPriority: [
        { id: 1, libelle: "Normal" },
        { id: 2, libelle: "High" },
        { id: 3, libelle: "Immediate" }
      ], 
    };
  },
  methods: {
    UpdateTask() {
      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api
          .update("Tasks/" + this.task.id, JSON.stringify(this.task))
          .then(response => {
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