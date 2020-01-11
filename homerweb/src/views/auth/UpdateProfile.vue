<template>
  <div class="journal-entry-editor">
    <h1>Update Profile</h1>

    <div class="row">
      <div class="col-md-6">
        <form @submit.prevent="saveProfile" v-if="user">
          <div class="form-group">
            <label for="firstName" class="sr-only">First Name</label>
            <input type="text" id="firstName" v-model="user.firstName" class="form-control" placeholder="First name" />
          </div>
          <div class="form-group">
            <label for="lastName" class="sr-only">Last Name</label>
            <input type="text" id="lastName" v-model="user.lastName" class="form-control" placeholder="Last name" />
          </div>

          <button type="submit" class="btn btn-primary">Save Profile</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  data () {
    return {
      user: null
    }
  },
  computed: {
    ...mapGetters('auth', ['authenticatedUser', 'profile'])
  },
  created () {
    this.user = this.profile
  },
  methods: {
    async saveProfile () {
      await this.$store.dispatch('auth/updateUser', this.user).then(() => {
        // TODO: Show alert / handle error
        console.log('User updated')
      })
    }
  }
}
</script>
