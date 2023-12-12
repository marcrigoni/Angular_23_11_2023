import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Evento } from '../models/Evento';
import { environment } from '@environments/environment.prod';

@Injectable()
export class EventoService implements OnInit {

  baseUrl = environment.apiUrl + 'api/Eventos';
  tokenHeader = new HttpHeaders({ Authorization: `Bearer ${JSON.parse(localStorage.getItem('user')!).token}`, });

  constructor(
  private http: HttpClient
  ) { }

  ngOnInit(): void {
    // this.getEvento();
  }

  public getEvento() : Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseUrl).pipe(take(1));
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseUrl}/${tema}/tema`).pipe(take(1));
  }

  public getEventosById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http.post<Evento>(this.baseUrl, evento).pipe(take(1));
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http.put<Evento>(`${this.baseUrl}/${evento.id}`, evento).pipe(take(1));
  }

  public deleteEvento(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}` ).pipe(take(1));
  }

  public postUpload(eventoId: number, file: File[]): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http.post<Evento>(`${this.baseUrl}/upload-image/${eventoId}`, formData).pipe(take(1));
  }
}
