import { CommentService } from './../../services/comment.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NewsService } from '../../services/news.service';
import { News } from '../../models/News';
import { faUserCircle, faSortAlphaDown } from '@fortawesome/free-solid-svg-icons';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { SaveNews } from '../../models/SaveNews';
import { AuthService } from 'src/app/Shared/services/auth.service';
@Component({
  selector: 'app-view-news',
  templateUrl: './view-news.component.html',
  styleUrls: ['./view-news.component.css']
})
export class ViewNewsComponent implements OnInit {
  news: News = <News>{
    tags: [],
    categories: []
  };
  userIcon = faUserCircle;
  commentSortIcon = faSortAlphaDown;
  currentComment = '';
  private _hubConnection: HubConnection;

  constructor(
    private activatedRoute: ActivatedRoute,
    private newsService: NewsService,
    private commentService: CommentService,
    private authService: AuthService
  ) {
    // this.news = <News>{};
    // this.news.tags = [];
    // this.news.categories = [];
  }

  ngOnInit() {
    const id = +this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.newsService.getNewsById(id)
        .subscribe(res => {
          this.news = res;
          console.log(this.news.userMiniCardDto);
          // console.log('Full news for view-news component -> ' + JSON.stringify(res));
          console.log('Comments news-> ' + JSON.stringify(res.comments));
        }, error => console.log(error));
    }
    this._hubConnection = new HubConnectionBuilder().withUrl(`https://localhost:5001/commentHub?newsId=${this.news.id}`).build();
    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('AddComment', (comment: CommentCard) => {
      console.log('comment from hubcontroller' + JSON.stringify(comment));
      this.news.comments.push(comment);
    });
  }
  addComment() {
    const comment = <SaveComment> {
      content: this.currentComment,
      userId: this.authService.getUserId(),
      newsId: this.news.id

    };
    this.commentService.createComment(comment)
    .subscribe(res => {
      console.log('Comment has been added');
      this.currentComment = '';
    }, error => console.log(error));
  }
  sortByDate() {
    this.news.comments.sort((a, b) => {
      if (a.modifiedAt > b.modifiedAt) {
        return -1;
      }
      return 1;
    });
  }
  sortByLikes() {
    this.news.comments.sort((a, b) => {
      if (a.countLikes > b.countLikes) {
        return -1;
      }
      return 1;
    });
  }
  changeRating($event) {
    console.log('Rating has been changed' + $event);
  }
}
