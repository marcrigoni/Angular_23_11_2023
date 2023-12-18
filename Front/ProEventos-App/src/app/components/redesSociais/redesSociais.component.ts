import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { RedeSocial } from '@app/models/RedeSocial';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RedesocialService } from './../../services/redesocial.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-redesSociais',
  templateUrl: './redesSociais.component.html',
  styleUrls: ['./redesSociais.component.css'],
})
export class RedesSociaisComponent implements OnInit {
declineDeleteLote() {
throw new Error('Method not implemented.');
}
  modalRef!: BsModalRef;
  @Input() eventoId = 0;
  formRS!: FormGroup;
  public redeSocialAtual = { Id: 0, nome: '', indice: 0 };

  public get redesSociais(): FormArray {
    return this.formRS.get('redesSociais') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private modalService: BsModalService,
    private redesocialService: RedesocialService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.carregarRedeSociais(this.eventoId);
    this.validation();
  }

  carregarRedeSociais(id: number = 0): void {

    let origem = 'palestrante';

    if (this.eventoId !== 0) {
      origem = 'evento';
    }

    this.spinner.show();

    const observer = {
      next: (redeSocialRetorno: RedeSocial[]) => {
        redeSocialRetorno.forEach((redeSocial) => {
          console.log('Rede Social url: ' + redeSocial.url);
          this.redesSociais.push(this.criarRedeSocial(redeSocial));
        });
      },
      error: (erro: any) => {
        this.toastr.error('Erro ao tentar carregar Rede Social', 'Erro');
        console.error(erro);
      },
    };

    this.redesocialService
      .getRedesSociais(origem, id)
      .subscribe(observer)
      .add(() => this.spinner.hide());
  }

  validation() {
    this.formRS = this.fb.group({
      redesSociais: this.fb.array([]),
    });
  }

  public cssValidator(campoForm?: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm.touched };
  }

  adicionarRedesSociais(): void {
    this.redesSociais.push(this.criarRedeSocial({ id: 0 } as RedeSocial));
  }

  criarRedeSocial(redeSocial: RedeSocial): any {
    return this.fb.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required],
    });
  }

  public retornaTitulo(nome: string): string {
    return nome === null || nome === '' ? 'Rede Social' : nome;
  }

  removerRedeSocial(template: TemplateRef<any>, indice: number): void {
    this.redeSocialAtual.Id = this.redesSociais.get(indice + '.id')?.value;
    this.redeSocialAtual.nome = this.redesSociais.get(indice + '.nome')?.value;
    this.redeSocialAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public salvarRedesSociais(): void {

    let origem = 'palestrante';

    if (this.eventoId !== 0) {
      origem = 'evento';
    }

    if (this.formRS.controls['redesSociais'].status) {
      this.spinner.show();
      this.redesocialService
        .saveRedeSocial(origem, this.eventoId, this.formRS.value['redesSociais'])
        .subscribe(
          () => {
            this.toastr.success(
              'Redes Sociais salvos com sucesso!',
              'Sucesso!'
            );
            this.redesSociais.removeAt(this.redeSocialAtual.indice);
          },
          (error: any) => {
            this.toastr.error('Erro ao salvar Redes Sociais!', 'Erro!');
            console.log(error);
          }
        )
        .add(() => {
          this.spinner.hide();
        });
    }
  }

  confirmDeleteLote(): void {

    let origem = 'palestrante';

    if (this.eventoId !== 0) {
      origem = 'evento';
    }

    this.modalRef.hide();
    this.spinner.show();

    this.redesocialService
      .deleteRedeSocial(origem, this.eventoId, this.redeSocialAtual.Id)
      .subscribe(
        () => {
          this.toastr.success('Rede Social deletada com sucesso!', 'Sucesso!');
          this.redesSociais.removeAt(this.redeSocialAtual.indice);
        },
        (error: any) => {
          this.toastr.error('Erro ao deletar Rede Social!', 'Erro!');
          console.log(error);
        }
      )
      .add(() => {
        this.spinner.hide();
      });
  }
}
