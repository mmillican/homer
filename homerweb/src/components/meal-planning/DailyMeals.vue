<template>
  <div class="card">
    <div class="card-body">
      <h4 class="card-title meal-date">
        {{ date | moment('ddd') }}
        <small>
          {{ date | moment('MMM Do') }}
        </small>
      </h4>
      <div class="card-text">
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
  }
}
</script>

<style lang="scss" scoped>
h4 {
  small { display: block !important; }
}
</style>
