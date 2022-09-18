<script setup lang="ts">

import Status from "./Status.vue";
import Results from "./Results.vue";
import { store } from "../api/teamApi";

</script>

<template>
  <div class="flex flex-col bg-background h-screen">
    <div class="text-4xl font-bold text-brown bg-light_background py-5 px-4 drop-shadow">
      <h1 class="drop-shadow shadow-accent">Jorden Rundt med</h1>
      <h1 class="text-accent">{{store.team?.teamName}}</h1>
    </div>

    <div class="px-10 py-5 space-y-4">
      <div class="text-3xl font-bold text-white drop-shadow-md">
        Status
      </div>
      
      <Status/>
      
    </div>

    <div class="flex-1 flex items-end px-4 pb-20">
      <a class="block w-full rounded-md bg-interactive py-3 px-5 text-center text-xl font-medium text-brown shadow-md active:bg-accept2 active:text-white" href="#">Skann QR Kode</a>
    </div>

  </div>
</template>