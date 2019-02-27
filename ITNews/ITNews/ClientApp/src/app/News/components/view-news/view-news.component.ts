import { CommentLike } from './../../models/CommentLike';
import { SaveCommentLike } from './../../models/SaveCommentLike';
import { CommentLikeService } from './../../services/comment-likes.service';
import { SaveRating } from './../../models/SaveRating';
import { RatingService } from './../../services/rating.service';
import { CommentService } from './../../services/comment.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, Inject } from '@angular/core';
import { NewsService } from '../../services/news.service';
import { News } from '../../models/News';
import { faUserCircle, faSortAlphaDown } from '@fortawesome/free-solid-svg-icons';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { SaveNews } from '../../models/SaveNews';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Observable, forkJoin } from 'rxjs';
import { CategoryService } from 'src/app/Shared/services/category.service';
import { CategoryStatistic } from 'src/app/Shared/models/CategoryStatistic';
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
  categories: CategoryStatistic[];
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
    private likeService: CommentLikeService,
    private categoryService: CategoryService,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    // this.news = <News>{};
    // this.news.tags = [];
    // this.news.categories = [];
  }

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn;
    const id = +this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      forkJoin(this.newsService.getNewsById(id),
      this.categoryService.getCountNewsForCategories(id))
      .subscribe(res => {
        this.news = res[0];
        if (!this.authService.isLoggedIN()) {
          this.news.commentsLikedByUser = [];
        }
        this.categories = res[1];
      }, error => console.log(error));

      // this.newsService.getNewsById(id)
      //   .subscribe(res => {
      //     this.news = res;
      //   }, error => console.log(error));
    }

    this._hubConnection = new HubConnectionBuilder().withUrl(`${this.baseUrl}commentHub?newsId=${id}`).build();
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
    console.log('This comment will be add to this news ' + JSON.stringify(comment));
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

    // TO-DO - use changeCountLikeForComment in NgZone
    console.log(commentId);
    const likeObj = <SaveCommentLike> {
      userId: this.authService.getUserId(),
      commentId: commentId
    };
    const ind = this.news.commentsLikedByUser.indexOf(commentId);
    console.log('Changing element index in commentsLikedByUser array is ' + ind);
    console.log('Comment Id is ' + commentId);
    if (ind !== -1) { // like has to be deleted
      // this.likeService.removeLike()
      this.likeService.removeLike(likeObj)
    .subscribe(res => {
        this.news.commentsLikedByUser.splice(ind, 1);
        console.log('commentsLikedByUser: ' + this.news.commentsLikedByUser);
        const commentInd = this.news.comments.findIndex(c => c.id === res.commentId);
          if (commentInd !== -1) {
            this.news.comments[commentInd].countLikes = res.countLike;
          } else {
            console.log(`Comment with ${commentInd} doesn't exist`);
          }
        // this.changeCountLikeForComment(res);
      });
    } else { // like has to be added
        this.likeService.setLike(likeObj)
        .subscribe(res => {
          this.news.commentsLikedByUser.push(commentId);
          const commentInd = this.news.comments.findIndex(c => c.id === res.commentId);
          if (commentInd !== -1 ) {
            this.news.comments[commentInd].countLikes = res.countLike;
          } else {
            console.log(`Comment with ${commentInd} already doesn't exist`);
          }
          //  this.changeCountLikeForComment(res);
        }, error => console.log(error));
    }
  }
  private changeCountLikeForComment(res: CommentLike) {
    const commentInd = this.news.comments.findIndex(c => c.id === res.commentId);
          if (commentInd) {
            this.news.comments[commentInd].countLikes = res.countLike;
          } else {
            console.log(`Comment with ${commentInd} doesn't exist`);
          }
  }

}
