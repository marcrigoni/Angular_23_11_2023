import {HttpClient} from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {
  public eventos: any;

  constructor(
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.getEventos();
  }

  public getEventos(): void {

    this.http.get('https://localhost:5001/api/evento').subscribe(
      response => this.eventos = response,
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
