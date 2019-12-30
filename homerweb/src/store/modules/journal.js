import { journalService } from '../../shared/services/journal.service'

export default {
  namespaced: true,

  state: {
    entries: [ ],
    editingEntry: null
  },

  getters: {
    entries (state) {
      return state.entries
    },
    editingEntry (state) {
      return state.editingEntry
    },
    isEditing (state) {
      return state.editingEntry !== null
    }
  },

  mutations: {
    setEntries (state, entries) {
      state.entries = entries
    },
    setEditingEntry (state, entry) {
      state.editingEntry = entry
    }
  },

  actions: {
    async fetchEntries ({ commit }) {
      var entries = await journalService.getJournalEntries()
      commit('setEntries', entries)
    },
    // async getEntry ({ commit }, entryDate) {
    //   var entry = await journalService.getJournalEntryForDate(entryDate)
    //   // commit('setEditingEntry', entry)
    // },
    addEntry ({ commit }) {
      let entry = {
        id: null,
        date: new Date(),
        personal: null,
        work: null,
        mood: null
      }
      commit('setEditingEntry', entry)
    },
    editEntry ({ commit }, entry) {
      commit('setEditingEntry', entry)
    },
    async saveEntry ({ commit }, entry) {
      await journalService.saveJournalEntry(entry)
      commit('setEditingEntry', null)
    }
  }
}
