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
          <th class="py-2">Menighet<br><p class="text-xs">&nbsp;</p></th>
          <th class="text-center">Poeng<br><p class="text-xs">gj. snitt</p></th>
          <th class="text-center px-2">Oppslutning<br><p class="text-xs">ant. deltakere</p></th>
          <th class="text-center">Score<br><p class="text-xs">&nbsp;</p></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="result in results" :key="result.church">
          <td class="pt-1">{{result.church}}<br><small>{{result.country}}</small></td>
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