<template>
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
</template>

<script>
export default {
    name:"Navbar",
    data() {
    return {
      
    };
  },
  mounted() {
    // api.getData("Users/Session").then(resp => resp.json()); // Transform the data into json
  },
  methods: {
    //To logout user of the app
    Logout() {
      localStorage.removeItem("userToken");
      this.$router.push("/login");
    }
  }
}
</script>

<style>

</style>