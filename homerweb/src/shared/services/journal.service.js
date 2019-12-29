import apiService from './api.service'

export default class JournalService {
  async getJournalEntries () {
    var response = await apiService.get('journal-entries')
    return response
  }

  async getJournalEntryForDate (date) {
    var response = await apiService.get(`journal-entries/${date}`)
    return response
  }

  async saveJournalEntry (entry) {
    var response
    if (entry.id) {
      response = await apiService.put(`journal-entries/${entry.id}`, entry)
      return response
    } else {
      response = await apiService.post(`journal-entries`, entry)
      return response
    }
  }
}

export const journalService = new JournalService()
