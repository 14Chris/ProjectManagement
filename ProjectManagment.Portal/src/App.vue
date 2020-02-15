<template>
  <!-- <div id="app">
  </div>-->
  <router-view></router-view>
</template>

<script>
import axios from "axios";

export default {
  name: "App",
  components: {},
  data() {
    return {
      apiData: ""
    };
  },
  mounted() {
    var apiUrl = "http://localhost:8000/";
    // process.env.API_URL;

    var config = {
      headers: { "Access-Control-Allow-Origin": "*" }
    };

    axios.get(apiUrl + "WeatherForecast", config).then(response => {
      this.apiData = response.data;
    });
  }
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
