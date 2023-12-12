import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '@app/services/AccountService.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/user/login');
  }

  isCollapsed = false;

  constructor(
    private router: Router,
    public accountService: AccountService
  ) { }

  ngOnInit() {
  }

  showMenu(): boolean {
    return this.router.url !== '/user/login';
  }
}
