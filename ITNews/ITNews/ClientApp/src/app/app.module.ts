import { AdminModule } from './Admin/admin.module';
import { AuthService } from './Shared/services/auth.service';
import { HomeComponent } from './Core/components/home/home.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './Core/core.module';
import { RouterModule } from '@angular/router';
import { CreateNewsComponent } from './News/components/create-news/create-news.component';
import { UserModule } from './User/user.module';
import { SharedModule } from './Shared/shared.module';
import { NewsModule } from './News/news.module';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    UserModule,
    NewsModule,
    AdminModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'create-news', component: CreateNewsComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
