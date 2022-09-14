<script setup lang="ts">
  
</script>

<template>
  <div class="flex flex-col bg-background h-screen">
    <div class="text-4xl font-bold text-brown bg-light_background py-5 px-4 drop-shadow">
      <h1 class="drop-shadow shadow-accent">Jorden Rundt</h1>
    </div>
    
    <div class="mx-auto px-4 py-5 space-y-4">
      <div class="text-3xl font-bold text-white drop-shadow-md">
        Registrer team
      </div>
      <div class="text-white text-2xl drop-shadow-md">
        Fyll inn team-navn, menighet og antall deltakere 
      </div>
      
      <form class="space-y-3">
        <div class="space-y-2">
          <label for="name" class="block text-sm font-medium text-white">Navn</label>
          <input type="text" name="name" id="name" class="block rounded-md w-full border-0 px-3 py-2 shadow-sm focus-within:shadow-lg transition-shadow focus:ring-2 focus:ring-accent focus:ring-2" />
        </div>

        <div class="space-y-2">
          <label for="church" class="block text-sm font-medium text-white">Menighet</label>
          <select id="church" name="church" class="mt-1 block w-full rounded-md border-0 py-2 pl-3 pr-10 text-base focus:outline-none focus:ring-2 focus:ring-accent">
            <option disabled selected>-- Velg menighet --</option>
            <option>Oslo</option>
            <option>Bergen</option>
            <option>Sandefjord</option>
          </select>
        </div>

        <div class="space-y-2">
          <label for="members" class="block text-sm font-medium text-white">Deltakere</label>
          <input type="number" name="members" id="members" class="block rounded-md w-full border-0 px-3 py-2 shadow-sm focus-within:shadow-lg transition-shadow focus:ring-2 focus:ring-accent focus:ring-2" />
        </div>
      </form>
      
    </div>
    
    <div class="flex-1 flex items-end px-4 pb-20">
      <a class="block w-full rounded-md bg-interactive py-3 px-5 text-center text-base font-medium text-brown shadow-md active:bg-accept2 active:text-white" href="#">Explore open positions</a>
    </div>

  </div>
</template>