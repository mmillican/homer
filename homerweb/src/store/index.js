import Vue from 'vue'
import Vuex from 'vuex'
import alerts from './modules/alerts'
import shopping from './modules/shopping'
import meals from './modules/meals'
import journal from './modules/journal'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    alerts,
    shopping,
    meals,
    journal
  },
  state: {
  },
  mutations: {
  },
  actions: {
  }
})
