import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Auth from '@okta/okta-vue'

Vue.use(Auth, {
  issuer: 'https://dev-510139.oktapreview.com/oauth2/default',
  clientId: '0oaojmjqdzCch3YTU0h7',
  redirectUri: 'http://localhost:8080/implicit/callback',
  // audience: 'http://localhost:8080',
  scopes: ['openid', 'profile', 'email'],
  pkce: true
})

Vue.use(VueRouter)

const routes = [
  { path: '/', name: 'home', component: Home },
  { path: '/shopping/:listId?', name: 'shopping', component: () => import('../views/Shopping.vue'), meta: { requiresAuth: true } },
  { path: '/implicit/callback', component: Auth.handleCallback(), meta: { requiresAuth: true } },
  {
    path: '/meal-plans',
    name: 'meal-planning',
    component: () => import('../views/meal-plans/MealPlanning.vue'),
    meta: { requiresAuth: true },
    children: [
      { path: 'planner', name: 'meal-planner', component: () => import('../views/meal-plans/Planner.vue') },
      { path: 'meals', name: 'meals', component: () => import('../views/meal-plans/Meals.vue') },
      { path: 'meals/new', name: 'add-meal', component: () => import('../views/meal-plans/AddMeal.vue') },
      { path: 'meals/edit/:id', name: 'edit-meal', component: () => import('../views/meal-plans/EditMeal.vue') }

    ]
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
  linkActiveClass: 'active',
  linkExactActiveClass: 'active'
})

router.beforeEach(async (to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!await Vue.prototype.$auth.isAuthenticated()) {
      next({
        path: '/',
        query: { redirect: to.fullPath }
      })
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router
