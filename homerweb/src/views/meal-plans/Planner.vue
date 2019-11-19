<template>

  <div class="meal-planner">
    <h1>Meal Planner</h1>

    <daily-meals-stacked v-for="date in dates" :key="date" :date="date" :meals="getMealsForDate(date)" @editMealTime="editMealTime" />

    <b-modal id="modal-mt-editor" hide-footer>
      <template v-slot:modal-title>
        Editing {{ editingMealTime.date | moment('dddd') }}'s {{ getMealTimeName(editingMealTime.timeId) }}
      </template>
      <form @submit.prevent="addScheduledMeal" class="d-block text-center">
        <multiselect v-model="editingMealTime.mealId"  track-by="id" label="name" :options="meals" :show-labels="true">
          <template slot="singleLabel" slot-scope="props">
            <div class="option__desc">
              <span class="option__title">{{ props.option.name }}</span>

              <span class="option__small float-right">
                <meal-icons :meal="props.option" />
              </span>
            </div>
          </template>
          <template slot="option" slot-scope="props">
            <div class="option__desc">
              <span class="option__title">{{ props.option.name }}</span>

              <span class="option__small float-right">
                <meal-icons :meal="props.option" />
              </span>
            </div>
          </template>
        </multiselect>
        <b-button type="submit" class="mt-3" variant="primary" block>Add Meal</b-button>
      </form>
    </b-modal>
  </div>
</template>

<script>
import moment from 'moment'
import Multiselect from 'vue-multiselect'
import DailyMealsStacked from '../../components/meal-planning/DailyMealsStacked'
import MealIcons from '../../components/meal-planning/MealIcons'
import { mapActions, mapGetters } from 'vuex'

export default {
  components: {
    DailyMealsStacked,
    MealIcons,
    Multiselect
  },
  data () {
    return {
      dates: [ ],
      editingMealTime: {
        date: null,
        timeId: null,
        mealId: null
      }
    }
  },
  computed: {
    ...mapGetters('meals', [
      'meals'
    ]),
    isEditingMealTime () {
      return this.editingMealTime.date !== null && this.editingMealTime.timeId !== null
    }
  },
  async created () {
    this.generateDates()
    await this.fetchMeals()
    await this.getScheduledMeals()
  },
  methods: {
    ...mapActions('meals', {
      fetchMeals: 'fetchMeals'
    }),
    generateDates () {
      for (var idx = 0; idx < 7; idx++) {
        var date = moment().startOf('week').add(idx, 'days')
        this.dates.push(date.format('YYYY-MM-DD'))
      }
    },
    async getScheduledMeals () {
      var params = {
        startDate: this.dates[0],
        endDate: this.dates[6]
      }

      await this.$store.dispatch('meals/fetchScheduledMeals', params)
    },
    getMealsForDate (date) {
      return this.$store.state.meals.scheduledMeals.filter(x => x.mealDate === date)
    },
    getMealTimeName (timeId) {
      if (timeId === 1) return 'Breakfast'
      if (timeId === 2) return 'Lunch'
      if (timeId === 3) return 'Dinner'

      return ''
    },
    editMealTime (date, timeId) {
      this.editingMealTime.date = date
      this.editingMealTime.timeId = timeId

      this.$bvModal.show('modal-mt-editor')
    },
    async addScheduledMeal () {
      var meal = {
        mealDate: this.editingMealTime.date,
        mealTimeId: this.editingMealTime.timeId,
        mealid: this.editingMealTime.mealId.id
      }

      console.log('new meal', meal)
      this.$store.dispatch('meals/addScheduledMeal', meal).then(() => {
        this.$bvModal.hide('modal-mt-editor')
      })
    }
  }
}
</script>
