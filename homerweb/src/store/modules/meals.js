import { mealsService } from '../../shared/services/meals.service'

export default {
  namespaced: true,

  state: {
    meals: [ ],
    currentMeal: null
  },

  getters: {
  },

  mutations: {
    setMeals (state, meals) {
      state.meals = meals
    },

    setCurrentMeal (state, meal) {
      state.currentMeal = meal
    },

    addMeal (state, meal) {
      state.meals.push(meal)
    },

    deleteMeal (state, meal) {
      state.meals.splice(state.meals.indexOf(meal), 1)
    }
  },

  actions: {
    async fetchMeals ({ commit }) {
      var meals = await mealsService.getMeals()

      commit('setMeals', meals)
    },
    async getMeal ({ commit }, id) {
      var meal = await mealsService.getMeal(id)

      commit('setCurrentMeal', meal)
    },
    async addMeal ({ commit }, meal) {
      var newItem = await mealsService.addMeal(meal)

      commit('addMeal', newItem)
    },
    async updateMeal ({ commit }, meal) {
      await mealsService.updateMeal(meal)

      commit('setCurrentMeal', meal)
    },
    async deleteMeal ({ commit }, meal) {
      await mealsService.deleteMeal(meal)

      commit('deleteMeal', meal)
    }
  }
}
