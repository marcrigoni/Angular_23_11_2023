import { Evento } from '../../../models/Evento';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
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

  get bsConfig(): any {
    return {
      adaptivepPosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm:a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  CSSValidator(campoForm: FormControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched }
  }

  form!: FormGroup;

  /**
  *: any f
  return this.form.controls;   */
  public get f(): any {
    return this.form.controls;
  }


  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  public carregarEvento(): void {

    this.spinner.show();

    const eventoIdParam = this.router.snapshot.paramMap.get('id');

    if (eventoIdParam !== null) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.eventoService.getEventosById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
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

        imageURL: ['', Validators.required],

        telefone: ['', Validators.required],

        email: ['', [Validators.required, Validators.email]]
      }
    );
  }  /**
   * resetForm
   */
  public resetForm(): void {
    this.form.reset();
  }

  public salvarAlteracao(): void {
    this.spinner.show();
    if (this.form.valid) {

      this.evento = (this.estadoSalvar === 'post') ? { ...this.form.value } : { id: this.evento.id, ...this.form.value };
      console.log('salvar: ' + this.estadoSalvar);

      if (this.estadoSalvar === 'post' || this.estadoSalvar ==='put') {
        this.eventoService[this.estadoSalvar](this.evento).subscribe(
          () => this.toastr.success('Evento salvo com sucesso!', 'Sucesso'),
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
}

