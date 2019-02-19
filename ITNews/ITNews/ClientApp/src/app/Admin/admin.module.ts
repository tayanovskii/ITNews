import { ManageUserService } from './services/manage-user.service';
import { NgModule } from '@angular/core';
import { SharedModule } from '../Shared/shared.module';
import { DashboardComponent } from '../Admin/components/dashboard/dashboard.component';
import { UsersComponent } from '../Admin/components/users/users.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    DashboardComponent,
    UsersComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'admin/users', component: UsersComponent },
      { path: 'admin/dashboard', component: DashboardComponent }
    ])
  ],
  exports: [
    RouterModule
  ],
  providers: [
    ManageUserService
  ]

})
export class AdminModule { }
