<script setup lang="ts">
import {login, store} from "../api/teamApi";
import {getChurches} from "../api/churchApi";
import {ref} from "vue";

const churches = ref<string[]>([])

getChurches().then(p => churches.value = p);

const teamName = ref('');
const churchName = ref('');
const members = ref(1);

async function join() {
  try {
    const result = await login(teamName.value, churchName.value, members.value);
    if(!result) {
        // todo: Validation?
      }
  } catch (error) { 
  }
}

</script>

<template>
  <div class="flex flex-col  min-h-screen">
    <div class="text-3xl font-bold text-accept2  py-5 px-4 drop-shadow">
      <h1 class="color-accent text-center">JORDEN RUNDT</h1>
    </div>
    
    <div class="mx-auto px-4 py-5 space-y-4">
      <div class="text-2xl font-bold text-brown text-center small-caps drop-shadow-md">
        Registrer lag
      </div>
      <div class="text-brown text-l drop-shadow-md">
        Fyll inn et unikt lagnavn, menighet og antall deltakere.
      </div>
      
      <form class="space-y-3">
        <div class="space-y-2">
          <label for="name" class="block text-sm font-medium text-brown">Unikt lagnavn</label>
          <input required v-model="teamName" type="text" name="name" id="name" class="block rounded-md w-full border-0 px-3 py-2 shadow-sm focus-within:shadow-lg transition-shadow focus:ring-2 focus:ring-accent focus:ring-2" />
        </div>

        <div class="space-y-2">
          <label for="church" class="block text-sm font-medium text-brown">Menighet</label>
          <select v-model="churchName" id="church" name="church" class="mt-1 block w-full rounded-md border-0 py-2 pl-3 pr-10 text-base focus:outline-none focus:ring-2 focus:ring-accent">
            <option disabled selected>-- Velg menighet --</option>
            <option v-for="church in churches" :key="church">{{ church }}</option>
          </select>
        </div>

        <div class="space-y-2">
          <label for="members" class="block text-sm font-medium text-brown">Deltakere på laget (1-5)</label>
          <input required v-model="members" type="number" min="1" max="5" name="members" id="members" class="block rounded-md w-full border-0 px-3 py-2 shadow-sm focus-within:shadow-lg transition-shadow focus:ring-2 focus:ring-accent focus:ring-2" />
        </div>
      </form>

      <div class="px-10 py-5" v-if="store.errorMessage">
        <div class="text-3xl text-accent font-bold">{{ store.errorMessage }}</div>
      </div>
      
    </div> 
    
    <div class="flex-1 flex items-end px-4 pb-20">
      <a class="block w-full text-xl rounded-md bg-interactive py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-interactive_active active:text-white" @click="join">Bli med</a>
    </div>

  </div>
</template>