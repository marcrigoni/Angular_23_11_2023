import {HttpClient} from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {
  public eventos: any = [];
  public eventosFiltrados: any = [];
  public widthImg: number = 50;
  public marginImg: number = 2;
  public mostrarImagem: boolean = true;
  private _filtroLista: string = '';


  public get filtroLista() : string {
    return this._filtroLista;
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string): any {

    filtrarPor = filtrarPor.toLowerCase();
    return this.eventos.filter(
      (evento: { tema: String; local: string; }) => evento.tema.toLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLowerCase().indexOf(filtrarPor) !== -1
    )

  }

  constructor(
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.getEventos();
  }

  /**
   * name
   */
  public alterarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  public getEventos(): void {

    this.http.get('https://localhost:5001/api/eventos').subscribe(
      response => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos;
        console.log(response);
      },
      error => console.error(error)
    );

    // this.eventos = [
    //   {
    //     Tema: 'Angular',
    //     Local: 'Belo Horizonte',
    //   },
    //   {
    //     Tema: '.Net 5',
    //     Local: 'São Paulo',
    //   },
    //   {
    //     Tema: 'Nelson Piquet',
    //     Local: 'Brasília',
    //   },
    // ];
  }
}
