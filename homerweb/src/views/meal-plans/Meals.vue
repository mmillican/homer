<template>
  <div class="meal-plans">
    <h1>Meals</h1>

    <div class="actions mb-2">
      <router-link to="./new" class="btn btn-success">Add Meal</router-link>
    </div>

    <table class="table table-striped table-hover">
      <tr>
        <th style="width: 10%;"></th>
        <th style="width: 25%;">Name</th>
        <th style="width: 10%;">Effort</th>
        <th style="width: 45%;">Description</th>
        <th style="width: 10%;"></th>
      </tr>
      <tr v-for="meal in meals" v-bind:key="meal.id">
        <td>
          <ul class="list-inline mb-0">
            <li class="list-inline-item">
              <font-awesome-icon v-if="meal.isKidFriendly" icon="child" class="text-primary" title="Kid friendly" />
            </li>
            <li class="list-inline-item">
              <font-awesome-icon v-if="meal.isFavorite" icon="star" class="text-warning" title="Favorite" />
            </li>
          </ul>
        </td>
        <td>{{ meal.name }}</td>
        <td>{{ meal.prepEffortName }}</td>
        <td>{{ meal.description }}</td>
        <td>
          <router-link :to="{ name: 'edit-meal', params: { id: meal.id } }">Edit</router-link>
        </td>
      </tr>
    </table>

    <div class="legend mt-2">
      <ul class="list-inline">
        <li class="list-inline-item pr-2">
          <font-awesome-icon icon="child" class="text-primary" title="Kid friendly" /> Kid Friendly
        </li>
        <li class="list-inline-item">
          <font-awesome-icon icon="star" class="text-warning" title="Favorite" /> Favorite
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'
export default {
  data () {
    return {

    }
  },

  computed: {
    ...mapState({
      meals: state => state.meals.meals
    })
  },

  async created () {
    this.fetchMeals()
  },

  methods: {
    ...mapActions({
      fetchMeals: 'meals/fetchMeals'
    })
  }
}
</script>
