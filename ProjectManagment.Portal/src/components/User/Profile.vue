<template>
  <div class="container">
    <div v-if="user != null">
      <form v-on:submit.prevent="updateUser">
        <h1>Profile</h1>
        {{file != null ? file.getAsBinary() : ""}}
        <AvatarSelector v-model="file" :formats="formats" :size="sizeKB" />

        <b-field label="First name">
          <b-input type="text" v-model="user.first_name"></b-input>
        </b-field>

        <b-field label="Last name">
          <b-input type="text" v-model="user.last_name"></b-input>
        </b-field>

        <b-button native-type="submit">Modify</b-button>
      </form>
    </div>
    <div v-else>
      <b-loading :is-full-page="isFullPage" :active.sync="isLoading" :can-cancel="true"></b-loading>
    </div>
  </div>
</template>

<script>
import ApiService from "../../services/api";
// import Avatar from "vue-avatar";
import AvatarSelector from "./Avatar/AvatarSelector"

var api = new ApiService();

export default {
  name: "Profile",
  components: {
    AvatarSelector
  },
  data() {
    return {
      user: null,
      isFullPage: false,
      isLoading: true,
      file: null,
      formats: ["image/jpg", "image/jpeg", "image/png"],
      sizeKB: 1000
    };
  },
  mounted() {
    var _this = this;
    api
      .getData("Users/Session")
      .then(resp => resp.json()) // Transform the data into json
      .then(function(data) {
        _this.user = data;
      });
  },
  methods: {
    updateUser() {
      // var _this = this;
      api
        .update("Users/" + this.user.id, JSON.stringify(this.user))
        .then(resp => {
          if (resp.status == 204) {
            this.success("Change made");
          } else {
            this.danger("Error during modification");
          }
        }); // Transform the data into json
    },
    success(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-success"
      });
    },
    danger(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-danger"
      });
    }
  }
};
</script>

<style>
</style>
