<template>
  <div class="shopping">
    <h1 v-if="list">Shopping List - {{ list.name }}</h1>
    <h1 v-else>Shopping List</h1>

    <div class="row">
      <div class="col-md-3">
        <shopping-lists />

        <div class="list-filter my-4">
          <select class="form-control" v-model="filterPurchased" @change="setFilter">
            <option value="">All</option>
            <option value="false">Not purchased</option>
            <option value="true">Purchased</option>
          </select>
        </div>
      </div>
      <div class="col-md-9">
        <template v-if="list">
          <add-list-item />

          <table class="table">
            <tr v-for="item in items" v-bind:key="item.id" :class="{ 'purchased': item.purchasedOn }">
              <td style="width: 10%">
                <button class="btn btn-link" @click="updatePurchaseStatus(item)">
                  <font-awesome-icon icon="check-square" />
                </button>
              </td>
              <td style="width: 80%">
                {{ item.name }}
              </td>
              <td style="width: 10%">
                <button class="btn btn-link" @click="deleteItem(item)">
                  <font-awesome-icon class="text-danger" icon="trash" />
                </button>
              </td>
            </tr>
          </table>
        </template>
        <div class="alert alert-info" v-else>
          Select a list to begin.
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import ShoppingLists from '../components/shopping/ShoppingLists'
import AddListItem from '../components/shopping/AddListItem'

export default {
  components: {
    ShoppingLists,
    AddListItem
  },
  data () {
    return {
      filterPurchased: this.$store.state.shopping.filterPurchased
    }
  },
  computed: {
    ...mapState({
      list: state => state.shopping.currentList,
      items: state => state.shopping.listItems
    })
  },
  async created () {
    await this.getList()
  },
  watch: {
    '$route': 'getList'
  },
  methods: {
    async getList () {
      if (this.$route.params.listId) {
        await this.$store.dispatch('shopping/getList', this.$route.params.listId)
      }
    },
    async setFilter () {
      await this.$store.dispatch('shopping/filterList', this.filterPurchased)
    },
    async updatePurchaseStatus (item) {
      await this.$store.dispatch('shopping/updatePurchaseStatus', item)
    },
    async deleteItem (item) {
      if (!confirm('Are you sure you want to delete this item?')) {
        return
      }
      await this.$store.dispatch('shopping/deleteItem', item)
    }
    // ...mapActions({
    //   getList: 'shopping/getList'
    // })
  }
}
</script>

<style lang="scss" scoped>
tr.purchased {
  .fa-check-square {
    color: darkgreen // Use variable
  }
}
</style>
