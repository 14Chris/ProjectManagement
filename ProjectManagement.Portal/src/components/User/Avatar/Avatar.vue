<template>
  <div>
    <avatar v-if="value" class="avatar-picture" :size="size" :src="avatarFileUrl"></avatar>
    <avatar v-else-if="urlCheck" class="avatar-picture" :size="size" :src="url" username="C"></avatar>
    <avatar v-else class="avatar-picture" :size="size" username="C"></avatar>
  </div>
</template>

<script>
import Avatar from "vue-avatar";
export default {
  name: "AvatarContainer",
  components: {
    Avatar
  },
  props: {
    size: Number,
    url: String,
    initial: String,
    value: Blob
  },
  computed: {
    avatarFileUrl: {
      get() {
        return this.value ? URL.createObjectURL(this.value) : "";
      }
    },
    urlCheck: {
      get() {
        if (this.url == null || this.url.length == 0) {
          return false;
        }
        return this.UrlExists(this.url);
      }
    }
  },
  data() {
    return {

    }
  },
  mounted() {},
  methods: {
    UrlExists(url) {
      var http = new XMLHttpRequest();
      http.open("HEAD", url, false);
      http.send();
      return http.status != 404;
    }
  }
};
</script>

<style scoped>
.avatar-picture {
  cursor: pointer;
}
</style>