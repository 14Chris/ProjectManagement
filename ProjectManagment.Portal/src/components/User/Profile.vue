<template>
  <div>
    <div v-if="user != null">
      <h1>Profile</h1>
    </div>
    <div v-else>
      <div class="ui segment">
        <p></p>
        <div class="ui active dimmer">
          <div class="ui loader"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
// import ApiService from "../../services/api";
// var _this = this;
// var api = new ApiService();
export default {
  name: "Profile",
  components: {},
  data() {
    return {
      user: null
    };
  },
  mounted() {},
  methods: {}
};
</script>

<style>
</style>
