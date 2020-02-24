<template>
  <div>
    <b-loading v-if="project == null" :is-full-page="false" :can-cancel="true"></b-loading>
    <div v-else>
      <div class="header-detail">
        <h2 class="title is-2">{{project.name}}</h2>
        <!-- <b-button type="is-danger" v-on:click="isRemoveModalActive = true">Remove</b-button> -->
        <b-modal :active.sync="isRemoveModalActive">
          <div class="card">
            <div class="card-content">
              <RemoveProjectForm :projectId="project.id" :projectName="project.name"></RemoveProjectForm>
            </div>
          </div>
        </b-modal>
        <!-- <b-field label>
          <label class="label">{{project.creation_date}}</label>
        </b-field>
        <b-field label>
          <label class="label">{{project.creator.first_name + " " + project.creator.last_name}}</label>
        </b-field> -->
      </div>
      <div class="content-detail">
        <b-menu class="side-menu">
          <b-menu-list label>
            <b-menu-item
              label="Dashboard"
              :active='this.$route.path.split("/")[3] == "dashboard"'
              @click="DashboardClick"
            ></b-menu-item>
            <b-menu-item
              label="Tasks"
              :active='this.$route.path.split("/")[3] == "tasks"'
              @click="TasksClick"
            ></b-menu-item>
            <b-menu-item
              label="Calendar"
              :active='this.$route.path.split("/")[3] == "calendar"'
              @click="CalendarClick"
            ></b-menu-item>
            <b-menu-item
              label="Documents"
              :active='this.$route.path.split("/")[3] == "documents"'
              @click="DocumentsClick"
            ></b-menu-item>
            <b-menu-item
              label="Settings"
              :active='this.$route.path.split("/")[3] == "settings"'
              @click="SettingsClick"
            ></b-menu-item>
          </b-menu-list>
        </b-menu>
        <div class="container">
          <router-view></router-view>
        </div>
      </div>
    </div>
  </div>
</template>
ù
<script>
import ApiService from "../../services/api";
import RemoveProjectForm from "./RemoveProjectForm";
var api = new ApiService();

export default {
  name: "ProjectDetail",
  components: {
    RemoveProjectForm
  },
  data() {
    return {
      project: null,
      isRemoveModalActive: false
    };
  },
  mounted() {
    console.log(this.$route.path.split("/"));
    api.getData("Projects/" + this.$route.params.id).then(response => {
      if (response.status == 200) {
        response.json().then(data => {
          console.log("data", data);
          this.project = data;
        });
      }
    });
  },
  methods: {
    DashboardClick() {
      this.$router.push("/projects/" + this.project.id + "/dashboard");
    },
    TasksClick() {
      this.$router.push("/projects/" + this.project.id + "/tasks");
    },
    CalendarClick() {
      this.$router.push("/projects/" + this.project.id + "/calendar");
    },
    DocumentsClick() {
      this.$router.push("/projects/" + this.project.id + "/documents");
    },
    SettingsClick() {
      this.$router.push("/projects/" + this.project.id + "/settings");
    }
  }
};
</script>

<style scoped>
.header-detail {
  display: flex;
  flex-direction: row;
  margin:0 auto;
  width: fit-content;
}

.side-menu {
  width: 100px;
}

.content-detail {
  display: flex;
  flex-direction: row;
}
</style>