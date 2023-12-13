import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, map, take } from 'rxjs';
import { Evento } from '../models/Evento';
import { environment } from '@environments/environment.prod';
import { PaginatedResult } from '@app/models/Pagination';

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

  public getEvento(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Evento[]>> {

    const paginatedResult: PaginatedResult<Evento[]> = new PaginatedResult<Evento[]>();
    let params = new HttpParams;

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (term !== null && term !== '' && term !== undefined) {
      params = params.append('term', term!);
    }    

    console.log('Params: ' + params); 

    return this.http.get<Evento[]>(this.baseUrl, { observe: 'response', params }).
      pipe(take(1),
        map(
          (response) => {
            if (response.body) {
              paginatedResult.result = response.body;
            }
            if (response.headers.has('Pagination')) {
              if (response.headers.get('Pagination') !== null) {
                paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')!);
              }
            }
            return paginatedResult;
          }
        )
      );
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
    return this.http.delete(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public postUpload(eventoId: number, file: File[]): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http.post<Evento>(`${this.baseUrl}/upload-image/${eventoId}`, formData).pipe(take(1));
  }
}
