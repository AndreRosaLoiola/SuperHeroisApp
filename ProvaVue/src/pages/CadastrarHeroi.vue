<template>
  <q-page class="q-pa-md">
    <q-card>
      <q-card-section class="row items-center justify-between">
        <div class="text-h6">Cadastrar Herói</div>
        <q-btn flat color="primary" icon="arrow_back" label="Voltar" @click="router.push('/')" />
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-form @submit.prevent="cadastrarHeroi">
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
          <q-btn type="submit" color="primary" label="Cadastrar" :loading="loading" />
        </q-form>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { api } from '../boot/axios';
import type { Superpoder, FormHeroi } from '../interfaces/heroi';
import { useQuasar } from 'quasar';

const $q = useQuasar();
const router = useRouter();
const loading = ref(false);
const superpoderes = ref<Superpoder[]>([]);
const form = ref<FormHeroi>({
  nome: '',
  nomeHeroi: '',
  dataNascimento: '',
  altura: null,
  peso: null,
  superpoderesIds: [],
});

async function carregarSuperpoderes() {
  const { data } = await api.get<Superpoder[]>('/api/Herois/superpoderes');
  superpoderes.value = data;
}

async function cadastrarHeroi() {
  if (!form.value.nome || form.value.nome.trim() === '') {
    $q.notify({ type: 'negative', message: 'Preencha o nome.' });
    return;
  }
  if (!form.value.nomeHeroi || form.value.nomeHeroi.trim() === '') {
    $q.notify({ type: 'negative', message: 'Preencha o nome do herói.' });
    return;
  }
  if (!form.value.dataNascimento || form.value.dataNascimento.trim() === '') {
    $q.notify({ type: 'negative', message: 'Preencha a data de nascimento.' });
    return;
  }
  if (
    form.value.altura === null ||
    form.value.altura === '' ||
    isNaN(Number(String(form.value.altura).replace(',', '.')))
  ) {
    $q.notify({ type: 'negative', message: 'Preencha a altura corretamente.' });
    return;
  }
  if (
    form.value.peso === null ||
    form.value.peso === '' ||
    isNaN(Number(String(form.value.peso).replace(',', '.')))
  ) {
    $q.notify({ type: 'negative', message: 'Preencha o peso corretamente.' });
    return;
  }
  if (!form.value.superpoderesIds || form.value.superpoderesIds.length === 0) {
    $q.notify({ type: 'negative', message: 'Selecione pelo menos um superpoder.' });
    return;
  }
  loading.value = true;
  try {
    await api.post('/api/Herois', {
      ...form.value,
      dataNascimento: form.value.dataNascimento,
      altura: Number(String(form.value.altura).replace(',', '.')),
      peso: Number(String(form.value.peso).replace(',', '.')),
    });
    void router.push('/');
  } catch (error: unknown) {
    let message = 'Erro ao cadastrar herói.';
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

onMounted(carregarSuperpoderes);
</script>
