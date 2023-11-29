import {HttpClient} from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../services/evento.service';
import { Evento } from '../models/Evento';
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public widthImg: number = 50;
  public marginImg: number = 2;
  public mostrarImagem: boolean = true;
  private _filtroLista: string = '';
  modalRef?: BsModalRef;
  message?: string;

  public get filtroLista() : string {
    return this._filtroLista;
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string): any {

    filtrarPor = filtrarPor.toLowerCase();
    return this.eventos.filter(
      (evento: { tema: String; local: string; }) => evento.tema.toLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLowerCase().indexOf(filtrarPor) !== -1
    )

  }

  constructor(
    private http: HttpClient,
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastrService: ToastrService,
    private spinnerService: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.getEventos();

    this.spinnerService.show();

    setTimeout(() => {
      this.spinnerService.hide();
    }, 100);
  }

  /**
   * name
   */
  public alterarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  public getEventos(): void {
    const observer = {
      next: (_eventos: Evento[]) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
        console.log(_eventos);
      },
      error: (erro: any) => {
        this.spinnerService.hide();
        this.toastrService.error('Erro ao carregar Eventos!', 'Verifique');
      }
    }
    this.eventoService.getEvento().subscribe(observer);
  }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, {
      class: 'modal-sm'
    });
  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastrService.success('Evento deletado com sucesso.', 'Deletado!');
  }

  decline(): void {
    this.modalRef?.hide();
  }
}
