<template>
  <div id="app">
    <main-nav />
    <div class="container body">
      <alerts />

      <!-- TODO: Slot for title -->

      <router-view />
    </div>
  </div>
</template>

<script>
import MainNav from './components/MainNav'
import Alerts from './components/Alerts'

export default {
  components: {
    MainNav,
    Alerts
  }
}
</script>

<style lang="scss">
</style>
