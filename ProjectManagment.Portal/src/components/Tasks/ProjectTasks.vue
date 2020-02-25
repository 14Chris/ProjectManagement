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
    <b-table :data="tasks">
      <template slot-scope="props">
        <b-table-column field="id" label="ID">{{ props.row.id }}</b-table-column>

        <b-table-column field="name" label="Name">{{ props.row.name }}</b-table-column>

        <b-table-column field="state" label="State">
          <b-select placeholder="Select a name">
            <option
              v-for="option in taskState"
              :value="option.id"
              :key="option.id"
              :v-model="props.row.state"
            >{{ option.label }}</option>
          </b-select>
        </b-table-column>
      </template>
    </b-table>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import AddTaskForm from "./AddTaskForm";

var api = new ApiService();
export default {
  name: "ProjectTasks",
  components: {
    AddTaskForm
  },
  mounted() {
    this.idProject = this.$route.params.id;
    this.GetTasks();
    this.GetTaskStates();
  },
  data() {
    return {
      tasks: [],
      taskState: [],
      isAddTaskModalActive: false,
      idProject: Number
    };
  },
  methods: {
    GetTasks() {
      api.getData("Tasks/project/" + this.idProject).then(response => {
        if (response.status == 200) {
          response.json().then(data => {
            this.tasks = data;
          });
        }
      });
    },
    GetTaskStates() {
      this.taskState.push({
        id: 1,
        label: "To Do"
      });

      this.taskState.push({
        id: 2,
        label: "Done"
      });
    },
    TaskAdded() {
      this.isAddTaskModalActive = false;
      this.GetTasks();
    },
    StateLabel(state) {
      var stateLabel = "";

      switch (state) {
        case 1:
          stateLabel = "To Do";
          break;
        case 2:
          stateLabel = "Done";
          break;
      }

      return stateLabel;
    }
  }
};
</script>

<style>
</style>