import apiService from './api.service'

export default class AddressService {
  async getAll () {
    var response = await apiService.get('addresses')
    return response
  }

  async getById (id) {
    var response = await apiService.get(`addresses/${id}`)
    return response
  }

  async save (address) {
    var response
    if (address.id) {
      response = await apiService.put(`addresses/${address.id}`, address)
      return response
    } else {
      response = await apiService.post(`addresses`, address)
      return response
    }
  }
}

export const addressService = new AddressService()
