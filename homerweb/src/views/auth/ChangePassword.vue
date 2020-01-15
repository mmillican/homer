<template>
  <div>
    <b-row class="justify-content-md-center">
      <b-col cols="12" md="6">
        <div class="b-form-1">
          <h1>Change Password</h1>

          <auth-alerts />

          <b-form @submit.prevent="updatePassword">
            <b-form-group label="Current password" label-for="currentPasswordInput" :label-sr-only="true">
              <b-form-input
                id="currentPasswordInput"
                type="password"
                v-model="currentPassword"
                required
                placeholder="Current password"
              />
            </b-form-group>
            <b-form-group label="New Password" label-for="newPasswordInput" :label-sr-only="true">
              <b-form-input
                id="newPasswordInput"
                type="password"
                v-model="newPassword"
                required
                placeholder="New password"
              />
            </b-form-group>
            <b-form-group label="Confirm New Password" label-for="confirmNewPasswordInput" :label-sr-only="true">
              <b-form-input
                id="confirmNewPasswordInput"
                type="password"
                v-model="confirmNewPassword"
                required
                placeholder="Confirm new password"
              />
            </b-form-group>
            <b-button type="submit" variant="primary">Change password</b-button>
          </b-form>
        </div>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import AuthAlerts from '../../components/AuthAlerts'
export default {
  components: {
    AuthAlerts
  },
  data () {
    return {
      currentPassword: null,
      newPassword: null,
      confirmNewPassword: null
    }
  },
  methods: {
    async updatePassword () {
      // if (this.confirmNewPassword !== this.newPassword) {
      //   alert('Confirmation password does not match') // TODO: show a nicer alert
      //   return
      // }
      let params = {
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
        confirmNewPassword: this.confirmNewPassword
      }

      await this.$store.dispatch('auth/changePassword', params)
    }
  }
}
</script>
