import Router from 'vue-router'
import Register from './components/User/Register'
import Login from './components/User/Login'
import Home from './components/Home'
import Profile from './components/User/Profile'

const router = new Router({
  routes: [
    {
      path: '/register',
      component: Register
    },
    {
      path: '/login',
      component: Login
    },
    {
      path: '/',
      component: Home,
      children: [{
        path: 'profile',
        component: Profile
      }],
      beforeEnter: ((to, from, next) => {
        var token = localStorage.getItem('userToken')
        console.log(token)
        // if the user is not authenticated, `next` is called twice
        if (token == "undefined" || token == null) {
          next('/login')
        }
        else {
          next()
        }
      }),
     
    }
  ]
})

export default router