import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-eventos-detalhe',
  templateUrl: './eventos-detalhe.component.html',
  styleUrls: ['./eventos-detalhe.component.css']
})
export class EventosDetalheComponent implements OnInit {

  form!: FormGroup;

  /**
  *: any f
  return this.form.controls;   */
  public get f(): any {
    return this.form.controls;
  }


  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
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
  }

  /**
   * resetForm
   */
  public resetForm(): void {
      this.form.reset();
    }
  }
