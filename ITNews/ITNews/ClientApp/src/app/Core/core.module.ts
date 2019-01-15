import { RouterModule } from '@angular/router';
import { CreateNewsComponent } from './components/create-news/create-news.component';
import { AppNavMenuComponent } from './components/app-nav-menu/app-nav-menu.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from '../Core/components/home/home.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [AppNavMenuComponent, HomeComponent, CreateNewsComponent],
  imports: [
    CommonModule,
    NgbModule,
    RouterModule.forChild([])
  ],
  exports: [
    AppNavMenuComponent,
    HomeComponent,
    CreateNewsComponent,
    NgbModule.forRoot().ngModule,
  ]
})
export class CoreModule { }
