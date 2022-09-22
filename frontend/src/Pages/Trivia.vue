<script setup lang="ts">
import { ref } from "vue";
import { FunFact, getTrivia } from "../api/triviaApi";

const results = ref<FunFact[]>([])
getTrivia().then(x => results.value = x)

</script>
    
<template>
  <div v-if="results?.length">
    <div v-for="result in results" :key="result.title">
      <div>{{result.title}}</div>
      <div>{{result.content}}</div>
    </div>
  </div>
  <div v-else class="font-bold text-2xl text-white">
    Du har ikke skannet noen QR koder enda
  </div>
</template>