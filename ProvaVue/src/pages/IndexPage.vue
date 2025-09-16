<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md" style="justify-content: flex-end">
      <q-btn
        color="positive"
        icon="add"
        label="Cadastrar"
        @click="$router.push('/cadastrar')"
        style="background: #21ba45; color: white"
      />
    </div>
    <ListarHerois />
  </q-page>
</template>

<script setup lang="ts">
import ListarHerois from 'components/ListarHerois.vue';
</script>
