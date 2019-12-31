<template>
  <div class="journal-entry">
    <template>
      <div class="float-right entry-actions">
        <button class="btn btn-outline-secondary" @click="editEntry">Edit</button>
      </div>
      <h3>{{ entry.date | moment('ddd, MMM DD') }}<mood-emoji :mood="entry.mood" /></h3>

      <p>
        <b>Personal</b><br />
        {{ entry.personal }}
      </p>
      <p>
        <b>Work</b><br />
        {{ entry.work }}
      </p>
      <!-- <p>
        <b>Mood:</b> {{ entry.mood }} <mood-emoji :mood="entry.mood" />
      </p> -->
    </template>

    <hr v-if="includeHr" />
  </div>
</template>

<script>
import MoodEmoji from './MoodEmoji'

export default {
  components: {
    MoodEmoji
  },
  props: {
    entry: {
      type: Object,
      required: true
    },
    includeHr: {
      type: Boolean,
      required: false,
      default: false
    }
  },
  methods: {
    editEntry () {
      this.$store.dispatch('journal/editEntry', this.entry)
      this.$emit('openEditor')
    }
  }
}
</script>
