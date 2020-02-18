<template>
  <div>
    <div v-if="user != null">
      <h1>Profile</h1>
      

      <b-field label="First name">
        <b-input type="text" v-model="user.first_name"></b-input>
      </b-field>

      <b-field label="Last name">
        <b-input type="text" v-model="user.last_name"></b-input>
      </b-field>

      <b-button native-type="submit">Modify</b-button>
    </div>
    <div v-else>
      <b-loading :is-full-page="isFullPage" :active.sync="isLoading" :can-cancel="true"></b-loading>
    </div>
  </div>
</template>

<script>
import ApiService from "../../services/api";
var api = new ApiService();
export default {
  name: "Profile",
  components: {},
  data() {
    return {
      user: null,
      isFullPage:false,
      isLoading:true
    };
  },
  mounted() {

    var _this = this;
     api
      .getData("Users/Session")
      .then(resp => resp.json()) // Transform the data into json
      .then(function(data) {
        _this.user = data
      });
  },
  methods: {}
};
</script>

<style>
</style>
