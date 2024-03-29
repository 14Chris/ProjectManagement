import Router from 'vue-router'
import Register from './components/User/Register'
import Login from './components/User/Login'
import Home from './components/Home'
import Profile from './components/User/Profile'
import ForgotPassword from './components/User/Password/ForgotPassword'
import ResetPassword from './components/User/Password/ResetPassword'
import Projects from './components/Projects/Projects'
import ProjectDetail from './components/Projects/ProjectDetail'
import ProjectTasks from './components/Projects/Tasks/Tasks'
import ProjectCalendar from './components/Projects/Calendar/Calendar'
import ProjectSettings from './components/Projects/Settings/Settings'
import ProjectDocuments from './components/Projects/Documents/Documents'
import ProjectDashboard from './components/Projects/Dashboard/Dashboard'

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
      path: '/forgot_password',
      component: ForgotPassword
    },
    {
      path: '/reset_password/:token',
      component: ResetPassword
    },
    {
      path: '/',
      component: Home,
      children: [
        {
          path: 'profile',
          component: Profile
        },
        {
          path: 'projects',
          component: Projects,

        },
        {
          path: "projects/:id",
          component: ProjectDetail,
          children: [
            {
              path: "dashboard",
              component: ProjectDashboard,
            },
            {
              path: "tasks",
              component: ProjectTasks,
            },
            {
              path: "calendar",
              component: ProjectCalendar,
            },
            {
              path: "documents",
              component: ProjectDocuments,
            },
            {
              path: "settings",
              component: ProjectSettings,
            },
          ]
        },
      ],
      beforeEnter: ((to, from, next) => {
        var token = localStorage.getItem('userToken')
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