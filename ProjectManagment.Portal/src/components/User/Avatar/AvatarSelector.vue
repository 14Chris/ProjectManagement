<template>
  <div>
    <div v-if="edit" class="avatar-selector">
      <div class="avatar-container">
        <avatar class="avatar-picture" :size="200" v-if="value" :src="avatarFileUrl"></avatar>
        <avatar class="avatar-picture" :size="200" v-else-if="urlCheck" :src="url"></avatar>
        <avatar class="avatar-picture" :size="200" v-else src="../../../assets/avatar.png"></avatar>
        <b-field class="change-picture-btn">
          <b-upload v-model="file">
            <a class="button is-text">
              <b-icon icon="upload"></b-icon>
            </a>
          </b-upload>
        </b-field>
      </div>
    </div>
    <div v-else class="avatar-selector">
      <div class="avatar-container">
        <avatar class="avatar-picture" :size="200" v-if="value" :src="avatarFileUrl"></avatar>
        <avatar class="avatar-picture" :size="200" v-else-if="urlCheck" :src="url"></avatar>
        <avatar class="avatar-picture" :size="200" v-else src="../../../assets/avatar.png"></avatar>
      </div>
    </div>
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
    url: String,
    edit: Boolean
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
  height: 200px;
  .avatar-instructions {
    font-style: italic;
    margin: 10px;
  }
}

.avatar-container {
  height: fit-content;
  width: fit-content;
}
.avatar-picture {
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
}
.change-picture-btn {
  width: 100%;
  height: 100%;
  position: absolute;
  top: 80px;
  left: 80px;

  // z-index: 10;
}
</style>