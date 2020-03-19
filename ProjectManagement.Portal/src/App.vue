<template>
  <!-- <div id="app">
  </div>-->
  <router-view></router-view>
</template>

<script>
export default {
  name: "App",
  components: {},
  data() {
    return {
      apiData: ""
    };
  },
  mounted() {}
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

.container {
  align-content: center;
  height: 100%;
}

.card {
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  width: 50%;
}
</style>

<style lang="scss">
@import "./buefy-config.scss";
</style>
