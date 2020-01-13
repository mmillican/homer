<template>
  <div class="edit-address">
    <h1>{{ title }}</h1>

    <div class="row">
      <div class="col-md-6">
        <b-form @submit.prevent="saveAddress" v-if="address">
          <b-form-group label="Last Name" label-for="lastNameInput">
            <b-form-input id="lastNameInput" type="text" v-model="address.lastName" required placeholder="Last name" />
          </b-form-group>
          <b-form-group label="First Name(s)" label-for="firstNameInput">
            <b-form-input id="firstNameInput" type="text" v-model="address.firstName" required placeholder="First name(s)" />
          </b-form-group>

          <b-form-group label="Address" label-for="addressLine1Input">
            <b-form-input id="addressLine1Input" type="text" v-model="address.addressLine1" required placeholder="Street address" />
          </b-form-group>
          <b-form-group label="Address (line 2)" label-for="addressLine2Input" :label-sr-only="true">
            <b-form-input id="addressLine2Input" type="text" v-model="address.addressLine2" placeholder="Street address (line 2)" />
          </b-form-group>

          <div class="row">
            <b-form-group label="City" label-for="cityInput" :label-sr-only="true" class="col-md-6">
              <b-form-input id="cityInput" type="text" v-model="address.city" required placeholder="City" />
            </b-form-group>

            <b-form-group label="State" label-for="stateInput" :label-sr-only="true" class="col-md-3">
              <b-form-input id="stateInput" type="text" v-model="address.state" required placeholder="State" />
            </b-form-group>

            <b-form-group label="Zip Code" label-for="zipCodeInput" :label-sr-only="true" class="col-md-3">
              <b-form-input id="zipCodeInput" type="text" v-model="address.zipCode" required placeholder="Zip Code" />
            </b-form-group>
          </div>

          <b-form-group>
            <!-- <div class="checkbox">
              <input type="needsUpdateCheck" id="checkbox" v-model="address.needsUpdate">
              <label for="needsUpdateCheck">Needs update{{ checked }}</label>
            </div> -->
            <b-form-checkbox id="needsUpdateCheck" v-model="address.needsUpdate">
              Needs update
            </b-form-checkbox>
          </b-form-group>

          <b-button type="submit" variant="primary">Save address</b-button>
          <b-button to="/addresses" variant="link">Cancel</b-button>
        </b-form>
      </div>
    </div>

  </div>
</template>
<script>
import { mapGetters } from 'vuex'
export default {
  data () {
    return {
      title: 'New Address',
      address: null
    }
  },
  computed: {
    ...mapGetters('address', [ 'currentAddress' ])
  },
  created () {
    this.getAddress()
  },
  watch: {
    '$route': 'getAddress'
  },
  methods: {
    async getAddress () {
      const id = this.$route.params.id
      if (id) {
        this.title = 'Edit Address'
        await this.$store.dispatch('address/getAddress', id)
        this.address = this.currentAddress
      } else {
        this.title = 'New Address'
        this.reset()
      }
    },
    saveAddress () {
      this.$store.dispatch('address/saveAddress', this.address).then(() => {
        this.$store.dispatch('alerts/addSuccess', 'The address has been saved.')
        this.$router.push('/addresses')
      }).catch(() => {
        this.$store.dispatch('alerts/addError', 'There was an error saving the address.')
      })
    },
    reset () {
      this.address = {
        id: null,
        lastName: null,
        firstName: null,
        addressLine1: null,
        addressLine2: null,
        city: null,
        state: null,
        zipCode: null,
        needsUpdate: false,
        lastUpdated: new Date()
      }
    }
  }
}
</script>
