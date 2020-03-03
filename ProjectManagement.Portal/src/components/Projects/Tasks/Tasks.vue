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
            disabled
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
        <b-table-column field="delete" label="Delete">
          <button class="button is-danger" @click="DeleteTask(props.row.id, $event)">
            <b-icon icon="delete" size="is-small"></b-icon>
          </button>
        </b-table-column>
      </template>
    </b-table>
    <!-- <EditTaskSideBar></EditTaskSideBar> -->
    <b-modal :active.sync="isEditTaskModalActive">
      <div class="card">
        <div class="card-content">
          <EditTaskForm :taskId="currentTaskId" v-bind:TaskUpdated="TaskUpdated"></EditTaskForm>
        </div>
      </div>
    </b-modal>
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
      idProject: Number,
      tasks: [],
      taskStates: [
        { id: 1, libelle: "To do" },
        { id: 2, libelle: "Done" }
      ],
      isAddTaskModalActive: false,
      isEditTaskModalActive: false,
      currentTaskId: null
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
    TaskClicked(row) {
      this.isEditTaskModalActive = true;
      this.currentTaskId = row.id;
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
    },
    TaskUpdated() {
      this.isEditTaskModalActive = false;
      this.GetTasks();
    },
    DeleteTask(id, event) {
      event.stopPropagation();

      api.delete("Tasks/" + id).then(response => {
        if (response.status == 200) {
          this.GetTasks();
        }
      });
    }
  }
};
</script>

<style scoped>
#side-menu {
  height: 100%;
  position: absolute;
  width: 300px;
}
</style>