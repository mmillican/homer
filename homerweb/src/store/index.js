import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'
import auth from './modules/auth'
import alerts from './modules/alerts'
import address from './modules/address'
import shopping from './modules/shopping'
import meals from './modules/meals'
import journal from './modules/journal'

Vue.use(Vuex)

const vuexLocal = new VuexPersistence({
  storage: window.localStorage,
  modules: [ 'auth' ]
})

export default new Vuex.Store({
  modules: {
    auth,
    alerts,
    address,
    shopping,
    meals,
    journal
  },
  plugins: [
    vuexLocal.plugin
  ]
})
