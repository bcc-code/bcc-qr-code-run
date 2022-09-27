<script setup lang="ts">
import LandingPage from "./Pages/LandingPage.vue";
import TeamPage from "./Pages/TeamPage.vue";
import {store, getLoggedInTeam} from "./api/teamApi";

getLoggedInTeam()

</script>
<template>
<!--  <TeamPage/>-->
<div class="">
  <div class="img-background min-h-screen max-w-md m-auto">
    <TeamPage v-if="store.isLoggedIn"></TeamPage>
    <LandingPage v-else/>
  <!--  <div class="container mx-auto">-->
  <!--    <div class="flex flex-col">-->
  <!--      <div class="text-3xl py-5">Hello world</div>-->
  <!--      <div class="flex">-->
  <!--        <button class="bg-interactive rounded text-white font-bold text-lg py-2 px-5" @click="login">Login</button>-->
  <!--        <button class="bg-interactive rounded text-white font-bold text-lg py-2 px-5" @click="get">Get</button>-->
  <!--        <button class="bg-interactive rounded text-white font-bold text-lg py-2 px-5" @click="logout">Logout</button>-->
  <!--      </div>-->
  <!--      <pre>{{reply}}</pre>-->

  <!--    </div>-->
  <!--  </div>-->
  </div>
</div>
</template>

<style scoped>
  .img-background {
    background-image: url('/HS22-BG.jpg');
  }
</style>