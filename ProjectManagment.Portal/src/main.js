import Vue from 'vue'
import App from './App.vue'
import router from './router'
import Vuelidate from 'vuelidate'
import VueRouter from 'vue-router'
import Buefy from 'buefy'
import Vuesax from 'vuesax'

import 'vuesax/dist/vuesax.css' 
import 'buefy/dist/buefy.css'

Vue.config.productionTip = false

Vue.use(Vuelidate)
Vue.use(VueRouter)
Vue.use(Buefy)
Vue.use(Vuesax)

new Vue({
  router,
  render: h => h(App)
})
  .$mount('#app')

