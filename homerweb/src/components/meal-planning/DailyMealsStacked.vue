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
          <ul class="list-unstyled">
            <li v-for="meal in breakfastMeals" v-bind:key="meal.id">{{ meal.meal.name }}</li>
          </ul>
        </dd>
        <dt>Lunch</dt>
        <dd>
          <ul class="list-unstyled">
            <li v-for="meal in lunchMeals" v-bind:key="meal.id">{{ meal.meal.name }}</li>
          </ul>
        </dd>
        <dt>Dinner</dt>
        <dd>
          <ul class="list-unstyled">
            <li v-for="meal in dinnerMeals" v-bind:key="meal.id">{{ meal.meal.name }}</li>
          </ul>
        </dd>
      </dl>
    </div>
  </div>
</template>

<script>
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
