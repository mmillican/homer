import Vue from 'vue'
import Vuex from 'vuex'
import shopping from './modules/shopping'
import meals from './modules/meals'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    shopping,
    meals
  },
  state: {
  },
  mutations: {
  },
  actions: {
  }
})
