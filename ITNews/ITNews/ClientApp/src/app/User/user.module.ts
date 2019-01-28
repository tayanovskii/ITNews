import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { LoginComponent } from '../User/components/login/login.component';
import { SharedModule } from '../Shared/shared.module';
import { AuthService } from '../Shared/services/auth.service';
import { RegistrationComponent } from '../User/components/registration/registration.component';
import { ChangePasswordComponent } from '../User/components/change-password/change-password.component';
import { UserProfileComponent } from '../User/components/user-profile/user-profile.component';
import { ProfilesComponent } from '../User/components/profiles/profiles.component';
import { MyNewsComponent } from '../User/components/my-news/my-news.component';
import { EditProfileComponent } from '../User/components/edit-profile/edit-profile.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegistrationComponent,
    ChangePasswordComponent,
    UserProfileComponent,
    ProfilesComponent,
    MyNewsComponent,
    EditProfileComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'user', component: LoginComponent },
      { path: 'user/login', component: LoginComponent},
      { path: 'user/registration', component: RegistrationComponent},
      { path: 'user/change-password', component: ChangePasswordComponent }
    ])
  ],
  exports: [
    RouterModule
  ],
  providers: [AuthService]
})
export class UserModule { }
