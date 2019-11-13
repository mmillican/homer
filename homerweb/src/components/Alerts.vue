<template>
  <div v-if="hasAlerts" class="alerts-wrapper">
    <!-- TODO: Could refactor this to only one loop -->
    <div v-for="alert in getByType('success')" v-bind:key="alert.message" class="alert alert-success">
      {{ alert.message }}
    </div>
    <div v-for="alert in getByType('error')" v-bind:key="alert.message" class="alert alert-danger">
      {{ alert.message }}
    </div>
    <div v-for="alert in getByType('warning')" v-bind:key="alert.message" class="alert alert-warning">
      {{ alert.message }}
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  computed: {
    ...mapGetters('alerts', ['getByType', 'hasAlerts'])
  }
}
</script>
