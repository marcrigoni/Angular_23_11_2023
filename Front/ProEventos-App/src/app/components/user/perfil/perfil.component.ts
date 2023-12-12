import {Router} from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { AccountService } from '@app/services/AccountService.service';
import { ToastrService } from 'ngx-toastr';
import {NgxSpinnerService, NgxSpinner} from 'ngx-spinner';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  form: FormGroup = new FormGroup({});

  public onSubmit(): void {

    this.atualizarUsuario();

  }
  atualizarUsuario() {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();

    console.log(this.userUpdate);

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => this.toaster.success('Usuário atualizado!', 'Sucesso!'),
      (error) => {
        this.toaster.error(error);
        console.error(error);
      }
    ).add(() => this.spinner.hide())
  }

  constructor(
    private formBuilder: FormBuilder,
    public accountService: AccountService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
  }

  validation() {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.formBuilder.group(
      {
        userName: [''],
        titulo: ['NaoInformado', Validators.required],
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', [Validators.required]],
        descricao: ['', [Validators.required]],
        funcao: ['NaoInformado', [Validators.required]],
        password: ['', [Validators.minLength(6), Validators.nullValidator]],
        confirmePassword: ['', Validators.nullValidator]
      }, formOptions
    );
  }

  get f(): any {
    return this.form.controls;
  }

  resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  private carregarUsuario(): void  {
    this.spinner.show();
    this.accountService.GetUser().subscribe(
      (userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário carregado!', 'Sucesso!');
      },
      (error) => {
        console.error(error);
        this.toaster.error('Usuário não carregado!', 'Erro!');
        this.router.navigate(['/dashboard']);
      },
      () => { this.spinner.hide()}
    )
  }
}
