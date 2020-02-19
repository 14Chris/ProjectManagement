<template>
    <div class="avatar-photo">
        <img v-if="value" :src="avatarFileUrl" alt="avatar">
        <img v-else src="../../../assets/avatar.png" alt="default-avatar">
    </div>
</template>

<script>
export default {
    name: 'Avatar',
    props: {
        value: Blob
    },
    computed: {
        avatarFileUrl: {
            get() {
                return this.value ? URL.createObjectURL(this.value) : '';
            }
        }
    }
};
</script>

<style lang="scss">
.avatar-photo {
    height: 100px;
    margin: 10px 10px 10px 20px;
    width: 100px;
    img {
        width: 100%;
    }
}
</style>