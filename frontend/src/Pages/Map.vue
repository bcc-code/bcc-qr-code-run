<script setup lang="ts">

import {ref} from "vue";
import {store} from "../api/teamApi";

const emits = defineEmits(["close"])


</script>

<template>
  <div>
    
      <img src="../../public/kart_liggende.png">

      <h3 class="border-t border-b border-interactive py-1 mb-1 mt-5 font-bold text-xl text-accept2">Poster</h3>
      <ol>
        <li>1. <b>Europa</b> - partytelt ved festplassen</li>
        <li>2. <b>Asia</b> - hus 33</li>
        <li>3. <b>Nord- og søramerika</b> - fotballbanen</li>
        <li>4. <b>Afrika</b> - økonomigården</li>
        <li>5. <b>Oseania</b> - foran arenaen</li>
        <li>6. <b>Antarktis</b> - amfi</li>
      </ol>

      <h3 class="border-t border-b border-interactive py-1 mb-1 mt-5 font-bold text-xl text-accept2">Skjulte skatter</h3>
      <div>Det finnes 4 skjulte poster og 4 hemmelige venner med QR-kode som dere kan scanne for å få bonuspoeng!   </div>

  </div>
</template>
