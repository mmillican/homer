<template>
  <div class="address-list">
    <h1>Addresses</h1>
    <div class="row row-cols-1 row-cols-md-3">
      <div class="col mb-4" v-for="address in addresses" :key="address.id">
        <div class="card address">
          <div class="card-body">
            <h5>{{ address.lastName }} <small>{{ address.firstName }}</small></h5>
            {{ address.addressLine1 }}<br />
            <template v-if="address.addressLine2">
              {{ address.addressLine2 }}<br />
            </template>
            {{ address.city + ', ' + address.state + ' ' + address.zipCode }}
          </div>
          <div class="card-footer py-2 d-flex justify-content-between align-items-center">
            <!-- <a href="#">Edit</a> -->
            <div>
              <b-dropdown id="dropdown-1" text="..." size="sm" variant="link" :no-caret="true">
                <template slot="button-content">
                  <font-awesome-icon icon="cog" title="Actions" />
                </template>
                <b-dropdown-item :href="generateGoogleMapUrl(address)" :disabled="!address.addressLine1">Google Maps</b-dropdown-item>
                <!-- TODO: Geocode the address and then can hook up Uber links <b-dropdown-item>Uber</b-dropdown-item> -->
                <b-dropdown-divider></b-dropdown-divider>
                <b-dropdown-item>Edit</b-dropdown-item>
              </b-dropdown>
            </div>
            <span v-if="address.needsUpdate" class="text-info">Needs update</span>
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

    }
  },
  computed: {
    ...mapGetters('address', [ 'addresses' ])
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
