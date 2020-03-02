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
    <b-table :data="tasks" @click="TaskClicked">
      <template slot-scope="props">
        <b-table-column field="name" label="Name">{{ props.row.name }}</b-table-column>
        <b-table-column field="state" label="State">
          <!-- {{ GetStateLibelle(props.row.state)}} -->
          <b-select
            @input="UpdateStateTask(props.row)"
            v-model="props.row.state"
            placeholder="Select a name"
          >
            <option
              v-for="option in taskStates"
              :value="option.id"
              :key="option.id"
            >{{ option.libelle }}</option>
          </b-select>
        </b-table-column>
      </template>
    </b-table>
    <vs-sidebar
      position-right
      parent="body"
      default-index="1"
      color="primary"
      class="sidebarx"
      spacer
      v-model="active"
    >
     <EditTaskForm></EditTaskForm>
    </vs-sidebar>
  </div>
</template>

<script>
import ApiService from "../../../services/api";
import AddTaskForm from "./AddTaskForm";
import EditTaskForm from "./EditTaskForm";

var api = new ApiService();
export default {
  name: "ProjectTasks",
  components: {
    AddTaskForm,
    EditTaskForm
  },
  data() {
    return {
      active: false,
      idProject: Number,
      tasks: [],
      taskStates: [
        { id: 1, libelle: "To do" },
        { id: 2, libelle: "Done" }
      ],
      isAddTaskModalActive: false
    };
  },
  mounted() {
    this.idProject = this.$route.params.id;
    this.GetTasks();
  },
  methods: {
    TaskAdded() {
      this.isAddTaskModalActive = false;
      this.GetTasks();
    },
    TaskClicked() {
      this.active = true;
    },
    GetTasks() {
      api.getData("Tasks/project/" + this.idProject).then(response => {
        response.json().then(data => {
          this.tasks = data;
        });
      });
    },
    UpdateStateTask(task) {
      console.log(task);
      api
        .update("Tasks/" + task.id + "/state", JSON.stringify(task.state))
        .then(response => {
          console.log(response);
        });
    }
  }
};
</script>

<style>
</style>