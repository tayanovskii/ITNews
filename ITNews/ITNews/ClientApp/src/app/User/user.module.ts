import { UserNewsComponent } from './components/user-news/user-news.component';
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
import { UserProfileService } from './services/user-profile.service';
import { CreateProfileComponent } from '../User/components/create-profile/create-profile.component';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}
@NgModule({
  declarations: [
    LoginComponent,
    RegistrationComponent,
    ChangePasswordComponent,
    UserProfileComponent,
    ProfilesComponent,
    MyNewsComponent,
    UserNewsComponent,
    CreateProfileComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'user', component: LoginComponent },
      { path: 'user/login', component: LoginComponent},
      { path: 'user/profiles/:userName', component: UserProfileComponent },
      { path: 'user/create-profile', component: CreateProfileComponent },
      { path: 'user/user-news/:id', component: UserNewsComponent},
      { path: 'user/registration', component: RegistrationComponent},
      { path: 'user/change-password', component: ChangePasswordComponent }
    ])
  ],
  exports: [
    RouterModule
  ],
  providers: [
    AuthService,
    UserProfileService,
    {
      provide: 'BASE_URL',
      useFactory: getBaseUrl
    },
  ]
})
export class UserModule { }
