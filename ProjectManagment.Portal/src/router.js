import Vue from 'vue'
import Router from 'vue-router'
import Register from './components/User/Register'
import Login from './components/User/Login'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/register',
      name: 'Register',
      component: Register
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    }
  ]
})