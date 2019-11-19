<template>
  <ul class="list-inline mb-0 meal-icons">
    <li class="list-inline-item">
      <font-awesome-icon v-if="meal.isKidFriendly" icon="child" class="text-primary" title="Kid friendly" />
    </li>
    <li class="list-inline-item">
      <font-awesome-icon v-if="meal.isFavorite" icon="star" class="text-warning" title="Favorite" />
    </li>
  </ul>
</template>

<script>
export default {
  props: {
    meal: {
      required: true
    }
  }
}
</script>
