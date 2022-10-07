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
        
        <div class="px-4 pt-0">
          
            <img class="rounded-3xl object-cover object-center max-w-full p-5" src="/HS22-Logo_Coloured.png" alt="" />
          
        </div>
      </div>
      
      <div class="mx-auto space-y-6 pb-6 px-4">
        <h2 class="text-3xl font-bold tracking-tight text-accept2 text-center" id="join-heading"><span class="text-accent text-base">QR-LØP</span><br> JORDEN RUNDT</h2>
        <p class="text-l text-brown text-center">Ta med noen av vennene dine mellom<br> 12:30 og 17:30 og reis <i>jorden rundt</i>! </p><p class="text-center">Reisen tar ca. 1,5 time - start når du vil!</p>
        <blockquote class="border-t text-accent border-interactive py-1 mb-1 mt-10 p-4 my-4 bg-gray-50 border-l-4 border-gray-300 dark:border-gray-500 dark:bg-gray-800"><b>Tips:</b> Menigheten din scorer basert på poeng samlet på postene OG hvor høy oppslutning det er fra din menighet.</blockquote>
        <a class="block w-full rounded-md bg-interactive text-xl py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-interactive_active active:text-white" @click="showRegister = true">KOM I GANG</a>
        <div v-if="showResults">
          <h3 class="border-t text-center border-interactive py-1 mb-1 mt-20 font-bold text-xl text-accept2">Resultater</h3>
          <Results/>
        </div>
      </div>
    </div>
  </div>

</template>

