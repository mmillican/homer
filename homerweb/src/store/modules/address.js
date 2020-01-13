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
    },
    currentAddress: state => state.currentAddress
  },

  mutations: {
    setAddresses (state, addresses) {
      state.addresses = addresses
    },

    setCurrentAddress (state, address) {
      state.currentAddress = address
    },

    addAddress (state, address) {
      state.addresses.push(address)
    }
  },

  actions: {
    async fetchAddresses ({ commit }) {
      var addresses = await addressService.getAll()

      commit('setAddresses', addresses)
    },
    async getAddress ({ commit }, id) {
      var address = await addressService.getById(id)
      commit('setCurrentAddress', address)
    },
    async saveAddress ({ commit }, address) {
      var isNew = address.id === null
      var response = await addressService.saveAddress(address)

      commit('setCurrentAddress', null)
      if (isNew) {
        commit('addAddress', response)
      }
    }
  }
}
