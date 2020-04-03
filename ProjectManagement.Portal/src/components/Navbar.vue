<template>
  <!-- Navbar -->
  <b-navbar type="is-primary">
    <template slot="start">
      <b-navbar-item tag="router-link" to="/">ProjectManagement</b-navbar-item>
      <b-navbar-item tag="router-link" to="/">Home</b-navbar-item>
      <b-navbar-item tag="router-link" to="/projects">Projects</b-navbar-item>
    </template>
    <template slot="end">
      <!-- <b-navbar-item> -->
      <b-dropdown aria-role="menu" class position="is-bottom-left">
        <AvatarContainer
          class="navbar-item"
          :size="30"
          :url="profilePictureUrl"
          slot="trigger"
          initial="Chris"
        ></AvatarContainer>

        <b-dropdown-item @click.native.self="GoProfile()" value="profile" aria-role="menuitem">Profil</b-dropdown-item>
        <b-dropdown-item value="settings">Settings</b-dropdown-item>
        <b-dropdown-item @click.native.self="Logout()" value="logout" aria-role="menuitem">Logout</b-dropdown-item>
      </b-dropdown>
      <!-- </b-navbar-item> -->
    </template>
  </b-navbar>
</template>

<script>
import AvatarContainer from "./User/Avatar/Avatar";
import ApiService from "../services/api";

var api = new ApiService();

export default {
  name: "Navbar",
  components: {
    AvatarContainer
  },
  data() {
    return {
      profilePictureUrl: "",
      user: null
    };
  },
  mounted() {
    var _this = this;
    api
      .getData("Users/Session")
      .then(resp => resp.json()) // Transform the data into json
      .then(function(data) {
        _this.user = data;
        _this.profilePictureUrl =
          api.apiUrl + "/Users/" + _this.user.id + "/profile_picture";
      });
  },
  methods: {
    //To logout user of the app
    Logout() {
      localStorage.removeItem("userToken");
      this.$router.push("/login");
    },
    GoProfile(){
       this.$router.push("/profile");
    }
  }
};
</script>

<style>
</style>