import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

const routes = [
  { path: '/', name: 'home', component: Home },
  { path: '/shopping/:listId?', name: 'shopping', component: () => import('../views/Shopping.vue') },
  {
    path: '/meal-plans',
    name: 'meal-planning',
    component: () => import('../views/meal-plans/MealPlanning.vue'),
    children: [
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

export default router
