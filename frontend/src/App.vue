<script setup lang="ts">
import {ref} from "vue";
import LandingPage from "./Pages/LandingPage.vue";
import RegisterTeam from "./Pages/RegisterTeam.vue";
import TeamPage from "./Pages/TeamPage.vue";

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
  }
  catch (e){ }
}

const get = async () => {
    const result = await fetch('https://localhost:7065/team/');
    reply.value = await result.json();
}


const logout = async () => {
  try {
    const result = await fetch('https://localhost:7065/team/logout', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    });
    reply.value = await result.json();
  }
  catch (e){ }
}

</script>
<template>
  <TeamPage/>
<!--  <div class="container mx-auto">-->
<!--    <div class="flex flex-col">-->
<!--      <div class="text-3xl py-5">Hello world</div>-->
<!--      <div class="flex">-->
<!--        <button class="bg-blue-500 rounded text-white font-bold text-lg py-2 px-5" @click="login">Login</button>-->
<!--        <button class="bg-green-500 rounded text-white font-bold text-lg py-2 px-5" @click="get">Get</button>-->
<!--        <button class="bg-red-400 rounded text-white font-bold text-lg py-2 px-5" @click="logout">Logout</button>-->
<!--        <pre>{{reply}}</pre>-->
<!--      </div>-->
<!--    </div>-->
<!--  </div>-->
</template>