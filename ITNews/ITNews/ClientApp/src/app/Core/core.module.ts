import { RouterModule } from '@angular/router';
import { AppNavMenuComponent } from './components/app-nav-menu/app-nav-menu.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from '../Core/components/home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../Shared/shared.module';

@NgModule({
  declarations: [
    AppNavMenuComponent,
    HomeComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([])
  ],
  exports: [
    AppNavMenuComponent,
    HomeComponent
  ]
})
export class CoreModule { }
