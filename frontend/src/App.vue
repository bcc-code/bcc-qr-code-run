<script setup lang="ts">
import {ref} from "vue";

const reply = ref()

const login = async () => {
  try {
    const result = await fetch('https://localhost:7065/team/register', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "groupCode": "string",
        "teamName": "string",
        "teamMemberCount": 0})
    });
    reply.value = await result.json();
  }
  catch (e){
    
  }
    
}
</script>
<template>
  <div class="container mx-auto">
    <div class="flex flex-col">
      <div class="text-3xl py-5">Hello world</div>
      <div class="flex">
        <button class="bg-blue-500 rounded text-white font-bold text-lg py-2 px-5" @click="login">Login</button>
        <pre>{{reply}}</pre>
      </div>
    </div>
  </div>
</template>