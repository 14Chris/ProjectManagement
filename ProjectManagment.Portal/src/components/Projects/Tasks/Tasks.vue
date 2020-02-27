<template>
  <div>
    <h1>Tasks</h1>
    <b-button type="is-primary" @click="isAddTaskModalActive = true">Add</b-button>
    <b-modal :active.sync="isAddTaskModalActive">
      <div class="card">
        <div class="card-content">
          <AddTaskForm v-bind:TaskAdded="TaskAdded"></AddTaskForm>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script>
// import ApiService from "../../services/api";
import AddTaskForm from "./AddTaskForm";

// var api = new ApiService();
export default {
  name: "ProjectTasks",
  components: {
    AddTaskForm
  },
  data() {
    return {
      tasks:null,
      isAddTaskModalActive:false
    };
  },
  methods: {
    TaskAdded() {}
  }
};
</script>

<style>
</style>