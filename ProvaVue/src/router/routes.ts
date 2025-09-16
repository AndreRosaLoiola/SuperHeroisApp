import type { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/home',
  },
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: 'home', component: () => import('pages/IndexPage.vue') },
      { path: 'cadastrar', component: () => import('pages/CadastrarHeroi.vue') },
      { path: 'editar/:id', component: () => import('pages/EditarHeroi.vue'), props: true },
    ],
  },

  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
