<template>
  <div class="journal-entry-editor">
    <div class="row">
      <div class="col-md-6">
        <form @submit.prevent="saveEntry">
          <div class="form-group">
            <label for="entry-date">Date</label>
            <datepicker id="entry-date" format="yyyy-MM-dd" :bootstrap-styling="true" :typable="true" v-model="entry.date" />
          </div>
          <div class="form-group">
            <label for="entry-personal">Personal (home)</label>
            <textarea id="entry-personal" rows="5" class="form-control" v-model="entry.personal"></textarea>
          </div>
          <div class="form-group">
            <label for="entry-work">Work</label>
            <textarea id="entry-work" rows="5" class="form-control" v-model="entry.work"></textarea>
          </div>
          <div class="form-group">
            <label for="entry-mood">Mood (overall)</label>
            <select id="entry-mood" class="form-control" v-model="entry.mood">
              <option value=""></option>
              <option value="Happy">Happy</option>
              <option value="Content">Content</option>
              <option value="Sad">Sad</option>
              <option value="Stressed">Stressed</option>
              <option value="Frustrated">Frustrated</option>
              <option value="Upset">Upset</option>
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

export default {
  props: {
    entry: {
      type: Object,
      required: false
    }
  },
  components: {
    Datepicker
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
