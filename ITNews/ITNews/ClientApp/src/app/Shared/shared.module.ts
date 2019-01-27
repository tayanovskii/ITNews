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

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    BrowserModule,
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
    {
      provide: 'BASE_URL',
      useValue: 'https://localhost:5001/',
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
    HttpClientModule
  ]
})
export class SharedModule { }
