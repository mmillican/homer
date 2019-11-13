import { mealsService } from '../../shared/services/meals.service'

export default {
  namespaced: true,

  state: {
    meals: [ ]
  },

  getters: {
  },

  mutations: {
    setMeals (state, meals) {
      state.meals = meals
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
    async addMeal ({ commit }, data) {
      var newItem = await mealsService.addMeal(data)

      commit('addMeal', newItem)
    },
    async deleteMeal ({ commit }, item) {
      await mealsService.deleteMeal(item)

      commit('deleteMeal', item)
    }
  }
}
