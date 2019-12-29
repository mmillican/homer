<template>
  <div class="journal-entry">
    <template v-if="!isEditing">
      <div class="float-right entry-actions">
        <button class="btn btn-outline-secondary" @click="editEntry">Edit</button>
      </div>
      <h3>{{ entry.date | moment('utc', 'ddd, MMM DD') }}</h3>

      <p>
        <b>Personal</b><br />
        {{ entry.personal }}
      </p>
      <p>
        <b>Work</b><br />
        {{ entry.work }}
      </p>
      <p>
        <b>Mood:</b> {{ entry.mood }}
      </p>
    </template>

    <editEntry v-if="isEditing" :entry="entry" @entrySaved="entrySaved" />

    <hr v-if="includeHr" />
  </div>
</template>

<script>
import EditEntry from './EditEntry'

export default {
  components: {
    EditEntry
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
  data () {
    return {
      isEditing: false
    }
  },
  methods: {
    editEntry () {
      this.isEditing = true
    },
    entrySaved () {
      this.isEditing = false
    }
  }
}
</script>
