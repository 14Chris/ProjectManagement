<template>
  <div>
    <div class="ui secondary menu">
      <a class="active item">Home</a>
      <a class="item">Messages</a>
      <a class="item">Friends</a>
      <div class="right menu">
        <div class="item">
          <div class="ui icon input">
            <input type="text" placeholder="Search..." />
            <i class="search link icon"></i>
          </div>
        </div>
        <a class="ui item">Logout</a>
      </div>
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
    return {};
  },
  mounted() {
    api
      .getData("Users/Session")
      .then(resp => resp.json()) // Transform the data into json
      .then(function(data) {
        console.log("session", data)
      });
  },
  methods: {}
};
</script>

<style>
</style>
