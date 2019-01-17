import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { LoginComponent } from '../User/components/login/login.component';
import { SharedModule } from '../Shared/shared.module';
import { AuthService } from '../Shared/services/auth.service';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'user', component: LoginComponent },
      { path: 'user/login', component: LoginComponent}
    ])
  ],
  exports: [
    RouterModule
  ],
  providers: [AuthService]
})
export class UserModule { }
