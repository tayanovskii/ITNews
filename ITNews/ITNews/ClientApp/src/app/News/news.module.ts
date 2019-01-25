import { CreateNewsComponent } from './components/create-news/create-news.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
import { MarkdownModule } from 'ngx-markdown';
import { HttpClient } from '@angular/common/http';
@NgModule({
  declarations: [
    CreateNewsComponent
  ],
  imports: [
    SharedModule,
    MarkdownModule.forRoot({ loader: HttpClient }),
    NguiAutoCompleteModule,
    RouterModule.forChild([
      { path: 'news/create-news', component: CreateNewsComponent },
      { path: 'news/create-news/:id', component: CreateNewsComponent }
    ])
  ]
})
export class NewsModule { }
