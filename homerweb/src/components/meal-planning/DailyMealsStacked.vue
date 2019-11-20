<template>
  <div class="card card-default day mb-2">
    <div class="card-header">
      <h4 class="card-title meal-date d-inline">
        {{ date | moment('dddd') }}
        <small>
          {{ date | moment('MMM Do') }}
        </small>
      </h4>
      <b-button variant="success" size="sm" class="float-right" @click="addMeal(date)">Add Meal</b-button>
    </div>
    <div class="card-body">
      <dl>
        <dt>Breakfast</dt>
        <dd>
          <planned-meal-list :meals="breakfastMeals" />
        </dd>
        <dt>Lunch</dt>
        <dd>
          <planned-meal-list :meals="lunchMeals" />
        </dd>
        <dt>Dinner</dt>
        <dd>
          <planned-meal-list :meals="dinnerMeals" />
        </dd>
      </dl>
    </div>
  </div>
</template>

<script>
import PlannedMealList from './PlannedMealList'

export default {
  props: {
    date: {
      type: String,
      required: true
    },
    meals: {
      type: Array
    }
  },
  components: {
    PlannedMealList
  },
  computed: {
    breakfastMeals () {
      return this.meals.filter(x => x.mealTimeId === 1)
    },
    lunchMeals () {
      return this.meals.filter(x => x.mealTimeId === 2)
    },
    dinnerMeals () {
      return this.meals.filter(x => x.mealTimeId === 3)
    }
  },
  methods: {
    addMeal (date) {
      this.$emit('openMealEditor', date)
    }
  }
}
</script>
