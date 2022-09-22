<script setup lang="ts">
import { ref } from "vue";
import { FunFact, getTrivia } from "../api/triviaApi";

const results = ref<FunFact[]>([])
getTrivia().then(x => results.value = x)

</script>
    
<template>
  <div v-if="results?.length">
    <div v-for="result in results" :key="result.title">
      <div class="text-2xl text-white">{{result.title}}</div>
      <div class="mt-3 text-xl text-white">{{result.content}}</div>
    </div>
  </div>
  <div v-else class="font-bold text-2xl text-white my-auto h-full flex">
    <div class="my-auto">
      Du har ikke skannet noen QR koder enda
    </div>
  </div>
</template>