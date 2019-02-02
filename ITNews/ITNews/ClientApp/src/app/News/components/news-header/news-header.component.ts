import { Component, OnInit, Input, OnChanges } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'news-header',
  templateUrl: './news-header.component.html',
  styleUrls: ['./news-header.component.css']
})
export class NewsHeaderComponent implements OnInit, OnChanges {
  // tslint:disable-next-line:no-input-rename
  @Input('user-card') user: UserProfileCard;
  // tslint:disable-next-line:no-input-rename
  @Input('time') publishedTime: Date;
  constructor() { }
  ngOnInit() {}
  ngOnChanges() {
  }

}
