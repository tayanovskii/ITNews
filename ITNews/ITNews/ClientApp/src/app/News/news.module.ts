import { CreateNewsComponent } from './components/create-news/create-news.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
@NgModule({
  declarations: [
    CreateNewsComponent
  ],
  imports: [
    SharedModule,
    NguiAutoCompleteModule,
    RouterModule.forChild([
      { path: 'news/create-news', component: CreateNewsComponent },
      { path: 'news/create-news/:id', component: CreateNewsComponent }
    ])
  ]
})
export class NewsModule { }
