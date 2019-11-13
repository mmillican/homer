<template>
  <div class="edit-meal">
    <h1>Add Meal</h1>

    <form @submit.prevent="saveMeal">
      <div class="row">
        <div class="col-md-6">
          <div class="form-group">
            <label for="name">Name</label>
            <input type="text" id="name" v-model="meal.name" class="form-control" />
          </div>
          <div class="form-group">
            <label for="description">Description</label>
            <textarea id="description" v-model="meal.description" class="form-control"></textarea>
          </div>
          <div class="form-group">
            <label for="prepEffort">Prep effort</label>
            <select id="prepEffort" v-model="meal.prepEffort" class="form-control">
              <option value="1">Low</option>
              <option value="2">Medium</option>
              <option value="3">High</option>
            </select>
          </div>
          <div class="form-group">
            <div class="form-check">
              <input class="form-check-input" type="checkbox" id="isKidFriendly" v-model="meal.isKidFriendly">
              <label class="form-check-label" for="isKidFriendly">
                Kid friendly
              </label>
            </div>
          </div>
          <div class="form-group">
            <div class="form-check">
              <input class="form-check-input" type="checkbox" id="isFavorite" v-model="meal.isFavorite">
              <label class="form-check-label" for="isFavorite">
                Favorite
              </label>
            </div>
          </div>
          <div class="form-group">
            <button type="submit" class="btn btn-primary">Save meal</button>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
export default {
  data () {
    return {
      meal: {
        id: 0,
        name: null,
        description: null,
        prepEffort: 1,
        isKidFriendly: false,
        isFavorite: false
      }
    }
  },
  methods: {
    async saveMeal () {
      this.meal.prepEffort = this.meal.prepEffort * 1
      await this.$store.dispatch('meals/addMeal', this.meal)

      this.$router.navigate('../')
    }
  }
}
</script>
