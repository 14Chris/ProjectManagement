<template>
  <div>
    <b-button icon-left="plus" type="is-light" @click="isAddTaskModalActive = true">Add New Task</b-button>
    <b-modal :active.sync="isAddTaskModalActive">
      <div class="card">
        <div class="card-content">
          <AddTaskForm v-bind:TaskAdded="TaskAdded"></AddTaskForm>
        </div>
      </div>
    </b-modal>
    <b-table @click="TaskClicked" :data="tasks" @contextmenu="TaskContextMenu">
      <template slot-scope="props">
        <b-table-column field="name" label="Name">{{ props.row.name }}</b-table-column>
        <b-table-column field="description" label="Description">{{ props.row.description }}</b-table-column>
        <b-table-column field="state" label="State">
          {{ props.row.state }}
        </b-table-column>
        <b-table-column field="assignedto" label="Assigned to"></b-table-column>
        <b-table-column field="priority" label="Priority">{{ props.row.priority }}</b-table-column>
      </template>
    </b-table>
    <!-- Context menu for tasks table -->
    <vue-context ref="menu">
      <template slot-scope="child">
        <li>
          <b-button @click="DeleteTask(child.data.taskId)" icon-left="delete" type="is-light">Delete</b-button>
        </li>
      </template>
    </vue-context>
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
//Context menu
import VueContext from "vue-context";
import "vue-context/src/sass/vue-context.scss";

import ApiService from "../../../services/api";
import AddTaskForm from "./AddTaskForm";
import EditTaskForm from "./EditTaskForm";

var api = new ApiService();
export default {
  name: "ProjectTasks",
  components: {
    AddTaskForm,
    EditTaskForm,
    VueContext
  },
  data() {
    return {
      idProject: Number,
      tasks: [],
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
    TaskContextMenu(row, contextMenuNativeEvent) {
      contextMenuNativeEvent.preventDefault();
      this.$refs.menu.open(contextMenuNativeEvent, { taskId: row.id });
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
    DeleteTask(id) {
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