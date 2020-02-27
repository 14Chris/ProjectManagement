<template>
  <div id="div-home">
    <!-- Navbar -->
    <b-navbar type="is-primary">
      <template slot="start">
        <b-navbar-item tag="router-link" to="/projects">Projects</b-navbar-item>
      </template>
      <template slot="end">
        <b-dropdown aria-role="menu" class position="is-bottom-left">
          <b-icon
            type="is-white"
            class="navbar-item"
            icon="account"
            size="is-medium"
            slot="trigger"
          ></b-icon>
          <b-dropdown-item>
            <router-link to="/profile">Profil</router-link>
          </b-dropdown-item>
          <b-dropdown-item value="settings">Settings</b-dropdown-item>
          <b-dropdown-item @click.native.self="Logout()" value="logout" aria-role="menuitem">Logout</b-dropdown-item>
        </b-dropdown>
      </template>
    </b-navbar>
    <!-- div router -->
    <div id="div-router">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import ApiService from "../services/api";
var api = new ApiService();

export default {
  name: "Home",
  components: {},
  data() {
    return {
      toggleCard: false
    };
  },
  mounted() {
    api.getData("Users/Session").then(resp => resp.json()); // Transform the data into json
  },
  methods: {
    toggle() {
      this.toggleCard = !this.toggleCard;
    },
    //To logout user of the app
    Logout() {
      localStorage.removeItem("userToken");
      this.$router.push("/login");
    }
  }
};
</script>

<style scoped>
#div-home {
  display: flex;
  flex-flow: column;
  height: 100%;
}

#div-router {
  height:calc(100% - 60px);
  /* overflow-y: scroll; */
  /* background-color: blue; */
}
</style>
