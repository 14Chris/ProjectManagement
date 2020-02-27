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
        <b-table-column field="name" label="Name">{{ props.row.name }}</b-table-column>
        <b-table-column field="state" label="State">
          <!-- {{ GetStateLibelle(props.row.state)}} -->
          <b-select @input="UpdateStateTask(props.row)" v-model="props.row.state" placeholder="Select a name">
            <option
              v-for="option in taskStates"
              :value="option.id"
              :key="option.id"
            >{{ option.libelle }}</option>
          </b-select>
        </b-table-column>
      </template>
    </b-table>
  </div>
</template>

<script>
import ApiService from "../../../services/api";
import AddTaskForm from "./AddTaskForm";

var api = new ApiService();
export default {
  name: "ProjectTasks",
  components: {
    AddTaskForm
  },
  data() {
    return {
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
    TaskAdded() {},
    GetTasks() {
      api.getData("Tasks/project/" + this.idProject).then(response => {
        response.json().then(data => {
          this.tasks = data;
        });
      });
    },
    // GetStateLibelle(idState) {
    //   var stateLibelle = "";

    //   switch (idState) {
    //     case 1:
    //       stateLibelle = "To do";
    //       break;
    //     case 2:
    //       stateLibelle = "Done";
    //       break;
    //   }

    //   return stateLibelle;
    // },
    UpdateStateTask(task){
      console.log(task)
      api.update("Tasks/"+task.id+"/state", JSON.stringify(task.state)).then(response=>{
        console.log(response)
      })
    }
  }
};
</script>

<style>
</style>