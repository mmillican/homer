import { mealsService } from '../../shared/services/meals.service'

export default {
  namespaced: true,

  state: {
    meals: [ ],
    currentMeal: null,
    scheduledMeals: [ ]
  },

  getters: {
    meals (state) {
      return state.meals
    },
    scheduledMeals (state) {
      return state.scheduledMeals
    },
    scheduledMealsForDate (state, date) {
      return state.scheduledMeals.filter(x => x.mealDate === date)
    }
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
    },

    setScheduledMeals (state, meals) {
      state.scheduledMeals = meals
    },

    addScheduledMeal (state, meal) {
      state.scheduledMeals.push(meal)
    },

    removeScheduledMeal (state, meal) {
      state.scheduledMeals.splice(state.scheduledMeals.indexOf(meal), 1)
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
    },
    async fetchScheduledMeals ({ commit }, params) {
      var meals = await mealsService.getScheduledMeals(params.startDate, params.endDate)

      commit('setScheduledMeals', meals)
    },
    async addScheduledMeal ({ commit }, meal) {
      var newMeal = await mealsService.addScheduledMeal(meal)

      commit('addScheduledMeal', newMeal)
    },
    async removeScheduledMeal ({ commit }, meal) {
      await mealsService.removeScheduledMeal(meal)

      commit('removeScheduledMeal', meal)
    }
  }
}
