<script setup lang="ts">

import {getResults, ChurchResult} from "../api/resultsApi";
import {ref} from "vue";

const results = ref<ChurchResult[]>([])
getResults().then(x => results.value = x)

</script>

<template>
  <div>
    <table class="table-auto w-full text-brown">
      <thead>
        <tr class="border-t border-b border-interactive text-l text-accent text-left">
          <th class="py-2">Menighet</th>
          <th class="text-center">Poeng</th>
          <th class="text-center">Oppslutning</th>
          <th class="text-center">Score</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="result in results" :key="result.church">
          <td class="pt-1">{{result.church}}, {{result.country}}</td>
          <td class="text-center">{{result.averagePoints}}</td>
          <td class="text-center">{{result.participation}}%</td>
          <td class="text-center"><b>{{result.score}}</b></td>
        </tr>
      </tbody>
    </table>
    <dl class=" grid grid-cols-[1fr_auto]">

    </dl>
  </div>
</template>