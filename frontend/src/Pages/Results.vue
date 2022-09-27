<script setup lang="ts">

import {getResults, Result} from "../api/resultsApi";
import {ref} from "vue";

const results = ref<Result[]>([])
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
      <template v-for="result in results" :key="result.church">
        <tr >
          <td class="pt-1">{{result.church}}</td>
          <td class="text-center">{{result.points}}</td>
          <td class="text-center">30%</td>
          <td class="text-center"><b>{{result.points*Math.min(0.3,1)*100}}</b></td>
        </tr>
      </template>
      </tbody>
    </table>
    <dl class=" grid grid-cols-[1fr_auto]">

    </dl>
  </div>
</template>