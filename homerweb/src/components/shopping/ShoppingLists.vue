<template>
  <div class="shopping-lists">
    <div class="list-group">
      <router-link class="list-group-item list-group-item-action" v-bind:class="{ active: currentList && list.id === currentList.id }" v-for="list in lists" v-bind:key="list.id" :to="{ name: 'shopping', params: { listId: list.id } }">
        {{ list.name }}
      </router-link>
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
      lists: state => state.shopping.lists,
      currentList: state => state.shopping.currentList
    })
  },

  async created () {
    this.fetchLists()
  },

  methods: {
    ...mapActions({
      fetchLists: 'shopping/fetchLists'
    })
  }
}
</script>
