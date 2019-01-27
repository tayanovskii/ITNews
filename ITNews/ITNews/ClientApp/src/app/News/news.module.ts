import { CreateNewsComponent } from './components/create-news/create-news.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
import { HttpClient } from '@angular/common/http';
import { AngularMarkdownEditorModule } from 'angular-markdown-editor';
import { MarkdownModule, MarkedOptions, MarkedRenderer } from 'ngx-markdown';
import { ReactiveFormsModule } from '@angular/forms';
import { PhotoDownloadComponent } from '../News/components/photo-download/photo-download.component';


@NgModule({
  declarations: [
    CreateNewsComponent,
    PhotoDownloadComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    MarkdownModule.forRoot({
      loader: HttpClient,
      markedOptions: {
        provide: MarkedOptions,
        useValue: {
          renderer: new MarkedRenderer(),
          gfm: true,
          tables: true,
          breaks: false,
          pedantic: false,
          sanitize: false,
          smartLists: true,
          smartypants: false,
        },
      },
    }),
    AngularMarkdownEditorModule.forRoot({
      // add any Global Options/Config you might want
      // to avoid passing the same options over and over in each components of your App
      iconlibrary: 'glyph'
    }),
    // MarkdownModule.forRoot({ loader: HttpClient }),
    NguiAutoCompleteModule,
    RouterModule.forChild([
      { path: 'news/create-news', component: CreateNewsComponent },
      { path: 'news/create-news/:id', component: CreateNewsComponent },
      { path: 'news/download-photo', component: PhotoDownloadComponent}
    ])
  ]
})
export class NewsModule { }
