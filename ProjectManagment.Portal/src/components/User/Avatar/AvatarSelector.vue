<template>
  <div class="avatar-selector">
    <div>
      <input type="file" @change="handleAvatarChange" :accept="formatsString" />
      <p
        class="avatar-selector-instructions"
      >{{formatsString}} files with a size less than {{size}}KB</p>
    </div>
    <avatar :size="200" v-if="value" :src="avatarFileUrl"></avatar>
    <avatar :size="200" v-else-if="urlCheck" :src="url"></avatar>
    <avatar :size="200" v-else src="../../../assets/avatar.png"></avatar>
  </div>
</template>

<script>
import Avatar from "vue-avatar";
export default {
  name: "AvatarSelector",
  components: {
    Avatar
  },
  data() {
    return {
      urlCheck: false
    };
  },
  props: {
    value: Blob,
    formats: Array,
    size: Number,
    url: String
  },
  computed: {
    formatsString: {
      get() {
        return this.formats ? this.formats.join(", ") : "*";
      }
    },
    avatarFileUrl: {
      get() {
        return this.value ? URL.createObjectURL(this.value) : "";
      }
    }
  },
  mounted() {
    this.urlCheck = this.UrlExists(this.url);
  },
  methods: {
    handleAvatarChange(e) {
      // URL.revokeObjectURL(this.value);
      const file = e.target.files[0];
      if (file && this.beforeAvatarUpload(file)) {
        this.$emit("input", file);
      } else {
        this.$emit("input", null);
      }
    },
    beforeAvatarUpload(file) {
      const isformatValid = this.formats.findIndex(f => f === file.type) >= 0;
      const isSizeValid = file.size / 1024 < this.size;
      if (!isformatValid) {
        this.$message.error("Avatar picture has invalid format!");
      }
      if (!isSizeValid) {
        this.$message.error("Avatar picture has invalid size!");
      }
      return isformatValid && isSizeValid;
    },
    UrlExists(url) {
      var http = new XMLHttpRequest();
      http.open("HEAD", url, false);
      http.send();
      return http.status != 404;
    }
  }
};
</script>

<style lang="scss" scoped>
.avatar-selector {
  align-items: center;
  display: flex;
  .avatar-instructions {
    font-style: italic;
    margin: 10px;
  }
}
</style>