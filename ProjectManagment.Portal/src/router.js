import Vue from 'vue'
import Router from 'vue-router'
import Register from './components/User/Register'
import Login from './components/User/Login'
import Home from './components/Home'

Vue.use(Router)


// function authGuard (to, from, next){
//   var isAuthenticated = (localStorage.getItem('userToken') != null)

//   if (!isAuthenticated) next('/login')
//   // if the user is not authenticated, `next` is called twice
//   else next()
// }

const router = new Router({
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
    },
    {
      path: '/',
      name: 'Home',
      component: Home,
      beforeEnter: ((to, from, next) => {
        var token = localStorage.getItem('userToken')
       
        if (token == "undefined") {
          next('/login')
        }
        // if the user is not authenticated, `next` is called twice
        else {
          next()
        }
      })
    }
  ]
})


export default router