import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';
import { faThumbsUp as emptyThumbsUp } from '@fortawesome/free-regular-svg-icons';
import { icon } from '@fortawesome/fontawesome-svg-core';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'news-comment',
  templateUrl: './news-comment.component.html',
  styleUrls: ['./news-comment.component.css']
})
export class NewsCommentComponent implements  OnChanges {
  @Input() comment: CommentCard;
  @Input() isLiked: boolean;
  @Input() canLike: boolean;
  @Output() changeLike: EventEmitter<any> = new EventEmitter();

  likeIcon = faThumbsUp;
  emptyLikeIcon = emptyThumbsUp;
  icon: any;
  constructor() {
    this.icon = this.isLiked ? this.likeIcon : this.emptyLikeIcon;
   }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isLiked'] !== undefined) {
      this.icon = this.isLiked ? this.likeIcon : this.emptyLikeIcon;
    }
  }
  setLike() {
    // console.log(this.comment.id);
    if (this.canLike) {
      this.changeLike.emit(this.comment.id);
    }
  }
}
