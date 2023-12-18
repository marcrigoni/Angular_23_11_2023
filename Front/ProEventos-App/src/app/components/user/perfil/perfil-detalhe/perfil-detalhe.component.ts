import {ValidatorField} from '../../../../helpers/ValidatorField';
import { UserUpdate } from '../../../../models/Identity/UserUpdate';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AccountService } from '../../../../services/AccountService.service';
import {Validators, AbstractControlOptions,  FormGroup,   FormBuilder} from '@angular/forms';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { PalestranteService } from '@app/services/palestrante.service';


@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.css']
})
export class PerfilDetalheComponent implements OnInit {
  @Output() changeFormValue = new EventEmitter();

  userUpdate = {} as UserUpdate;
  form: FormGroup = new FormGroup({});

  constructor(private formBuilder: FormBuilder,
    public accountService: AccountService,
    public palestranteService: PalestranteService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificarNome();
  }
  verificarNome() {
    this.form.valueChanges.subscribe(() => this.changeFormValue.emit({
      ...this.form.value
    }))
  }

  validation() {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.formBuilder.group(
      {
        imagemUrl: [''],
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

  private carregarUsuario(): void {
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
      () => { this.spinner.hide() }
    )
  }

  get f(): any {
    return this.form.controls;
  }

  public onSubmit(): void {

    this.atualizarUsuario();

  }

  atualizarUsuario() {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();

    console.log('Função: ' + this.f.funcao.value);

    if (this.f.funcao.value === 'Palestrante') {

      console.log('Entrou if: ' + this.f.funcao.value);

      const observer = {
        next: () => {
          this.toaster.success('Função Palestrante Ativada', 'Sucesso');
        },
        error: (erro: any) => {
          this.toaster.error('A função palestrante não pôde ser ativada', 'Erro!');
        },
        complete: () => this.spinner.hide()
      };

      this.palestranteService.post().subscribe(observer);

      const observerUpdate = {

        next: () => {
          this.toaster.success('Usuário atualizado!', 'Sucesso!');
        },
        error: (erro: any) => {
          this.toaster.error(erro);
        },
        complete: () => {
          this.spinner.hide();
        }
      }

      this.accountService.updateUser(this.userUpdate).subscribe(observer);
    }

    console.log(this.userUpdate);

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => this.toaster.success('Usuário atualizado!', 'Sucesso!'),
      (error) => {
        this.toaster.error(error);
        console.error(error);
      }
    ).add(() => this.spinner.hide())
  }

  resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }
}
