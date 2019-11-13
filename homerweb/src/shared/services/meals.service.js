import apiService from './api.service'

export default class MealsService {
  async getMeals () {
    var response = await apiService.get('meals')
    return response
  }

  async getMeal (id) {
    var response = await apiService.get(`meals/${id}`)
    return response
  }

  async addMeal (meal) {
    var response = await apiService.post(`meals`, meal)
    return response
  }

  async updateMeal (meal) {
    var response = await apiService.put(`meals/${meal.id}`, meal)
    return response
  }

  async deleteMeal (meal) {
    await apiService.delete(`meals/${meal.id}`)
    // return response
  }
}

export const mealsService = new MealsService()
