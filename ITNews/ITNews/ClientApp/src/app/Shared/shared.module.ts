import { WriterRoleGuard } from './services/writer-role.guard';
import { AuthGuard } from './services/auth.guard';
import { FormsModule } from '@angular/forms';
import { CategoryService } from './services/category.service';
import { TagService } from './services/tag.service';
import { AuthService } from './services/auth.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { NewsService } from '../News/services/news.service';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PhotoService } from './services/photo-service';
import { UserCardComponent } from '../Shared/components/user-card/user-card.component';
import { NewsStatComponent } from '../Shared/components/news-stat/news-stat.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { FileUploadModule } from 'primeng/fileupload';
import { OtherUserGuard } from './services/other-user.guard';
import { NewsCardComponent } from '../Core/components/news-card/news-card.component';
export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

@NgModule({
  declarations: [
    UserCardComponent,
    NewsStatComponent],
  imports: [
    CommonModule,
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    BrowserModule,
    NgbModule,
    RouterModule,
    FileUploadModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot() // ToastrModule added
    // JwtModule.forRoot({
    //   config: {
    //     tokenGetter: () => {
    //       return localStorage.getItem('ITNews_access_token');
    //     }
    //   }
    // })
  ],
  providers: [
    AuthService,
    NewsService,
    TagService,
    PhotoService,
    CategoryService,
    OtherUserGuard,
    AuthGuard,
    WriterRoleGuard,
    {
      provide: 'BASE_URL',
      useFactory: getBaseUrl
    },
    { provide: 'TOKEN',
      useValue: 'ITNews_access_token'
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  exports: [
    CommonModule,
    FormsModule,
    FontAwesomeModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NgbModule.forRoot().ngModule,
    UserCardComponent,
    ToastrModule,
    FileUploadModule,
    NewsStatComponent
  ]
})
export class SharedModule { }
