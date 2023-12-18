import {Palestrante} from '../models/Palestrante';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, map, take } from 'rxjs';
import { Evento } from '../models/Evento';
import { environment } from '@environments/environment.prod';
import { PaginatedResult } from '@app/models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {

  baseUrl = environment.apiUrl + 'api/Palestrante';
  tokenHeader = new HttpHeaders({ Authorization: `Bearer ${JSON.parse(localStorage.getItem('user')!).token}`, });

  constructor(
    private http: HttpClient
  ) { }

  ngOnInit(): void {

  }

  public getPalestrantes(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Palestrante[]>> {

    const paginatedResult: PaginatedResult<Palestrante[]> = new PaginatedResult<Palestrante[]>();
    let params = new HttpParams;

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (term !== null && term !== '' && term !== undefined) {
      params = params.append('term', term!);
    }

    console.log('Params: ' + params);

    return this.http.get<Palestrante[]>(this.baseUrl + '/all'  , { observe: 'response', params }).
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

  public getPalestrante(): Observable<Palestrante> {
    return this.http.get<Palestrante>(`${this.baseUrl}`).pipe(take(1));
  }

  public post(): Observable<Palestrante> {
    return this.http.post<Palestrante>(this.baseUrl, { } as Palestrante).pipe(take(1));
  }

  public put(Palestrante: Palestrante): Observable<Palestrante> {
    return this.http.put<Palestrante>(`${this.baseUrl}`, Palestrante).pipe(take(1));
  }
}
