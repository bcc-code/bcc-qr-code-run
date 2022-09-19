<script setup lang="ts">
import { store } from "../api/teamApi";
import {ChurchResult, myChurch} from "../api/resultsApi";
import {ref} from "vue";

const churchResult = ref<ChurchResult>()
myChurch().then(x=> churchResult.value = x)

</script>

<template>
  <div>
    <dl class="text-2xl text-white grid grid-cols-[1fr_auto] gap-3" v-if="store.team">
      <dt>Poeng</dt>
      <dd>{{store.team.score}}</dd>

      <dt>Poster</dt>
      <dd>{{store.team.posts.length}}</dd>

      <dt>Tid</dt>
      <dd>{{store.team.timeSpent}}</dd>

      <dt>Skjulte Skatter</dt>
      <dd>{{store.team.secretsFound.length}}</dd>
    </dl>

    <template v-if="churchResult">
      <h3 class="border-t border-interactive pt-5 mt-5 font-bold text-3xl text-white">{{ churchResult.churchName }}</h3>
  
      <dl class="text-2xl text-white grid grid-cols-[1fr_auto] gap-3">
        <dt>Lag</dt>
        <dd>{{churchResult.teams }}</dd>
  
        <dt>Poeng</dt>
        <dd>{{churchResult.score}}</dd>
  
        <dt>Total Tid</dt>
        <dd>{{churchResult.timeSpent}}</dd>
  
        <dt>Skjulte Skatter</dt>
        <dd>{{churchResult.secretsFound}}</dd>
      </dl>
    </template>
  </div>
</template>