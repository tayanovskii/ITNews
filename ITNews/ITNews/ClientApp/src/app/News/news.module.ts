import { CreateNewsComponent } from './components/create-news/create-news.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    CreateNewsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'news/create-news', component: CreateNewsComponent }
    ])
  ]
})
export class NewsModule { }
