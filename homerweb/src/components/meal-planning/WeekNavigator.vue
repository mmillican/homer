<template>
  <ul class="pagination justify-content-center">
    <li class="page-item">
      <router-link :to="{ query: { week: previousWeek.format('YYYY-MM-DD') } }" class="page-link">
        <!-- {{ previousWeek | moment('DD-MMM-YYYY') }} -->
        Previous Week
      </router-link>
    </li>
    <li class="page-item">
      <router-link :to="{ query: { week: nextWeek.format('YYYY-MM-DD') } }" class="page-link">
        <!-- {{ nextWeek | moment('DD-MMM-YYYY') }} -->
        Next Week
      </router-link>
    </li>
  </ul>
</template>

<script>
import moment from 'moment'
export default {
  props: {
    weekStart: {
      type: String,
      required: true
    }
  },
  computed: {
    previousWeek () {
      return moment(this.weekStart).subtract(1, 'weeks')
    },
    nextWeek () {
      return moment(this.weekStart).add(1, 'weeks')
    }
  }
}
</script>
