import apiService from './api.service'

export default class ShoppingService {
  async getLists () {
    var response = await apiService.get('shopping-lists')
    return response
  }

  async getList (id) {
    var response = await apiService.get(`shopping-lists/${id}`)
    return response
  }

  async getListItems (listId, purchased) {
    var response = await apiService.get(`shopping-lists/${listId}/items?purchased=${purchased}`)
    return response
  }

  async addListItem (listId, item) {
    var response = await apiService.post(`shopping-lists/${listId}/items`, item)
    return response
  }

  async updateListItem (listId, item) {
    var response = await apiService.put(`shopping-lists/${listId}/items/${item.id}`, item)
    return response
  }

  async deleteListItem (listId, item) {
    await apiService.delete(`shopping-lists/${listId}/items/${item.id}`)
    // return response
  }
}

export const shoppingService = new ShoppingService()
