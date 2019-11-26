<template>
  <div class="add-list-item">
    <form class="add-item-form mb-4" v-on:submit.prevent="addItemToList">
      <div class="form-group row">
          <div class="col-md-10 col-8">
              <input type="text" name="name" v-model="item.name" class="form-control form-control-lg" placeholder="What do you want to add?">
          </div>
          <div class="col-md-2 col-4">
              <button type="submit" class="btn btn-primary btn-block btn-lg">Add</button>
          </div>
      </div>
  </form>
  </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  data () {
    return {
      item: {
        name: null
      }
    }
  },
  computed: {
    ...mapState({
      currentList: state => state.shopping.currentList
    })
  },
  async created () {

  },
  methods: {
    async addItemToList () {
      await this.$store.dispatch('shopping/addItemToList', { listId: this.currentList.id, item: this.item })

      this.item.name = null
    }
  }
}
</script>
