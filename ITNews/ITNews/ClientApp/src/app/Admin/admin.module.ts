import { NgModule } from '@angular/core';
import { SharedModule } from '../Shared/shared.module';
import { DashboardComponent } from '../Admin/components/dashboard/dashboard.component';
import { UsersComponent } from '../Admin/components/users/users.component';

@NgModule({
  declarations: [DashboardComponent, UsersComponent],
  imports: [
    SharedModule
  ]
})
export class AdminModule { }
