<template>
  <ul class="list-unstyled">
    <li v-for="meal in meals" v-bind:key="meal.id">
      <b-button variant="link" size="sm" @click="removeMeal(meal)" class="pl-0">
        <font-awesome-icon icon="trash" class="text-danger" title="Remove meal" />
      </b-button>
      {{ meal.meal.name }}
      <meal-icons :meal="meal.meal" class="d-inline" />
    </li>
  </ul>
</template>

<script>
import MealIcons from './MealIcons'
export default {
  props: {
    meals: {
      type: Array,
      required: true
    }
  },
  components: {
    MealIcons
  },
  methods: {
    async removeMeal (meal) {
      if (!confirm('Are you sure you want to remove this meal?')) {
        return
      }

      await this.$store.dispatch('meals/removeScheduledMeal', meal)
    }
  }
}
</script>
