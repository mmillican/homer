<template>
  <div class="journal-entry">
    <template v-if="!isEditing">
      <div class="float-right entry-actions">
        <button class="btn btn-outline-secondary" @click="editEntry">Edit</button>
      </div>
      <h3>{{ entry.date | moment('ddd, MMM DD') }}</h3>

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

    <hr v-if="includeHr" />
  </div>
</template>

<script>
export default {
  components: {
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
  },
  methods: {
    editEntry () {
      this.$store.dispatch('journal/editEntry', this.entry)
      this.$emit('openEditor')
    }
  }
}
</script>
