import { shoppingService } from '../../shared/services/shopping.service'

export default {
  namespaced: true,

  state: {
    lists: [ ],
    currentList: null,
    listItems: [ ],
    filterPurchased: false
  },

  getters: {
    currentList (state) {
      return state.currentList
    },

    listItems (state) {
      return state.listItems
    }
  },

  mutations: {
    setLists (state, lists) {
      state.lists = lists
    },

    setCurrentList (state, list) {
      state.currentList = list.list
      state.listItems = list.items
    },

    setListFilter (state, purchased) {
      state.filterPurchased = purchased
    },

    addItemToList (state, item) {
      state.listItems.push(item)
    },

    deleteItem (state, item) {
      state.listItems.splice(state.listItems.indexOf(item), 1)
    }
  },

  actions: {
    async fetchLists ({ commit }) {
      var lists = await shoppingService.getLists()

      commit('setLists', lists)
    },
    async getList ({ state, commit }, listId) {
      var list = await shoppingService.getList(listId)
      var items = await shoppingService.getListItems(listId, state.filterPurchased)

      commit('setCurrentList', { list, items })
    },
    async filterList ({ state, dispatch, commit }, purchased) {
      commit('setListFilter', purchased)
      await dispatch('getList', state.currentList.id)
    },
    async addItemToList ({ commit }, data) {
      var newItem = await shoppingService.addListItem(data.listId, data.item)

      commit('addItemToList', newItem)
    },
    async deleteItem ({ commit }, item) {
      await shoppingService.deleteListItem(item.listId, item)

      commit('deleteItem', item)
    },

    async updatePurchaseStatus ({ commit }, item) {
      if (!item.purchasedOn) {
        item.purchasedOn = new Date() // TODO: UTC
      } else {
        item.purchasedOn = null
      }
      await shoppingService.updateListItem(item.listId, item)
    }
  }
}
