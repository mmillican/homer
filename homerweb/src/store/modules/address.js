/* eslint-disable comma-dangle */
import { addressService } from '../../shared/services/address.service'

export default {
  namespaced: true,

  state: {
    addresses: [ ],
    currentAddress: null
  },

  getters: {
    addresses (state) {
      return state.addresses
    }
  },

  mutations: {
    setAddresses (state, addresses) {
      state.addresses = addresses
    },

    // setCurrentAddress (state, address) {
    //   state.currentAddress = address
    // },

    // addMeal (state, meal) {
    //   state.meals.push(meal)
    // },

    // deleteMeal (state, meal) {
    //   state.meals.splice(state.meals.indexOf(meal), 1)
    // },

    // setScheduledMeals (state, meals) {
    //   state.scheduledMeals = meals
    // },

    // addScheduledMeal (state, meal) {
    //   state.scheduledMeals.push(meal)
    // },

    // removeScheduledMeal (state, meal) {
    //   state.scheduledMeals.splice(state.scheduledMeals.indexOf(meal), 1)
    // }
  },

  actions: {
    async fetchAddresses ({ commit }) {
      var addresses = await addressService.getAll()

      commit('setAddresses', addresses)
    },
    // async getMeal ({ commit }, id) {
    //   var meal = await mealsService.getMeal(id)

    //   commit('setCurrentMeal', meal)
    // },
    // async addMeal ({ commit }, meal) {
    //   var newItem = await mealsService.addMeal(meal)

    //   commit('addMeal', newItem)
    // },
    // async updateMeal ({ commit }, meal) {
    //   await mealsService.updateMeal(meal)

    //   commit('setCurrentMeal', meal)
    // },
    // async deleteMeal ({ commit }, meal) {
    //   await mealsService.deleteMeal(meal)

    //   commit('deleteMeal', meal)
    // },
    // async fetchScheduledMeals ({ commit }, params) {
    //   var meals = await mealsService.getScheduledMeals(params.startDate, params.endDate)

    //   commit('setScheduledMeals', meals)
    // },
    // async addScheduledMeal ({ commit }, meal) {
    //   var newMeal = await mealsService.addScheduledMeal(meal)

    //   commit('addScheduledMeal', newMeal)
    // },
    // async removeScheduledMeal ({ commit }, meal) {
    //   await mealsService.removeScheduledMeal(meal)

    //   commit('removeScheduledMeal', meal)
    // }
  }
}
