<script setup lang="ts">
import { ref } from "vue";
import { FunFact, getTrivia } from "../api/triviaApi";

const results = ref<FunFact[]>([])
getTrivia().then(x => results.value = x)

</script>
    
<template>
  <div v-if="results?.length">
    <div v-for="result in results" :key="result.title">
      <div class="text-2xl text-brown">{{result.title}}</div>
      <div class="mt-3 mb-4 text-l text-brown" v-html="result.content"></div>
    </div>
    <hr class="border-1">
  </div>
  <div v-else class="font-bold text-xl text-brown my-auto h-full flex">
    <div class="my-auto">
      Du har ikke skannet noen QR koder enda
    </div>
  </div>
</template>