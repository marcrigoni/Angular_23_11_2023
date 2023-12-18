import {UserUpdate} from './Identity/UserUpdate';
import { Evento } from './Evento';
import { RedeSocial } from './RedeSocial';

export interface Palestrante {
  id: number;
  miniCurriculo: string;
  user: UserUpdate;
  redesSociais: RedeSocial[];
  eventos: Evento[];
}
