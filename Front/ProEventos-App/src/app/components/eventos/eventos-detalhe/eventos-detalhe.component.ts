import { Evento } from '../../../models/Evento';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import {AbstractControl, FormArray, FormBuilder,  FormControl,  FormGroup,  Validators} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Lote } from '@app/models/Lote';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@environments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import {BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { EventoService } from 'src/app/services/evento.service';


@Component({
  selector: 'app-eventos-detalhe',
  templateUrl: './eventos-detalhe.component.html',
  styleUrls: ['./eventos-detalhe.component.css']
})
export class EventosDetalheComponent implements OnInit {

  evento = {} as Evento;
  estadoSalvar = 'post';
  staten: string = this.estadoSalvar = 'post' ? 'post' : 'put';
  eventoId: any;
  form!: FormGroup;
  modalRef!: BsModalRef;
  loteAtual = { id: 0, nome: '', indice: 0 };
  imagemURL = 'assets/img/upload.png';
  file!: File[];

  onFileChange(eve: any): void {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    this.file = eve.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  public uploadImage(): void {
    this.spinner.show();
    console.log("Entrou no upload");
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toastr.success('A imagem foi carregada com sucesso!', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar imagem!', 'Erro!');
        console.log(error);
      }
    ).add(() => { this.spinner.hide() });
  }

  retornaTituloLote(nome: string): string {
    return nome === null || nome === '' ?
      'Nome do Lote' : nome;
  }

  bsConfigLote(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumber: false
    };
  }

  mudarValorData(value: Date, indice: number, campo: string) {
    this.lotes.value[indice][campo] = value;
  }

  declineDeleteLote() {
    this.modalService.hide();
  }


  confirmDeleteLote() {
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso!', 'Sucesso!');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any) => {
        this.toastr.error('Erro ao deletar lote!', 'Erro!');
        console.log(error);
      }
    ).add(() => { this.spinner.hide() });
  }

  removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome')?.value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });

  }

  public get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  public get f(): any {
    return this.form.controls;
  }

  public get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get bsConfig(): any {
    return {
      adaptivepPosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm:a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  CSSValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched }
  }


  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {
    this.localeService.use('pt-br');
  }

  public carregarEvento(): void {

    this.spinner.show();

    this.eventoId = this.activatedRouter.snapshot.paramMap.get('id');

    if (this.eventoId !== null && this.eventoId !== 0) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.eventoService.getEventosById(+this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          if (this.evento.imageURL !== '') {
            this.imagemURL = environment.apiUrl + 'resources/images/' + this.evento.imageURL;
          }
          this.carregarLotes();
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar Evento.', 'Erro!');
          console.error(error);
        },
        () => {
          this.spinner.hide();
        }
      )
    }
    this.spinner.hide();
  }
  carregarLotes() {
    this.spinner.show();
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(
          lote => {
            this.lotes.push(
              this.criarLote(lote)
            );
          }
        );
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar Lotes!', 'Erro!');
        console.log(error);
      }
    ).add(() => { this.spinner.hide() });
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void {

    this.form = this.fb.group(
      {
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],

        local: ['', Validators.required],

        dataEvento: ['', Validators.required],

        qtdePessoas: ['', [Validators.required, Validators.max(120000)]],

        imageURL: [''],

        telefone: ['', Validators.required],

        email: ['', [Validators.required, Validators.email]],

        lotes: this.fb.array([])
      }
    );
  }

  adicionarLote(): void {
    (this.form.get('lotes') as FormArray).push(
      this.criarLote({ id: 0 } as Lote)
    );
  }

  public criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      qtde: [lote.qtde, Validators.required],
      dataInicio: [lote.dataInicio == null ? null : new Date(lote.dataInicio)],
      dataFim: [lote.dataFim == null ? null : new Date(lote.dataFim)]
      }
    )
  }

  public resetForm(): void {
    this.form.reset();
  }

  public salvarAlteracao(): void {
    this.spinner.show();
    if (this.form.valid) {

      this.evento = (this.estadoSalvar === 'post') ? { ...this.form.value } : { id: this.evento.id, ...this.form.value };
      console.log('salvar: ' + this.estadoSalvar);

      if (this.estadoSalvar === 'post' || this.estadoSalvar === 'put') {
        this.eventoService[this.estadoSalvar](this.evento).subscribe(
          (eventoRetorno: Evento) => {
            this.toastr.success('Evento salvo com sucesso!', 'Sucesso');
            this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
          },
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.success('Erro ao salvar evento!', 'Erro!');
          },
          () => this.spinner.hide()
        )
      }
    }
  }

  /**
  * SalvarLote
  */
  public salvarLotes(): void {
    if (this.form.valid) {
      this.spinner.show();
      console.log(this.form.value['lotes']);
      if (this.form.controls['lotes'].valid) {
        this.loteService.saveLote(this.eventoId, this.form.value['lotes']).subscribe(
          () => {
            this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
          },
          (error: any) => {
            this.toastr.error('Erro ao salvar Lotes!', 'Erro!');
            console.log(error);
          }
        ).add(() => { this.spinner.hide() });
      }
    }
  }
}

