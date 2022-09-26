<script setup lang="ts">

import {getResults, Result} from "../api/resultsApi";
import {ref} from "vue";

const results = ref<Result[]>([])
getResults().then(x => results.value = x)

</script>

<template>
  <div>
    <dl class="text-xl text-brown grid grid-cols-[1fr_auto]">
      <template v-for="result in results" :key="result.church">
        <dt>{{result.church}}</dt>
        <dd>{{result.points}} poeng</dd>
      </template>
    </dl>
  </div>
</template>