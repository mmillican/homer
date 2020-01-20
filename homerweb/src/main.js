import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import BootstrapVue from 'bootstrap-vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VueMoment from 'vue-moment'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faEdit, faTrash, faCheckSquare, faChild, faStar, faCog, faSearch, faMapMarked } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import Auth from '@aws-amplify/auth'
import AuthConfig from './aws-exports'

// TODO: Replace with SCSS version
import 'bootstrap/dist/css/bootstrap.css'
// import 'bootstrap-vue/dist/bootstrap-vue.css'
import 'vue-multiselect/dist/vue-multiselect.min.css'

Vue.config.productionTip = false

Auth.configure(AuthConfig)

Vue.use(BootstrapVue)
Vue.use(VueMoment)

Vue.use(VueAxios, axios)

library.add(faEdit, faTrash, faCheckSquare, faChild, faStar, faCog, faSearch, faMapMarked)

Vue.component('font-awesome-icon', FontAwesomeIcon)

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
