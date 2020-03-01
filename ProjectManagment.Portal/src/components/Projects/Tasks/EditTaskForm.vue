<template>
  <div>
    <form>
      <div class="centerx default-input">
        <vs-input class="inputx" placeholder="Placeholder" v-model="value1" />
        <vs-input disabled class="inputx" placeholder="Disabled" v-model="value2" />
      </div>
    </form>
  </div>
</template>

<script>
export default {};
</script>

<style>
</style>

<style lang="stylus">
.default-input {
  .inputx {
    margin: 10px;
  }
}
</style>