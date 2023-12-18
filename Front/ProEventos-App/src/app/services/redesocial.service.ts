import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, take } from 'rxjs';
import { RedeSocial } from './../models/RedeSocial';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class RedesocialService {

  baseURL = environment.apiUrl + 'api/redesSociais';

  constructor(private http: HttpClient) {}

  public getRedesSociais(origem: string, id: number): Observable<RedeSocial[]> {
    let url = id === 0 ? `${this.baseURL}/${origem}` : `${this.baseURL}/${origem}/${id}`;

    return this.http.get<RedeSocial[]>(url).pipe(take(1));
  }

  saveRedeSocial(origem: string, id: number, redesSociais: RedeSocial[]): Observable<RedeSocial[]> {
    let URL =
      id === 0 ?
        `${this.baseURL}/${origem}`
        : `${this.baseURL}/${origem}/${id}`;

    return this.http.put<RedeSocial[]>(URL, redesSociais).pipe(take(1));
  }

  deleteRedeSocial(origem: string, id: number, redeSocialId: number): Observable<any> {
    let URL =
      id === 0 ?
        `${this.baseURL}/${origem}/${redeSocialId}`
        : `${this.baseURL}/${origem}/${id}/${redeSocialId}`;

    return this.http.delete(URL).pipe(take(1));
  }
}
