<template>
  <q-page class="q-pa-md">
    <q-card v-if="form">
      <q-card-section class="row items-center justify-between">
        <div class="text-h6">Editar Herói</div>
        <q-btn flat color="primary" icon="arrow_back" label="Voltar" @click="router.push('/')" />
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-form @submit.prevent="editarHeroi">
          <q-input v-model="form.nome" label="Nome" required class="q-mb-md" />
          <q-input v-model="form.nomeHeroi" label="Nome do Herói" required class="q-mb-md" />
          <q-input
            v-model="form.dataNascimento"
            label="Data de Nascimento"
            type="date"
            required
            class="q-mb-md"
          />
          <q-input
            v-model="form.altura"
            label="Altura (ex: 1,75)"
            mask="#.##"
            fill-mask="0"
            reverse-fill-mask
            required
            class="q-mb-md"
            hint="Use vírgula para decimais"
          />
          <q-input
            v-model="form.peso"
            label="Peso (kg)"
            type="number"
            min="0"
            step="0.01"
            required
            class="q-mb-md"
            hint="Informe o peso em quilos, use ponto para gramas (ex: 70.5)"
          />
          <q-select
            v-model="form.superpoderesIds"
            :options="superpoderes"
            option-label="superpoder"
            option-value="id"
            label="Superpoderes"
            multiple
            emit-value
            map-options
            class="q-mb-md"
          />
          <q-btn type="submit" color="primary" label="Salvar" :loading="loading" />
        </q-form>
      </q-card-section>
    </q-card>
    <q-card v-else>
      <q-card-section>
        <q-spinner color="primary" size="2em" />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { api } from '../boot/axios';
import type { Superpoder, FormHeroi } from '../interfaces/heroi';
import { useQuasar } from 'quasar';

const $q = useQuasar();
const route = useRoute();
const router = useRouter();
const loading = ref(false);
const superpoderes = ref<Superpoder[]>([]);
const form = ref<FormHeroi | null>(null);

async function carregarSuperpoderes() {
  const { data } = await api.get<Superpoder[]>('/api/Herois/superpoderes');
  superpoderes.value = data;
}

async function carregarHeroi() {
  const idParam = Array.isArray(route.params.id) ? route.params.id[0] : route.params.id;
  const { data } = await api.get(`/api/Herois/${idParam}`);
  form.value = {
    ...data,
    dataNascimento: data.dataNascimento ? data.dataNascimento.substring(0, 10) : '',
    superpoderesIds: Array.isArray(data.superpoderes)
      ? data.superpoderes.map((s: Superpoder) => s.id)
      : [],
  };
}

async function editarHeroi() {
  loading.value = true;
  try {
    const idParam = Array.isArray(route.params.id) ? route.params.id[0] : route.params.id;
    await api.put(`/api/Herois/${idParam}`, {
      ...form.value,
      dataNascimento: form.value?.dataNascimento,
      altura: Number(String(form.value?.altura).replace(',', '.')),
      peso: Number(String(form.value?.peso).replace(',', '.')),
    });
    void router.push('/');
  } catch (error: unknown) {
    let message = 'Erro ao editar herói.';
    if (typeof error === 'object' && error !== null && 'response' in error) {
      const err = error as { response?: { data?: unknown } };
      if (err.response && err.response.data) {
        if (typeof err.response.data === 'string') {
          message = err.response.data;
        } else if (
          typeof err.response.data === 'object' &&
          err.response.data !== null &&
          'title' in err.response.data
        ) {
          message = (err.response.data as { title: string }).title;
        }
      }
    }
    $q.notify({ type: 'negative', message });
  } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  await carregarSuperpoderes();
  await carregarHeroi();
});
</script>
