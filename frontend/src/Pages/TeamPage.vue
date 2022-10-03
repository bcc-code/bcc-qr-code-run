<script setup lang="ts">

import Status from "./Status.vue";
import Results from "./Results.vue";
import { store } from "../api/teamApi";
import Trivia from "./Trivia.vue";
import Map from "./Map.vue";
import {ref} from "vue";
import ScanQrCode from "./ScanQrCode.vue";

import { TrophyIcon, MapIcon, QrCodeIcon, SparklesIcon, StarIcon, UserGroupIcon, GlobeEuropeAfricaIcon, ArrowLeftOnRectangleIcon} from '@heroicons/vue/24/solid'

import {logout} from "../api/teamApi";

const selectedTab = ref(1);

const showQrScan = ref(false);

function setTab (tab: number) {
  selectedTab.value
}



</script>

<template>
  <div class="flex flex-col h-screen relative">
    
    

    <div class="text-4xl font-bold text-accept2 text-center py-5 px-4 drop-shadow">
      <h1 class="text-accept2">Jorden Rundt </h1>
    </div>
    <div class="absolute h-7 w-7 text-right right-14 top-7 text-accept2" @click="logout">
      <ArrowLeftOnRectangleIcon />
    </div>
    
    <template v-if="showQrScan">
      <ScanQrCode @close="showQrScan=false"/>
    </template>
    <template v-else>

      <div class="px-10 py-3">
        <div class="grid grid-cols-4 gap-4">
          <div @click="selectedTab = 1"><UserGroupIcon  :class="`h-12 w-12 ${selectedTab == 1 ? 'text-accent' : 'text-accept2'}`"/></div>
          <div @click="selectedTab = 3"><MapIcon :class="`h-12 w-12 ${selectedTab == 3 ? 'text-accent' : 'text-accept2'}`"/></div> 
          <div @click="selectedTab = 4"><GlobeEuropeAfricaIcon :class="`h-12 w-12 ${selectedTab == 4 ? 'text-accent' : 'text-accept2'}`"/></div>            
          <div @click="selectedTab = 2"><TrophyIcon :class="`h-12 w-12 ${selectedTab == 2 ? 'text-accent' : 'text-accept2'}`"/></div>             
        </div>
        
        <select v-if="false" v-model="selectedTab" class="w-full rounded-md bg-interactive text-brown text-xl font-bold">
          <option class="font-medium text-base" :value="1">Status</option>
          <option class="font-medium text-base" :value="2">Resultater</option>
          <option class="font-medium text-base" :value="3">Kart</option>
          <option class="font-medium text-base" :value="4">Triva</option>
        </select>
      </div>
  
      <div class="px-10 py-5 space-y-4 mb-3 h-full overflow-y-scroll scrollbox">
          <Status v-if="selectedTab===1"/>
          <Results v-if="selectedTab===2"/>
          <Map v-if="selectedTab===3" />
          <Trivia v-if="selectedTab===4"/>
      </div>
  
      <div class="flex-1 flex items-end px-4 pb-5">
        <button class="block w-full rounded-md bg-interactive py-3 px-5 text-center text-xl font-medium text-brown shadow-md active:bg-interactive_active active:text-white"
        @click="showQrScan=true">Skann QR-kode</button>
      </div>

    </template>

  </div>
  
</template>

<style>
.scrollbox {
  /*background:
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