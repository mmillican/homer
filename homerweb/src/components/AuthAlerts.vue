<template>
  <b-alert :show="hasAuthenticationStatus" :variant="authenticationStatus.variant">
    {{ authenticationStatus.message }}
  </b-alert>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  mounted () {
    // Clear existing error
    this.$store.dispatch('auth/clearAuthenticationStatus')
  },
  computed: {
    ...mapGetters('auth', [ 'hasAuthenticationStatus', 'authenticationStatus' ])
  }
}
</script>
