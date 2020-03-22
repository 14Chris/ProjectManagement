<template>
  <div id="div-home">
    <Navbar></Navbar>
    <!-- div router -->
    <div id="div-router">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
// import ApiService from "../services/api";
import Navbar from "./Navbar";
// var api = new ApiService();

export default {
  name: "Home",
  components: {Navbar},
  data() {
    return {
      toggleCard: false
    };
  },
  mounted() {
    // api.getData("Users/Session").then(resp => resp.json()); // Transform the data into json
  },
  methods: {
    toggle() {
      this.toggleCard = !this.toggleCard;
    },
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
  flex:1;
  /* overflow-y: scroll; */
  /* background-color: blue; */
}
</style>
