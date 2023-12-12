import { Component, OnInit } from '@angular/core';
import { AccountService } from './services/AccountService.service';
import { User } from './models/Identity/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit  {
  title = 'ProEventos-App';

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    let user: User | null;

    if (localStorage.getItem('user')) {
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    } else {
      user = null;
    }

    if (user) {
      this.accountService.SetCurrentUser(user);
    }
  }
}
