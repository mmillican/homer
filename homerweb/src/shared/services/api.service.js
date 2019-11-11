// import Vue from 'vue'
import axios from 'axios'

const client = axios.create({
  baseURL: 'https://localhost:44362/api/',
  json: true
})

// TODO: Interceptors

export default {
  async execute (method, resource, data) {
    // TODO: Access token

    return client({
      method,
      url: resource,
      data,
      headers: { }
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
