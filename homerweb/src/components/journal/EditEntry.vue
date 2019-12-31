<template>
  <div class="journal-entry-editor">
    <div class="row">
      <div class="col-md-12">
        <form @submit.prevent="saveEntry">
          <div class="form-group">
            <label for="entry-date" class="sr-only">Date</label>
            <datepicker id="entry-date" :bootstrap-styling="true" :typeable="true" v-model="entry.date" />
          </div>
          <div class="form-group">
            <label for="entry-personal">Personal (home)</label>
            <textarea id="entry-personal" rows="3" class="form-control" v-model="entry.personal"></textarea>
          </div>
          <div class="form-group">
            <label for="entry-work">Work</label>
            <textarea id="entry-work" rows="3" class="form-control" v-model="entry.work"></textarea>
          </div>
          <div class="form-group">
            <label for="entry-mood">Mood (overall)</label>
            <select id="entry-mood" class="form-control" v-model="entry.mood">
              <option value=""></option>
              <option v-for="mood in moodOptions" :key="mood.name" :value="mood.name">
                {{ mood.emoji }} {{ mood.name }}
              </option>
            </select>
          </div>

          <button type="submit" class="btn btn-primary">Save Entry</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import Datepicker from 'vuejs-datepicker'
import { mapState } from 'vuex'
import moods from '../../shared/models/moods'

export default {
  components: {
    Datepicker
  },
  data () {
    return {
      moodOptions: moods,
      entry: {

      }
    }
  },
  computed: {
    ...mapState('journal', {
      editing: 'editingEntry'
    })
  },
  mounted () {
    this.entry = this.editing
  },
  methods: {
    async saveEntry () {
      await this.$store.dispatch('journal/saveEntry', this.entry).then(() => {
        this.$emit('entrySaved')
      })
    }
  }
}
</script>
