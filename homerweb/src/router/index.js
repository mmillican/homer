import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import store from '../store'

Vue.use(VueRouter)

const routes = [
  { path: '/', name: 'home', component: Home, meta: { requiresAuth: true } },
  { path: '/signin', name: 'signin', component: () => import('../views/auth/SignIn') },
  { path: '/user/profile', name: 'userEditProfile', component: () => import('../views/auth/UpdateProfile'), meta: { requiresAuth: true } },
  { path: '/journal', name: 'journal', component: () => import('../views/journal/Journal.vue'), meta: { requiresAuth: true } },
  { path: '/shopping/:listId?', name: 'shopping', component: () => import('../views/Shopping.vue'), meta: { requiresAuth: true } },
  {
    path: '/addresses',
    component: () => import('../views/contacts/Addresses.vue'),
    meta: { requiresAuth: true },
    children: [
      { path: '', name: 'address-list', component: () => import('../views/contacts/AddressList.vue') },
      { path: 'new', name: 'create-address', component: () => import('../views/contacts/EditAddress.vue') },
      { path: 'edit/:id', name: 'edit-address', component: () => import('../views/contacts/EditAddress.vue') }
    ]
  },
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

router.beforeEach((to, from, next) => {
  // Is there meta / auth?
  if (to.meta && to.meta.requiresAuth === true) {
    if (store.getters['auth/isAuthenticated']) {
      next()
      return // continue on
    }

    let returnUrl = encodeURI(to.fullPath)
    router.push({ name: 'signin', query: { return: returnUrl } })
    return
  }
  next() // continue routing
})

export default router
