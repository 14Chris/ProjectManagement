<template>
  <div class="container">
    <form v-on:submit.prevent="DeleteProject">
      <div>
        This action cannot be undone. This will permanently delete the {{projectName}} project.
      </div>
      <div>
          Please type <b>{{projectName}}</b> to confirm.
      </div>

      <b-field label="">
        <b-input v-model="model.repeatName"></b-input>
      </b-field>

      <b-button v-if="$v.model.repeatName.sameAsName" type="is-danger" native-type="submit">I understand the consequences, delete this project</b-button>
      <b-button disabled v-else type="is-danger" v-on:click="isRemoveModalActive = true">I understand the consequences, delete this project</b-button>
    </form>
  </div>
</template>

<script>
import { sameAs } from "vuelidate/lib/validators";
import ApiService from "../../services/api";

var api = new ApiService();
export default {
  name: "RemoveProjectForm",
  components: {},
  props: ["projectId", "projectName"],
  data() {
    return {
      submitStatus: "",
      model: {
        name: "",
        repeatName: ""
      }
    };
  },
  mounted() {
    console.log(this.projectName);
    this.model.name = this.projectName;
  },
  methods: {
    DeleteProject() {
      this.submitStatus = "SUBMITTED";
      if (this.$v.$invalid) {
        this.submitStatus = "ERROR";
      } else {
        this.submitStatus = "PENDING";
        api.delete("Projects/" + this.projectId).then(reponse => {
          console.log("response", reponse);
          //If created
          if (reponse.status == 200) {
            this.submitStatus = "OK";
            this.$router.push("/projects");
          } else {
            this.submitStatus = "ERROR";
            console.log("Remove project: Error");
          }
        });
      }
    }
  },
  validations: {
    model: {
      name: {},
      repeatName: {
        sameAsName: sameAs("name")
      }
    }
  }
};
</script>

<style>
</style>