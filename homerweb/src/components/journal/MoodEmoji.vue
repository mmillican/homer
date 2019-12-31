<template>
  <span class="emoji"  :title="mood">
    {{ emoji }}
  </span>
</template>

<script>
import moods from '../../shared/models/moods'

export default {
  props: {
    mood: {
      type: String
    }
  },
  computed: {
    emoji () {
      let mood = moods.find(x => x.name === this.mood)
      return mood ? mood.emoji : ''
    }
  }
}
</script>
