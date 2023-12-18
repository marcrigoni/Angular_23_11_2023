import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/Identity/User';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { environment } from '@environments/environment.prod';
import { Observable, ReplaySubject, map, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private currentSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentSource.asObservable();

  baseUrl = environment.apiUrl + 'api/account/';
  constructor(private http: HttpClient) {}

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.SetCurrentUser(user);
        }
      })
    );
  }

  public logout(): void {
    localStorage.removeItem('user');
    this.currentSource.next(new User());
    this.currentSource.complete();
  }

  public SetCurrentUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentSource.next(user);
  }

  GetUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(this.baseUrl + 'getUser').pipe(take(1));
  }

  updateUser(model: UserUpdate): Observable<void> {
    return this.http.put<UserUpdate>(this.baseUrl + 'updateUser', model).pipe(
      take(1),
      map((user: UserUpdate) => {
        this.SetCurrentUser(user);
      })
    );
  }

  public register(model: any): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'register', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.SetCurrentUser(user);
        }
      })
    );
  }

  public postUpload(file: File[]): Observable<UserUpdate> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http
      .post<UserUpdate>(`${this.baseUrl}upload-image`, formData)
      .pipe(take(1));
  }
}
