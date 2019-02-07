import { SaveCommentLike } from './../../models/SaveCommentLike';
import { CommentLikeService } from './../../services/comment-likes.service';
import { SaveRating } from './../../models/SaveRating';
import { RatingService } from './../../services/rating.service';
import { CommentService } from './../../services/comment.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NewsService } from '../../services/news.service';
import { News } from '../../models/News';
import { faUserCircle, faSortAlphaDown } from '@fortawesome/free-solid-svg-icons';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { SaveNews } from '../../models/SaveNews';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Observable } from 'rxjs';
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
  isLoggedIn$: Observable<boolean>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private newsService: NewsService,
    private commentService: CommentService,
    private authService: AuthService,
    private ratingService: RatingService,
    private likeService: CommentLikeService
  ) {
    // this.news = <News>{};
    // this.news.tags = [];
    // this.news.categories = [];
  }

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn;
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
    this._hubConnection = new HubConnectionBuilder().withUrl(`https://localhost:5001/commentHub?newsId=${id}`).build();
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
  changeRating(ratingValue) {
    const newRatingData: SaveRating = {
      newsId: this.news.id,
      value: ratingValue,
      userId: this.authService.getUserId()
    };
    this.ratingService.createRating(newRatingData)
    .subscribe(res => {
      console.log(JSON.stringify(res));
      this.news.isNewsRatedByUser = true;
      this.news.newsStatistic.rating = res.rating;
      this.news.newsStatistic.ratingCount = res.ratingCount;
      }, error => console.log(error));
  }
  changeLike(commentId: number) {
    console.log(commentId);
    const ind = this.news.commentsLikedByUser.indexOf(commentId, 0);
    if (ind !== -1) { // like has to be deelted
      // this.likeService.removeLike()
    } else { // like has to be added
      this.news.commentsLikedByUser.push(commentId);

      this.news.commentsLikedByUser.slice(ind, 1);
      const likeObject: SaveCommentLike = {
        commentId: commentId,
        userId: this.authService.getUserId()
      };
      this.likeService.setLike(likeObject)
        .subscribe(res => {
          console.log('Like Info has been changed on the server');
          console.log('Like Info' + JSON.stringify(res));
          const commentInd = this.news.comments.findIndex(c => c.id === res.commentId);
          if (commentInd) {
            this.news.comments[commentInd].countLikes = res.countLike;
          } else {
            console.log(`Comment with ${commentInd} has been deleted`);
          }
        }, error => console.log(error));
    }
  }

}
