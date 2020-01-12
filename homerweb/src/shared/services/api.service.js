import axios from 'axios'
import store from '../../store'

const client = axios.create({
  // baseURL: 'https://localhost:44362/',
  baseURL: process.env.VUE_APP_API_URL,
  json: true
})

// TODO: Interceptors

export default {
  async execute (method, resource, data) {
    let accessToken = store.getters['auth/jwtToken']

    return client({
      method,
      url: resource,
      data,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    }).then(response => {
      return response.data
    })
  },

  async get (resource, data) {
    return this.execute('GET', resource, data)
  },

  async post (resource, data) {
    return this.execute('POST', resource, data)
  },

  async put (resource, data) {
    return this.execute('PUT', resource, data)
  },

  async delete (resource) {
    return this.execute('DELETE', resource)
  }
}
