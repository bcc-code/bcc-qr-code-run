<script setup lang="ts">
import { ref } from "vue";
import { FunFact, getTrivia } from "../api/triviaApi";

const results = ref<FunFact[]>([])
getTrivia().then(x => results.value = x)

</script>
    
<template>
  <div v-if="results?.length">
    <div v-for="result in results" :key="result.title">
      <h3 class="border-t border-b border-interactive py-1 mb-1 mt-5 font-bold text-xl text-accept2">{{result.title}}</h3>
      <div class="mt-3 mb-4 text-brown content-block" v-html="result.content"></div>
    </div>
  </div>
  <div v-else class="font-bold text-xl text-brown my-auto h-full flex">
    <div class="my-auto">
      Du har ikke skannet noen QR koder enda
    </div>
  </div>
</template>
<style>
  .content-block ul  {
    list-style: disc;
    margin-left: 1em;
  }
  .content-block ul li  {
    margin-bottom: 0.3em;
    margin-top: 0.3em;
  }
</style>