<template>
  <q-card>
    <q-card-section>
      <div class="text-h6">Lista de Heróis</div>
    </q-card-section>
    <q-separator />
    <q-card-section>
      <q-table
        :rows="herois"
        :columns="columns"
        row-key="id"
        flat
        :loading="loading"
        :pagination="{ rowsPerPage: 10 }"
      >
        <template #body-cell-acoes="props">
          <q-td :props="props">
            <q-btn color="primary" icon="edit" dense flat @click="editarHeroi(props.row.id)" />
            <q-btn
              color="negative"
              icon="delete"
              dense
              flat
              @click="confirmarExclusao(props.row)"
            />
          </q-td>
        </template>
        <template #body-cell-expand="props">
          <q-td :props="props" auto-width>
            <q-btn
              size="sm"
              color="primary"
              round
              dense
              flat
              :icon="expandedRows[props.row.id] ? 'person_off' : 'person'"
              @click.stop="toggleRow(props.row.id)"
            />
          </q-td>
        </template>
        <template #body="props">
          <q-tr :props="props">
            <q-td v-for="col in props.cols" :key="col.name" :props="props">
              <template v-if="col.name === 'expand'">
                <q-btn
                  size="sm"
                  color="primary"
                  round
                  dense
                  flat
                  :icon="expandedRows[props.row.id] ? 'person_off' : 'person'"
                  @click.stop="toggleRow(props.row.id)"
                />
              </template>
              <template v-else-if="col.name === 'acoes'">
                <q-btn color="primary" icon="edit" dense flat @click="editarHeroi(props.row.id)" />
                <q-btn
                  color="negative"
                  icon="delete"
                  dense
                  flat
                  @click="confirmarExclusao(props.row)"
                />
              </template>
              <template v-else>
                {{ props.row[col.field] }}
              </template>
            </q-td>
          </q-tr>
          <q-tr v-if="expandedRows[props.row.id]">
            <q-td colspan="100%">
              <div v-if="props.row.superpoderes && props.row.superpoderes.length">
                <div class="q-mb-sm text-bold">Superpoderes:</div>
                <q-list dense bordered>
                  <q-item v-for="sp in props.row.superpoderes" :key="sp.id">
                    <q-item-section>
                      <div class="text-subtitle2">{{ sp.superpoder }}</div>
                      <div class="text-caption">{{ sp.descricao }}</div>
                    </q-item-section>
                  </q-item>
                </q-list>
              </div>
              <div v-else>Nenhum superpoder cadastrado.</div>
            </q-td>
          </q-tr>
        </template>
      </q-table>
    </q-card-section>
    <q-dialog v-model="dialogExcluir">
      <q-card>
        <q-card-section class="row items-center">
          <q-icon name="warning" color="negative" size="2em" class="q-mr-md" />
          <div>
            Deseja realmente excluir o herói <b>{{ heroiParaExcluir?.nomeHeroi }}</b
            >?
          </div>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat label="Cancelar" color="primary" v-close-popup />
          <q-btn flat label="Excluir" color="negative" @click="excluirHeroiConfirmado" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-card>
</template>

<script setup lang="ts">
import { ref, onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { api } from '../boot/axios';
import type { Heroi } from '../interfaces/heroi';

const router = useRouter();
const herois = ref<Heroi[]>([]);

const loading = ref(false);
const dialogExcluir = ref(false);
const heroiParaExcluir = ref<Heroi | null>(null);
const expandedRows = reactive<Record<number, boolean>>({});

function toggleRow(id: number) {
  expandedRows[id] = !expandedRows[id];
}

const columns = [
  { name: 'nome', label: 'Nome', field: 'nome', align: 'left' as const },
  { name: 'nomeHeroi', label: 'Nome Herói', field: 'nomeHeroi', align: 'left' as const },
  {
    name: 'dataNascimentoFormatada',
    label: 'Nascimento',
    field: 'dataNascimentoFormatada',
    align: 'left' as const,
  },
  { name: 'altura', label: 'Altura', field: 'altura', align: 'left' as const },
  { name: 'peso', label: 'Peso', field: 'peso', align: 'left' as const },
  { name: 'expand', label: 'Poderes', field: '', align: 'left' as const },
  { name: 'acoes', label: 'Ações', field: 'acoes', align: 'right' as const },
];

async function carregarHerois() {
  loading.value = true;
  try {
    const { data } = await api.get<Heroi[]>('/api/Herois');
    herois.value = data;
  } catch {
    herois.value = [];
  } finally {
    loading.value = false;
  }
}

function editarHeroi(id: number) {
  void router.push(`/editar/${id}`);
}

function confirmarExclusao(heroi: Heroi) {
  heroiParaExcluir.value = heroi;
  dialogExcluir.value = true;
}

async function excluirHeroiConfirmado() {
  if (heroiParaExcluir.value) {
    await api.delete(`/api/Herois/${heroiParaExcluir.value.id}`);
    dialogExcluir.value = false;
    heroiParaExcluir.value = null;
    await carregarHerois();
  }
}

onMounted(carregarHerois);
</script>
