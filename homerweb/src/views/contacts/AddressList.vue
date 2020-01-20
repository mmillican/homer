<template>
  <div class="address-list">
    <h1>Addresses</h1>

    <div class="actions mb-2">
      <b-button variant="outline-success" :to="{ name: 'create-address' }">Add Address</b-button>
    </div>

    <div class="search my-4">
      <div class="input-group">
        <div class="input-group-prepend">
          <span class="input-group-text" id="basic-addon1">
            <font-awesome-icon icon="search" title="Search" />
          </span>
        </div>
        <input type="text" v-model="query" class="form-control" placeholder="Filter by last name" aria-label="Last name" aria-describedby="search-addon-buttons">
      </div>
    </div>

    <div class="row row-cols-1 row-cols-md-3">
      <div class="col mb-4" v-for="address in filteredAddreses" :key="address.id">
        <div class="card address">
          <div class="card-body">
            <h5>{{ address.lastName }} <small>{{ address.firstName }}</small></h5>
            {{ address.addressLine1 }}<br />
            <template v-if="address.addressLine2">
              {{ address.addressLine2 }}<br />
            </template>
            {{ address.city + ', ' + address.state + ' ' + address.zipCode }}
          </div>
          <div class="card-footer py-1 d-flex justify-content-between">
            <b-button variant="link" size="sm" :href="generateGoogleMapUrl(address)" :disabled="!address.addressLine1">
              <font-awesome-icon icon="map-marked" title="Get Directions" />
            </b-button>

            <span v-if="address.needsUpdate" class="text-info float-right">Needs update</span>

            <b-button variant="link" size="sm" :to="{ name: 'edit-address', params: { id: address.id }}">
              <font-awesome-icon icon="edit" title="Get Directions" />
            </b-button>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  data () {
    return {
      query: null
    }
  },
  computed: {
    ...mapGetters('address', [ 'addresses' ]),
    filteredAddreses: function () {
      if (this.query) {
        return this.addresses.filter(x => x.lastName.toLowerCase().includes(this.query.toLowerCase()))
      } else {
        return this.addresses
      }
    }
  },
  async created () {
    await this.getAddresses()
  },
  methods: {
    async getAddresses () {
      await this.$store.dispatch('address/fetchAddresses')
    },
    stringify (address) {
      return `${address.addressLine1}, ${address.city}, ${address.state}, ${address.zipCode}`
    },
    generateGoogleMapUrl (address) {
      var addressStr = encodeURIComponent(this.stringify(address))
      return `https://www.google.com/maps/dir/?api=1&destination=${addressStr}`
    }
  }
}
</script>
