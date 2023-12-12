import { Component, OnInit } from '@angular/core';
import { UserLogin } from '@app/models/Identity/UserLogin';
import { AccountService } from './../../../services/AccountService.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public model = {} as UserLogin;

  constructor(private accountService: AccountService,
              private router: Router,
              private toaster: ToastrService
              ) { }

  ngOnInit() : void {}

  public login() {


    this.accountService.login(this.model).subscribe(
      () => {
        this.router.navigateByUrl('/dashboard');
      },
      (error: any) => {
        if (error.status === 401) {
          this.toaster.error('Usuário ou senha inválidos!', 'Erro!');
        } else {
          console.error(error);
        }
      }
      )
    }
  }
  // const observer = {
  //   next: () => {
  //     this.router.navigateByUrl('/dashboard');
  //   },
  //   error: (error: any) => {
  //     if (error.status === 401) {
  //       this.toaster.error('Usuário ou senha inválido(s)!', 'Erro!')
  //     } else {
  //       console.error(error);
  //     }
  //   }
  // }

  // console.log(this.model);

  // this.accountService.login(this.model).subscribe(observer);
