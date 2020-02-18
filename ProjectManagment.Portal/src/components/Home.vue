<template>
  <div>
    <b-navbar>
      <!-- <template slot="brand">
            <b-navbar-item tag="router-link" :to="{ path: '/' }">
                <img
                    src="https://raw.githubusercontent.com/buefy/buefy/dev/static/img/buefy-logo.png"
                    alt="Lightweight UI components for Vue.js based on Bulma"
                >
            </b-navbar-item>
      </template>-->
      <template slot="start">
        <b-navbar-item href="#">Project</b-navbar-item>
      </template>
      <template slot="end">
        <b-dropdown aria-role="menu" class position="is-bottom-left">
          <b-icon class="navbar-item" icon="account" size="is-medium" slot="trigger"></b-icon>

          <b-dropdown-item>
            <router-link :to="{name: '/home/profile'}">Profil</router-link>
          </b-dropdown-item>

          <b-dropdown-item value="settings">
            <b-icon icon="settings"></b-icon>Settings
          </b-dropdown-item>
          <b-dropdown-item value="logout" aria-role="menuitem">
            <b-icon icon="logout"></b-icon>Logout
          </b-dropdown-item>
        </b-dropdown>
      </template>
    </b-navbar>
    <router-view></router-view>
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
    api
      .getData("Users/Session")
      .then(resp => resp.json()) // Transform the data into json
      .then(function(data) {
        console.log("session", data);
      });
  },
  methods: {
    toggle() {
      this.toggleCard = !this.toggleCard;
    }
  }
};
</script>

<style>
</style>
