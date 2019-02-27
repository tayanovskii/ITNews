import { CoreModule } from './../Core/core.module';
import { CommentLikeService } from './services/comment-likes.service';
import { CommentService } from './services/comment.service';
import { ViewNewsComponent } from './components/view-news/view-news.component';
import { CreateNewsComponent } from './components/create-news/create-news.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule, getBaseUrl } from '../Shared/shared.module';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
import { HttpClient } from '@angular/common/http';
import { AngularMarkdownEditorModule } from 'angular-markdown-editor';
import { MarkdownModule, MarkedOptions, MarkedRenderer } from 'ngx-markdown';
import { ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AuthGuard } from '../Shared/services/auth.guard';
import { NewsSuccessSavingComponent } from '../News/components/success-news-saving/success-news-saving.component';
import { NewsHeaderComponent } from '../News/components/news-header/news-header.component';
import { NewsCommentComponent } from '../News/components/news-comment/news-comment.component';
import { RatingService } from './services/rating.service';
import { OtherUserGuard } from '../Shared/services/other-user.guard';
import { WriterRoleGuard } from '../Shared/services/writer-role.guard';
import { SearchResultComponent } from '../News/components/search-result/search-result.component';
import { NewsCardComponent } from '../Core/components/news-card/news-card.component';

@NgModule({
  declarations: [
    CreateNewsComponent,
    NewsSuccessSavingComponent,
    ViewNewsComponent,
    NewsHeaderComponent,
    NewsCommentComponent,
    SearchResultComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    CoreModule,
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
      { path: 'news/edit/:id', component: CreateNewsComponent, canActivate: [AuthGuard, OtherUserGuard] },
      { path: 'news/create', component: CreateNewsComponent, canActivate: [AuthGuard, WriterRoleGuard]  },
      { path: 'news/success-edit/:id', component: NewsSuccessSavingComponent },
      { path: 'news/success-create/:id', component: NewsSuccessSavingComponent },
      { path: 'news/view-news/:id', component: ViewNewsComponent},
      { path: 'news/search-result', component: SearchResultComponent}
    ])
  ],
  providers: [
    CommentService,
    RatingService,
    CommentLikeService,
    {
      provide: 'BASE_URL',
      useFactory: getBaseUrl
    }
  ]
})
export class NewsModule { }
