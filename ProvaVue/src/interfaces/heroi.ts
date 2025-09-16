export interface Superpoder {
  id: number;
  superpoder: string;
  descricao: string;
}

export interface Heroi {
  id: number;
  nome: string;
  nomeHeroi: string;
  dataNascimento: string;
  altura: number;
  peso: number;
  superpoderes: Superpoder[];
}

export interface FormHeroi {
  nome: string;
  nomeHeroi: string;
  dataNascimento: string;
  altura: number | null | string;
  peso: number | null | string;
  superpoderesIds: number[];
}
