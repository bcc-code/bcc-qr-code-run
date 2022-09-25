<script setup lang="ts">

import Status from "./Status.vue";
import Results from "./Results.vue";
import { store } from "../api/teamApi";
import Trivia from "./Trivia.vue";
import {ref} from "vue";
import ScanQrCode from "./ScanQrCode.vue";

const selectedTab = ref(1);

const showQrScan = ref(false);

</script>

<template>
  <div class="flex flex-col bg-background h-screen">
    <div class="text-4xl font-bold text-brown bg-light_background py-5 px-4 drop-shadow">
      <h1 class="drop-shadow shadow-accent">Jorden Rundt med</h1>
      <h1 class="text-accent">{{store.team?.teamName}}</h1>
    </div>
    
    <template v-if="showQrScan">
      <ScanQrCode @close="showQrScan=false"/>
    </template>
    <template v-else>

      <div class="px-10 py-5">
        <select v-model="selectedTab" class="w-full rounded-md bg-interactive text-brown text-2xl font-bold">
          <option class="font-medium text-base" :value="1">Status</option>
          <option class="font-medium text-base" :value="2">Resultater</option>
          <option class="font-medium text-base" :value="3">Kart</option>
          <option class="font-medium text-base" :value="4">Triva</option>
        </select>
      </div>
  
      <div class="px-10 py-5 space-y-4 mb-3 h-full overflow-y-scroll scrollbox">
          <Status v-if="selectedTab===1"/>
          <Results v-if="selectedTab===2"/>
          <div v-if="selectedTab===3">Map</div>
          <Trivia v-if="selectedTab===4"/>
      </div>
  
      <div class="flex-1 flex items-end px-4 pb-20">
        <button class="block w-full rounded-md bg-interactive py-3 px-5 text-center text-xl font-medium text-brown shadow-md active:bg-interactive_active active:text-white"
        @click="showQrScan=true">Skann QR Kode</button>
      </div>

    </template>

  </div>
  
</template>

<style>
.scrollbox {
  background:
      linear-gradient(theme("colors.background") 30%, rgba(255, 255, 255, 0)),
      linear-gradient(rgba(255, 255, 255, 0), theme("colors.background") 30%) 0 100%,
      linear-gradient(rgba(0,0,0,.2) 30%, rgba(255, 255, 255, 0)),
      linear-gradient(rgba(255, 255, 255, 0), rgba(0,0,0,.2)) 0 100%;

/*linear-gradient(rgba(0,0,0,.2) 30%, rgba(255, 255, 255, 0)),*/
/*linear-gradient(rgba(255, 255, 255, 0) 30%, rgba(0,0,0,0.2)) 0 100%;*/
  
  background-repeat: no-repeat;
  background-size: 100% 40px, 100% 40px, 100% 14px, 100% 14px;
  /*!* Opera doesn't support this in the shorthand *!*/
  background-attachment: local, local, scroll, scroll;
}
</style>