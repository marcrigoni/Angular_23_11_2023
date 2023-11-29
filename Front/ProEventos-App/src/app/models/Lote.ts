import { Evento } from "./Evento";

export interface Lote {

  id: number;

  nome: string;

  preco: number;

  dataInicio?: Date;

  dataFim?: Date;

  qtde: number;

  eventoId: number;

  evento: Evento;

}
