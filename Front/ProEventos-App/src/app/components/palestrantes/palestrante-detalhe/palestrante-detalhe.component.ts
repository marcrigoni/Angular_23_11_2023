import {Palestrante} from '../../../models/Palestrante';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.css']
})
export class PalestranteDetalheComponent implements OnInit {

  public form!: FormGroup;
  public situacaoDoForm: string = '';
  public corDaDescricao: string = '';

  constructor(
    private fb: FormBuilder,
    public palestranteService: PalestranteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {

    this.validation();
    this.verificaForm();
    this.carregarPalestrante();
  }

  carregarPalestrante(): void {
    this.spinner.show();

    this.palestranteService.getPalestrante().subscribe(
      (palestrante: Palestrante) => {
        this.form.patchValue(palestrante);
      },
      (error: any) => {
        this.toastr.error('Erro ao Carregar o Palestrante', 'Erro');
      }
    )
  }

  verificaForm() {
    this.form.valueChanges.pipe(
       map(() => {
        this.situacaoDoForm = 'Minicurrículo está sendo atualizado';
        this.corDaDescricao = 'text-warning';
      }),
      debounceTime(200),
      tap(() => this.spinner.show())
    )
      .subscribe(() => {
        this.palestranteService.put({ ...this.form.value }).subscribe(
          () => {
            this.situacaoDoForm = 'Minicurrículo atualizado!';
            this.corDaDescricao = 'text-success';

            setTimeout(() => {
              this.situacaoDoForm = 'Minicurrículo foi carregado!';
              this.corDaDescricao = 'text-success';
            }, 2000);
        },
          (error: any) => {
            this.toastr.error('erro ao atualizar minicurrículo', 'Erro!');
          }
        ).add(()=> this.spinner.hide());
    })
  }

  validation() {
    this.form = this.fb.group({
      miniCurriculo: ['']
    })
  }

  public get f() : any {
    return this.form.controls;
  }
}
