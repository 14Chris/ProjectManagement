import Vue from 'vue'
import App from './App.vue'
import router from './router'
import Vuelidate from 'vuelidate'
import VueRouter from 'vue-router'

Vue.config.productionTip = false
Vue.use(Vuelidate)
Vue.use(VueRouter)

new Vue({
  render: h => h(App),
  router
})
.$mount('#app')

