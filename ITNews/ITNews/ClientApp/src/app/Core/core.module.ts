import { RouterModule } from '@angular/router';
import { AppNavMenuComponent } from './components/app-nav-menu/app-nav-menu.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from '../Core/components/home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../Shared/shared.module';
import { NewsCardListComponent } from '../Core/components/news-card-list/news-card-list.component';
import { NewsCardComponent } from '../Core/components/news-card/news-card.component';
import { MiniNewsCardComponent } from '../Core/components/mini-news-card/mini-news-card.component';
import { TagCloudModule } from 'angular-tag-cloud-module';
import { NoAccessComponent } from '../Core/components/no-access/no-access.component';

@NgModule({
  declarations: [
    AppNavMenuComponent,
    HomeComponent,
    NewsCardComponent,
    NewsCardListComponent,
    MiniNewsCardComponent,
    NoAccessComponent
  ],
  imports: [
    SharedModule,
    TagCloudModule,
    RouterModule.forChild([
    ])
  ],
  exports: [
    AppNavMenuComponent,
    HomeComponent,
    NoAccessComponent
  ]
})
export class CoreModule { }
