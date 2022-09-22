<script setup lang="ts">

import Status from "./Status.vue";
import Results from "./Results.vue";
import { store } from "../api/teamApi";
import Trivia from "./Trivia.vue";
import {ref} from "vue";

const selectedTab = ref(1);

</script>

<template>
  <div class="flex flex-col bg-background h-screen">
    <div class="text-4xl font-bold text-brown bg-light_background py-5 px-4 drop-shadow">
      <h1 class="drop-shadow shadow-accent">Jorden Rundt med</h1>
      <h1 class="text-accent">{{store.team?.teamName}}</h1>
    </div>

    <div class="px-10 py-5 space-y-4">
      <div class="drop-shadow-md">
        <select v-model="selectedTab" class="w-full rounded-md bg-interactive text-brown text-2xl font-bold">
          <option class="font-medium text-base" :value="1">Status</option>
          <option class="font-medium text-base" :value="2">Resultater</option>
          <option class="font-medium text-base" :value="3">Kart</option>
          <option class="font-medium text-base" :value="4">Triva</option>
        </select>
      </div>

      

      <Status v-if="selectedTab===1"/>
      <Results v-if="selectedTab===2"/>
      <div v-if="selectedTab===3">Map</div>
      <Trivia v-if="selectedTab===4"/>
      
    </div>

    <div class="flex-1 flex items-end px-4 pb-20">
      <a class="block w-full rounded-md bg-interactive py-3 px-5 text-center text-xl font-medium text-brown shadow-md active:bg-accept2 active:text-white" href="#">Skann QR Kode</a>
    </div>

  </div>
</template>