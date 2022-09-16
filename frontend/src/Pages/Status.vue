<script setup lang="ts">
import { store } from "../Composables/UseLogin";

</script>

<template>
  <div>
    <dl class="text-2xl text-white grid grid-cols-[1fr_auto] gap-3">
      <dt>Poeng</dt>
      <dd>{{store.team?.score}}</dd>

      <dt>Poster</dt>
      <dd>{{store.team?.posts.length}}</dd>

      <dt>Tid</dt>
      <dd>{{store.team?.timeSpent}}</dd>

      <dt>Skjulte Skatter</dt>
      <dd>{{store.team?.secretsFound.length}}</dd>
    </dl>

    <h3 class="border-t border-interactive pt-5 mt-5 font-bold text-3xl text-white">Terwolde</h3>

    <dl class="text-2xl text-white grid grid-cols-[1fr_auto] gap-3">
      <dt>Lag</dt>
      <dd>8</dd>

      <dt>Poeng</dt>
      <dd>43</dd>

      <dt>Total Tid</dt>
      <dd>08:40</dd>

      <dt>Skjulte Skatter</dt>
      <dd>12</dd>
    </dl>
  </div>
</template>