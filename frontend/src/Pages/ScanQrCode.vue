<script setup lang="ts">

import { QrcodeStream } from 'vue3-qrcode-reader'
import {QrCodeResult, scanCode} from "../api/qrCodeApi";
import {ref} from "vue";
import {store} from "../api/teamApi";

const emits = defineEmits(["close"])
const scannedQrCode = ref<QrCodeResult|undefined>()
const processing = ref(false);
const errorMessage = ref('')



function isQrCode(result: string | QrCodeResult): result is QrCodeResult {
  return (result as QrCodeResult).funFact !== undefined;
}

async function onDecode(decodedString: string) {
  console.log(decodedString);
  processing.value = true;
  const result = await scanCode(decodedString);
  processing.value = false;
  console.log("Result: " + result)
  if(isQrCode(result)) {
    console.log(result)
    scannedQrCode.value = result;
    store.team = result.team;
  }
  else {
    errorMessage.value = result
  }
}

</script>

<template>
  <div>
    <div class="px-10 py-5">
      <div class="text-brown text-3xl font-bold flex items-center" @click="$emit('close')">
        <svg class="aspect-1 w-5 mr-2 fill-white">
          <path d="M15.41 7.41 14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path>
        </svg>
        <span class="flex-1 block w-full text-xl rounded-md bg-interactive py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-interactive_active active:text-white">Tilbake</span>
        
      </div>
    </div>
    
  <!-- <button @click="onDecode('eyJRc3kNvZGVJZCI6MX0=')">scan</button> -->
    
    <div class="px-10 py-5" v-if="errorMessage">
      <div class="text-3xl text-accent font-bold">{{ errorMessage }}</div>
    </div>
    <qrcode-stream @decode="onDecode" v-else-if="!scannedQrCode"/>
    
    <div v-else class="px-10 py-5">
      <div class="text-4xl text-brown font-bold">Gratulerer!</div>
      <div class="text-brown text-2xl mt-5">{{scannedQrCode.points}} poeng</div>
      
      <div class="mt-10 text-2xl text-brown">{{scannedQrCode.funFact.title}}</div>
      <div class="mt-3 text-xl text-brown" v-html="scannedQrCode.funFact.content"></div>
    </div>
  </div>
</template>
