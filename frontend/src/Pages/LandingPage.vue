<script setup lang="ts">

import {ref} from "vue";
import RegisterTeam from "./RegisterTeam.vue";
import Results from "./Results.vue";

const showRegister = ref(false);
const showResults = ref(false);

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
        <p class="text-lg text-brown">Ta med noen av vennene dine og bli med på en reise <span class="text-accent">jorden rundt</span>!</p>
        <p> Blir det <i>din</i> menighet som har best oppslutning?</p>
        <a class="block w-full rounded-md bg-interactive text-xl py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-interactive_active active:text-white" @click="showRegister = true">Kom i gang!</a>
        <h2 class="text-2xl font-bold text-accent tracking-tight">Resultater</h2>
        <Results/>
      </div>
    </div>
  </div>

</template>

