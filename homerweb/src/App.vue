<template>
  <div id="app">
    <main-nav />
    <div class="container body">
      <router-view/>
    </div>
  </div>
</template>

<script>
import MainNav from './components/MainNav'

export default {
  components: {
    MainNav
  }
}
</script>

<style lang="scss">
</style>
