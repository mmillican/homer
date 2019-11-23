<template>
  <b-navbar toggleable="lg" type="dark" variant="dark">
    <div class="container">
      <b-navbar-brand to="/">Homer</b-navbar-brand>

      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <template v-if="isAuthenticated">
            <b-nav-item to="/shopping">Shopping</b-nav-item>
            <b-nav-item to="/meal-plans/meals">Meals</b-nav-item>
            <b-nav-item to="/meal-plans/planner">Meal Planner</b-nav-item>
          </template>
        </b-navbar-nav>

        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <b-nav-item v-on:click="login" v-if="!isAuthenticated">Sign in</b-nav-item>
          <b-nav-item to="/signup" v-if="!isAuthenticated">Sign up</b-nav-item>
          <b-nav-item-dropdown right v-if="isAuthenticated">
            <!-- Using 'button-content' slot -->
            <template slot="button-content">{{ userName }}</template>
            <b-dropdown-item href="#">Profile</b-dropdown-item>
            <b-dropdown-item href="#" v-on:click="logout">Sign Out</b-dropdown-item>
          </b-nav-item-dropdown>
        </b-navbar-nav>
      </b-collapse>
    </div>
  </b-navbar>
</template>

<script>
export default {
  data () {
    return {
      isAuthenticated: false,
      userName: ''
    }
  },
  created () {
    this.authenticate()
  },
  watch: {
    '$route': 'authenticate'
  },
  methods: {
    async authenticate () {
      this.isAuthenticated = await this.$auth.isAuthenticated()
      var user = await this.$auth.getUser()
      if (user) {
        this.userName = user.name
      }
    },
    login () {
      this.$auth.loginRedirect('/')
    },
    async logout () {
      await this.$auth.logout()
      await this.authenticate()

      // Navigate back to home
      this.$router.push({ path: '/' })
    }
  }
}
</script>
