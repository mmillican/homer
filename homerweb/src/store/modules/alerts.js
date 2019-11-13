export default {
  namespaced: true,

  state: {
    alerts: [ ]
  },

  getters: {
    getByType: (state) => (type) => {
      return state.alerts.filter(x => x.type === type)
    },
    hasAlerts: state => {
      return state.alerts.length > 0
    }
  },

  mutations: {
    addAlert (state, alert) {
      state.alerts.push(alert)
    },
    removeAlert (state, alert) {
      state.alerts.splice(state.alerts.indexOf(alert), 1)
    },
    clearAlerts (state) {
      state.alerts = [ ]
    }
  },

  actions: {
    addAlert ({ commit }, alert) {
      commit('addAlert', alert)

      // This isn't ideal but works for now
      setTimeout(function () {
        commit('removeAlert', alert)
      }, 10000)
    },
    addSuccess ({ dispatch }, message) {
      dispatch('addAlert', { type: 'success', message: message })
    },
    addError ({ dispatch }, message) {
      dispatch('addAlert', { type: 'error', message: message })
    },
    addWarning ({ dispatch }, message) {
      dispatch('addAlert', { type: 'warning', message: message })
    },
    clearAlerts ({ commit }) {
      commit('clearAlerts')
    }
  }
}
