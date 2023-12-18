import {Palestrante} from '../../../models/Palestrante';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environments/environment';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject, debounceTime } from 'rxjs';
import { PalestranteService } from '@app/services/palestrante.service';


@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.css']
})
export class PalestranteListaComponent implements OnInit {

  public palestrantes: Palestrante[] = [];
  eventoId: number = 0;
  pagination = {} as Pagination;
  imageUrl!: string;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  filtrarPalestrantes(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged.pipe(debounceTime(200)).subscribe(
        (filtrarPor) => {
          this.spinnerService.show();
          const observer = {
            next: (paginationResult: PaginatedResult<Palestrante[]>) => {
              this.palestrantes = paginationResult.result;
              this.pagination = paginationResult.pagination;
            },
            error: (error: any) => {
              this.spinnerService.hide();
              this.toastrService.error('Erro ao carregar os Palestrantes', 'Erro!');
            },
            complete: () => this.spinnerService.hide()
          }

          this.palestranteService.getPalestrantes(
            this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtrarPor
          ).subscribe(observer);
        }
      )
    }
    this.termoBuscaChanged.next(evt.value);
  }

  /**
   * getImage
imageName: string  : string  */
  public getImage(imageName: string): string {
    if (imageName) {
      return environment.apiUrl + `resources/images/${imageName}`;
    } else {
      return './assets/img/perfil.png';
    }
  }

  public pageChanged($event: PageChangedEvent): void {
    if (this.pagination != null) {
      console.log($event.page);
      this.pagination.currentPage = $event.page;
      // this.getEventos();
    }
  }

  mostraImagem(imageUrl: string) {
    return (imageUrl !== '')
      ? `${environment.apiUrl}resources/images/${imageUrl}`
      : 'assets/img/semimagem.jpeg';
  }


  constructor(
    private http: HttpClient,
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastrService: ToastrService,
    private spinnerService: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1
    } as Pagination;
    this.carrgarPalestrantes();
  }

  public alterarImagem() {
    // this.mostrarImagem = !this.mostrarImagem;
  }

  public carrgarPalestrantes(): void {
    this.spinnerService.show();
    setTimeout(() => {
      this.spinnerService.hide();
    }, 100);

    const observer = {
      next: (paginatedResult: PaginatedResult<Palestrante[]>) => {
        this.palestrantes = paginatedResult.result;
        this.pagination = paginatedResult.pagination;
      },
      error: (erro: any) => {
        this.spinnerService.hide();
        this.toastrService.error('Erro ao carregar Eventos!', 'Verifique');
        console.log(erro);
      },
      complete: () => this.spinnerService.hide()
    }
    this.palestranteService.getPalestrantes(this.pagination.currentPage,
      this.pagination.itemsPerPage).subscribe(observer);
  }

  // openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
  //   event.stopPropagation();
  //   this.eventoId = eventoId;
  //   this.modalRef = this.modalService.show(template, {
  //     class: 'modal-sm'
  //   });
  // }

  confirm(): void {
    // this.modalRef?.hide();
    // this.spinnerService.show();

    // this.eventoService.deleteEvento(this.eventoId).subscribe(
    //   (result: any) => {
    //     if (result.message === 'Deletado!') {
    //       this.toastrService.success('O Evento foi deletado com sucesso!', 'Deletado!');
    //       this.getEventos();
    //     }
    //   },
    //   (error: any) => {
    //     console.log(error);
    //     this.toastrService.error(`Erro ao tentar deletar o evento ${this.eventoId}`, 'Erro!');
    //   }
    // ).add(() => this.spinnerService.hide());

    // this.toastrService.success('Evento deletado com sucesso.', 'Deletado!');
  }

  decline(): void {
    // this.modalRef?.hide();
  }

  public detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
