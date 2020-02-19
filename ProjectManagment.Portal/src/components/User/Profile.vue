<template>
  <div class="container">
    <div v-if="user != null">
      <div v-if="edit">
        <form v-on:submit.prevent="updateUser">
          <AvatarSelector
            v-model="file"
            :formats="formats"
            :size="sizeKB"
            :url="profilePictureUrl"
            :edit="edit"
          />

          <b-field label="First name">
            <b-input type="text" v-model="user.first_name"></b-input>
          </b-field>

          <b-field label="Last name">
            <b-input type="text" v-model="user.last_name"></b-input>
          </b-field>
          <b-field label="Email">
            <h3>{{user.email}}</h3>
          </b-field>

          <b-button type="is-success" native-type="submit">Submit</b-button>
          <b-button type="is-danger" @click="edit=false">Cancel</b-button>
        </form>
      </div>
      <div v-else>
        <AvatarSelector
          v-model="file"
          :formats="formats"
          :size="sizeKB"
          :url="profilePictureUrl"
          :edit="edit"
        />
        <b-field label="First name">
          <h3>{{user.first_name}}</h3>
        </b-field>
        <b-field label="Last name">
          <h3>{{user.last_name}}</h3>
        </b-field>
        <b-field label="Email">
          <h3>{{user.email}}</h3>
        </b-field>
        <b-button type="is-primary" @click="edit=true">Modify</b-button>
      </div>
    </div>
    <div v-else>
      <b-loading :is-full-page="isFullPage" :active.sync="isLoading" :can-cancel="true"></b-loading>
    </div>
  </div>
</template>

<script>
import ApiService from "../../services/api";
import AvatarSelector from "./Avatar/AvatarSelector";

var api = new ApiService();

export default {
  name: "Profile",
  components: {
    AvatarSelector
  },
  data() {
    return {
      user: null,
      isFullPage: false,
      isLoading: true,
      file: null,
      formats: ["image/jpg", "image/jpeg", "image/png"],
      sizeKB: 5000,
      profilePictureUrl: "",
      edit: false
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
    updateUser() {
      var _this = this;
      if (this.file != null) {
        var reader = new FileReader();
        reader.readAsDataURL(this.file);
        reader.onload = function() {
          _this.user.profile_picture = reader.result.split(",")[1];

          api
            .update("Users/" + _this.user.id, JSON.stringify(_this.user))
            .then(resp => {
              if (resp.status == 204) {
                _this.success("Change made");
              } else {
                _this.danger("Error during modification");
              }
            });
        };
      } else {
        api
          .update("Users/" + _this.user.id, JSON.stringify(_this.user))
          .then(resp => {
            if (resp.status == 204) {
              _this.success("Change made");
            } else {
              _this.danger("Error during modification");
            }
          });
      }
    },
    success(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-success"
      });
    },
    danger(message) {
      this.$buefy.toast.open({
        duration: 5000,
        message: message,
        position: "is-bottom",
        type: "is-danger"
      });
    },
    SetModifyMode() {
      this.edit = true;
    },
    CancelModifyMode() {
      this.edit = false;
    }
  }
};
</script>

<style>
</style>
