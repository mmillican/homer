/* eslint-disable camelcase */
import Auth from '@aws-amplify/auth'

const state = {
  user: null,
  isAuthenticated: false,
  authenticationStatus: null,
  jwtToken: null
}

const getters = {
  authenticatedUser: state => state.user,
  profile: state => {
    return {
      username: state.user.username,
      email: state.user.attributes ? state.user.attributes.email : '',
      firstName: state.user.attributes ? state.user.attributes.given_name : '',
      lastName: state.user.attributes ? state.user.attributes.family_name : ''
    }
  },
  isAuthenticated: state => state.isAuthenticated,
  authenticationStatus: state => {
    return state.authenticationStatus ? state.authenticationStatus : { variant: 'secondary' }
  },
  hasAuthenticationStatus: state => {
    return state.authenticationStatus !== null
  },
  jwtToken: state => state.jwtToken
}

const mutations = {
  setAuthenticationSuccess (state, msg) {
    state.authenticationStatus = {
      state: 'success',
      message: msg,
      variant: 'success'
    }
  },
  setAuthenticationError (state, err) {
    state.authenticationStatus = {
      state: 'failed',
      message: err.message,
      variant: 'danger'
    }
  },
  clearAuthenticationStatus: state => {
    state.authenticationStatus = null
  },
  setUserAuthenticated (state, user) {
    state.user = user
    state.isAuthenticated = true
  },
  clearAuthentication: state => {
    state.user = null
    state.userId = null
    state.isAuthenticated = null
  },
  setJwtToken (state, token) {
    state.jwtToken = token
  }
}

const actions = {
  clearAuthenticationStatus: context => {
    context.commit('clearAuthenticationStatus', null)
  },
  signIn: async (context, params) => {
    context.commit('clearAuthenticationStatus', null)

    try {
      const user = await Auth.signIn(params.username, params.password)
      context.commit('setUserAuthenticated', user)

      // TODO: Should force user to update their password
      if (user.challengeName === 'NEW_PASSWORD_REQUIRED') {
        await Auth.completeNewPassword(user, params.password)
      }
      const jwtToken = (await Auth.currentSession()).getIdToken().getJwtToken()
      context.commit('setJwtToken', jwtToken)
    } catch (err) {
      console.log(err)
      context.commit('setAuthenticationError', err)
    }
  },
  signOut: async context => {
    try {
      await Auth.signOut()
      Auth.updateUserAttributes()
    } catch (err) {
      console.err(err)
    }
    context.commit('clearAuthentication', null)
  },
  changePassword: async (context, params) => {
    if (params.confirmNewPassword !== params.newPassword) {
      context.commit('setAuthenticationError', { message: 'New passwords do not match.' })
      return
    }

    try {
      let user = await Auth.currentAuthenticatedUser()

      await Auth.changePassword(user, params.currentPassword, params.newPassword)
      context.commit('setAuthenticationSuccess', 'Your password has been changed.')
    } catch (err) {
      console.log('Error changing password', err)
      context.commit('setAuthenticationError', err)
    }
  },
  updateUser: async (context, params) => {
    try {
      let user = await Auth.currentAuthenticatedUser()

      let profile = {
        // Username can't be updated
        email: params.email,
        given_name: params.firstName,
        family_name: params.lastName
      }

      let result = await Auth.updateUserAttributes(user, profile)

      if (result !== 'SUCCESS') {
        throw new Error(`Error updating user: ${result}`)
      }

      // Update the user in state
      user = await Auth.currentUserInfo()
      context.commit('setUserAuthenticated', user)
    } catch (err) {
      console.error('Error updating user profile', err)
    }
  }
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
}
