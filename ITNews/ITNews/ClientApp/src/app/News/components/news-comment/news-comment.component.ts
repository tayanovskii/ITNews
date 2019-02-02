import { Component, OnInit, Input } from '@angular/core';
import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';
// import { faThumbsUp as emptylikeIcon } from '@fortawesome/fontawesome-svg-core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'news-comment',
  templateUrl: './news-comment.component.html',
  styleUrls: ['./news-comment.component.css']
})
export class NewsCommentComponent implements OnInit {
  @Input() comment: CommentCard;
  likeIcon = faThumbsUp;
  constructor() { }

  ngOnInit() {
  }
  addLike() {}
}
