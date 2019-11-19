<template>
  <div class="card card-default day mb-2">
    <div class="card-header">
      <h4 class="card-title meal-date">
        {{ date | moment('dddd') }}
        <small>
          {{ date | moment('MMM Do') }}
        </small>
      </h4>
    </div>
    <div class="card-body">
      <dl>
        <dt>Breakfast</dt>
        <dd>
          <ul class="list-unstyled">
            <li v-for="meal in breakfastMeals" v-bind:key="meal.id">{{ meal.meal.name }}</li>
          </ul>
          <b-button size="sm" variant="outline-success" @click="editMealTime(date, 1)">Add</b-button>
        </dd>
        <dt>Lunch</dt>
        <dd>
          <ul class="list-unstyled">
            <li v-for="meal in lunchMeals" v-bind:key="meal.id">{{ meal.meal.name }}</li>
          </ul>
          <b-button size="sm" variant="outline-success" @click="editMealTime(date, 2)">Add</b-button>
        </dd>
        <dt>Dinner</dt>
        <dd>
          <ul class="list-unstyled">
            <li v-for="meal in dinnerMeals" v-bind:key="meal.id">{{ meal.meal.name }}</li>
          </ul>
          <b-button size="sm" variant="outline-success" @click="editMealTime(date, 3)">Add</b-button>
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
    editMealTime (date, timeId) {
      this.$emit('editMealTime', date, timeId)
    }
  }
}
</script>
