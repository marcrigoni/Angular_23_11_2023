import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { AccountService } from '@app/services/AccountService.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService, NgxSpinner } from 'ngx-spinner';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { environment } from '@environments/environment.prod';
@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss'],
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  public file!: File[];
  public imageURL!: string;


  setFormValue(usuario: UserUpdate) {
    this.userUpdate = usuario;
    console.log(usuario);
    if (this.userUpdate.imagemUrl) {
      this.imageURL = environment.apiUrl + `resources/images/${this.userUpdate.imagemUrl}`;
    } else {
      this.imageURL ='./assets/img/perfil.png';
    }
  }

  onFileChange(eve: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => (this.imageURL = event.target.result);

    this.file = eve.target.files;

    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  public get ehPalestrante(): boolean {
    // console.log(this.userUpdate.funcao);
    return this.userUpdate.funcao === 'Palestrante';
  }

  constructor(
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {}

  public get f(): any {
    return '';
  }

  private uploadImage(): void {
    this.spinner.show();
    this.accountService.postUpload(this.file).subscribe(
      () => this.toastr.success('Imagem atualizada com sucesso!', 'Sucesso!'),
      (error: any) => {
        this.toastr.error('Erro ao carregar imagem', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }
}
