import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './components/eventos/eventos.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EventoService } from './services/evento.service';
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';

import { TooltipModule } from "ngx-bootstrap/tooltip";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ModalModule } from "ngx-bootstrap/modal";
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { TituloComponent } from './components/titulo/titulo.component';
import { EventosDetalheComponent } from './components/eventos/eventos-detalhe/eventos-detalhe.component';
import { EventosListaComponent } from './components/eventos/eventos-lista/eventos-lista.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { defineLocale, ptBrLocale } from 'ngx-bootstrap/chronos';
import { LoteService } from './services/lote.service';
import { NgxCurrencyDirective } from "ngx-currency";
import { jwtInterceptor } from './interceptors/jwt.interceptor';
import { AccountService } from './services/AccountService.service';
import { HomeComponent } from './components/home/home.component';
import { PaginationModule } from "ngx-bootstrap/pagination";
import { TabsModule } from "ngx-bootstrap/tabs";
import { PerfilDetalheComponent } from './components/user/perfil/perfil-detalhe/perfil-detalhe.component';
import { PalestranteListaComponent } from './components/palestrantes/palestrante-lista/palestrante-lista.component';
import { PalestranteDetalheComponent } from './components/palestrantes/palestrante-detalhe/palestrante-detalhe.component';
import { RedesocialService } from './services/redesocial.service';
import { RedesSociaisComponent } from './components/redesSociais/redesSociais.component';

defineLocale('pt-br', ptBrLocale);
@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    EventosComponent,
    PalestrantesComponent,
    PalestranteListaComponent,
    PalestranteDetalheComponent,
    NavComponent,
    DateTimeFormatPipe,
    ContatosComponent,
    DashboardComponent,
    PerfilComponent,
    RedesSociaisComponent, 
    PerfilDetalheComponent,
    TituloComponent,
    EventosDetalheComponent,
    EventosListaComponent,
    UserComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent],
    imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      BrowserAnimationsModule,
      CollapseModule,
      FormsModule,
      ReactiveFormsModule,
      TooltipModule.forRoot(),
      BsDropdownModule.forRoot(),
      ModalModule.forRoot(),
      ToastrModule.forRoot({
        timeOut: 10000,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
        progressBar: true
      }),
      NgxSpinnerModule,
      PaginationModule.forRoot(),
      NgxCurrencyDirective,
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot()
    ],
    providers: [
      EventoService,
      LoteService,
      AccountService,
      RedesocialService,
      {
        provide: HTTP_INTERCEPTORS, useClass: jwtInterceptor, multi: true
      },
    ],
    bootstrap: [AppComponent]
  })
  export class AppModule { }
