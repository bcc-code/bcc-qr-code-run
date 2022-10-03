<script setup lang="ts">
import { store } from "../api/teamApi";
import {ChurchResult, myChurch} from "../api/resultsApi";
import {ref} from "vue";

const churchResult = ref<ChurchResult>()
myChurch().then(x=> churchResult.value = x)

</script>

<template>
  <div>
    <h3 class="border-t border-b border-interactive py-1 mb-1 mt-0 font-bold text-xl text-accent">{{store.team?.teamName}}</h3>
    <dl class="text-l text-accent grid grid-cols-[1fr_auto] gap-3" v-if="store.team">
      <dt>Poeng</dt>
      <dd>{{store.team.score}}</dd>

      <dt>Poster</dt>
      <dd>{{store.team.posts.length}}</dd>

      <dt>Tid</dt>
      <dd>{{store.team.timeSpent}}</dd>

      <dt>Skjulte skatter</dt>
      <dd>{{store.team.secretsFound.length}}</dd>
    </dl>

    <template v-if="churchResult">
      <h3 class="border-t border-b border-interactive py-1 mb-1 mt-5 font-bold text-xl text-accept2">{{ churchResult.church }}</h3>
  
      <dl class="text-l text-accept2 grid grid-cols-[1fr_auto] gap-3">
        <dt>Lag</dt>
        <dd>{{churchResult.teams }}</dd>
  
        <dt>Gjennomsnitt poeng</dt>
        <dd>{{churchResult.averagePoints}}</dd>
  
        <dt>Total Tid</dt>
        <dd>{{churchResult.timeSpent}}</dd>
  
        <dt>Skjulte skatter</dt>
        <dd>{{churchResult.secretsFound}}</dd>

        <dt>Oppslutning</dt>
        <dd>{{churchResult.participation}}%</dd>
      </dl>
    </template>
  </div>
</template>