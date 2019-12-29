<template>
  <div class="journal">
    <h1>Journal</h1>

    <div class="actions my-2">
      <button class="btn btn-success" @click="addEntry">Add Entry</button>
    </div>

    <editEntry v-if="isAddingNew" :entry="newEntry" @entrySaved="entrySaved" />

    <div class="entries">
      <journal-entry v-for="entry in entries" :key="entry.id" :entry="entry" :includeHr="true" />
    </div>
  </div>
</template>

<script>
import JournalEntry from '../../components/journal/JournalEntry'
import EditEntry from '../../components/journal/EditEntry'
import { mapState } from 'vuex'

export default {
  components: {
    JournalEntry,
    EditEntry
  },
  data () {
    return {
      isAddingNew: false,
      newEntry: {
        id: '',
        date: new Date(),
        personal: null,
        work: null,
        mood: null
      }
    }
  },
  computed: {
    ...mapState({
      entries: state => state.journal.entries
    })
  },
  async created () {
    await this.getEntries()
  },
  methods: {
    async getEntries () {
      await this.$store.dispatch('journal/fetchEntries')
    },
    addEntry () {
      this.resetNewEntry()
      this.isAddingNew = true
    },
    async entrySaved () {
      this.isAddingNew = false
      await this.getEntries()
    },
    resetNewEntry () {
      this.newEntry.id = ''
      this.newEntry.date = new Date()
      this.newEntry.personal = null
      this.newEntry.work = null
      this.newEntry.mood = null
    }
  }
}
</script>
