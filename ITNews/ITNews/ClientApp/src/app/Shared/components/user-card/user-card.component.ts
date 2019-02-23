import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit, OnChanges {
  // tslint:disable-next-line:no-input-rename
  @Input('user-card') userCard: UserProfileCard;
  likeImage = faThumbsUp;
  constructor() { }
  ngOnInit() {
  }
  ngOnChanges() {
  }


}
