<template>

  <div class="meal-planner">
    <h1>Meal Planner</h1>
    <week-navigator :weekStart="startOfWeek" />

    <daily-meals-stacked v-for="date in dates" :key="date" :date="date" :meals="getMealsForDate(date)" @openMealEditor="openMealEditor" />

    <b-modal id="modal-mt-editor" hide-footer>
      <template v-slot:modal-title>
        Add Meal for {{ editingMealTime.date | moment('dddd') }}
      </template>
      <form @submit.prevent="addScheduledMeal" class="d-block text-center">
        <div class="form-group">
          <select v-model="editingMealTime.mealTimeId" class="form-control">
          <option value="1">Breakfast</option>
          <option value="2">Lunch</option>
          <option value="3">Dinner</option>
        </select>
        </div>
        <div class="form-group">
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
        </div>
        <b-button type="submit" class="mt-3" variant="primary" block>Add Meal</b-button>
      </form>
    </b-modal>
  </div>
</template>

<script>
import moment from 'moment'
import Multiselect from 'vue-multiselect'
import WeekNavigator from '../../components/meal-planning/WeekNavigator'
import DailyMealsStacked from '../../components/meal-planning/DailyMealsStacked'
import MealIcons from '../../components/meal-planning/MealIcons'
import { mapActions, mapGetters } from 'vuex'

export default {
  components: {
    WeekNavigator,
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
    },
    startOfWeek () {
      var today = moment()
      var weekStart = 3 // Wednesday - TODO: make this a setting
      var startOfWeek = moment()
      if (this.$route.query.week) {
        startOfWeek = moment(this.$route.query.week)
      } else {
        if (today.day() >= weekStart) {
          startOfWeek = today.day(weekStart)
        } else {
          startOfWeek = today.day(weekStart - 7)
        }
      }
      return startOfWeek
    }
  },
  async created () {
    await this.renderSchedule()
  },
  watch: {
    '$route': 'renderSchedule'
  },
  methods: {
    ...mapActions('meals', {
      fetchMeals: 'fetchMeals'
    }),
    async renderSchedule () {
      this.generateDates()
      await this.fetchMeals()
      await this.getScheduledMeals()
    },
    generateDates () {
      this.dates = [ ]
      for (var idx = 0; idx < 7; idx++) {
        var date = moment(this.startOfWeek).add(idx, 'days')
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
    openMealEditor (date) {
      this.editingMealTime.date = date

      this.$bvModal.show('modal-mt-editor')
    },
    async addScheduledMeal () {
      var meal = {
        mealDate: this.editingMealTime.date,
        mealTimeId: this.editingMealTime.mealTimeId * 1,
        mealid: this.editingMealTime.mealId.id
      }

      console.log('new meal', meal)
      this.$store.dispatch('meals/addScheduledMeal', meal).then(() => {
        this.$bvModal.hide('modal-mt-editor')
        this.editingMealTime.timeId = 0
        this.editingMealTime.mealId = 0
      })
    }
  }
}
</script>
