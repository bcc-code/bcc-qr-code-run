<script setup lang="ts">

import { QrcodeStream } from 'vue3-qrcode-reader'
import {QrCodeResult, scanCode} from "../api/qrCodeApi";
import {ref} from "vue";
import {store} from "../api/teamApi";

const emits = defineEmits(["close"])
const scannedQrCode = ref<QrCodeResult|undefined>()
const processing = ref(false);



function isQrCode(result: number | QrCodeResult): result is QrCodeResult {
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
    console.log("Result is: " + result)
  }
}

</script>

<template>
  <div>
    <div class="px-10 py-5">
      <div class="text-interactive text-3xl font-bold" @click="$emit('close')">
        Tilbake
      </div>
    </div>
    
<!--    <button @click="onDecode('eyJRckNvZGVJZCI6MX0=')">scan</button>-->
    
    <qrcode-stream @decode="onDecode" v-if="!scannedQrCode"/>
    <div v-else class="px-10 py-5">
      <div class="text-4xl text-white font-bold">Gratulerer!</div>
      <div class="text-white text-2xl mt-5">{{scannedQrCode.points}} poeng</div>
      
      <div class="mt-10 text-2xl text-white">{{scannedQrCode.funFact.title}}</div>
      <div class="mt-3 text-xl text-white">{{scannedQrCode.funFact.content}}</div>
    </div>
  </div>
</template>
