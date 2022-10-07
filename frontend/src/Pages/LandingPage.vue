<script setup lang="ts">

import {ref} from "vue";
import RegisterTeam from "./RegisterTeam.vue";
import Results from "./Results.vue";

const showRegister = ref(false);
const showResults = ref(true);

</script>

<template>
  <RegisterTeam v-if="showRegister" />

  <div v-else >
    <div class="mx-auto">
      <div class="relative z-10">
        
        <div class="px-4 pt-10">
          
            <img class="rounded-3xl object-cover object-center max-w-full p-5" src="/HS22-Logo_Coloured.png" alt="" />
          
        </div>
      </div>
      
      <div class="mx-auto space-y-6 py-6 px-4">
        <h2 class="text-3xl font-bold tracking-tight text-accept2 text-center" id="join-heading"><span class="text-accent">QR-løp:</span> Jorden rundt</h2>
        <p class="text-l text-brown">Ta med noen av vennene dine og bli med mellom 1230 og 1600, og reis <span class="text-accent">jorden rundt</span>! </p><p>Reisen tar ca. 1.5 timer - start når du vil!</p>
        <blockquote class="border-t text-brown border-interactive py-1 mb-1 mt-10 p-4 my-4 bg-gray-50 border-l-4 border-gray-300 dark:border-gray-500 dark:bg-gray-800">Tips: Menigheten din scorer basert på poeng samlet på postene OG hvor høy oppslutning det er fra din menighet.</blockquote>
        <a class="block w-full rounded-md bg-interactive text-xl py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-interactive_active active:text-white" @click="showRegister = true">Kom i gang!</a>
        <div v-if="showResults">
          <h3 class="border-t text-center border-interactive py-1 mb-1 mt-20 font-bold text-xl text-accept2">Resultater</h3>
          <Results/>
        </div>
      </div>
    </div>
  </div>

</template>

