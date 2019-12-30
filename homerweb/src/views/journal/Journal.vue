<template>
  <div class="journal">
    <h1>Journal</h1>

    <div class="actions my-2">
      <button class="btn btn-success" @click="addEntry">Add Entry</button>
    </div>

    <b-modal id="modal-entry-editor" hide-footer>
      <template v-slot:modal-title>
        Journal Entry
      </template>

      <editEntry :entry="newEntry" @entrySaved="entrySaved" />
    </b-modal>

    <div class="entries">
      <journal-entry v-for="entry in entries" :key="entry.id" :entry="entry" :includeHr="true" @openEditor="$bvModal.show('modal-entry-editor')" />
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
      this.$store.dispatch('journal/addEntry')
      this.$bvModal.show('modal-entry-editor')
    },
    async entrySaved () {
      this.isAddingNew = false
      this.$bvModal.hide('modal-entry-editor')
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
