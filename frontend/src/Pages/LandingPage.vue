<script setup lang="ts">

import {ref} from "vue";
import RegisterTeam from "./RegisterTeam.vue";

const showRegister = ref(false);

</script>

<template>
  <RegisterTeam v-if="showRegister" />

  <div v-else class="img-background min-h-screen">
    <div class="mx-auto">
      <div class="relative z-10">
        
        <div class="px-4 pt-10">
          
            <img class="rounded-3xl object-cover object-center max-w-full p-5" src="HS22-Logo_Coloured.png" alt="" />
          
        </div>
      </div>

      <div class="mx-auto space-y-6 py-6 px-4">
        <h2 class="text-3xl font-bold tracking-tight text-accept2 text-center" id="join-heading"><span class="text-accent">QR-løp:</span> Jorden rundt</h2>
        <p class="text-lg text-brown">Bli med på en reise jorden rundt. Varius facilisi mauris sed sit. Non sed et duis dui leo, vulputate id malesuada non. Cras aliquet purus dui laoreet diam sed lacus, fames.</p>
        <a class="block w-full rounded-md bg-interactive py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-interactive_active active:text-white" @click="showRegister = true">Registrer deg</a>
      </div>
    </div>
  </div>

</template>

<style scoped>
  .img-background {
    background-image: url('hs22-bg.jpg');
  }
</style>