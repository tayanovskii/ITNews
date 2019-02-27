import { Component, OnInit, Input } from '@angular/core';
import { NewsCard } from '../../../Shared/models/news-card';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'news-card',
  templateUrl: './news-card.component.html',
  styleUrls: ['./news-card.component.css']
})
export class NewsCardComponent implements OnInit {
  // tslint:disable-next-line:no-input-rename
  @Input('card') card: NewsCard;
  @Input() isForSearch = false;
  constructor() { }

  ngOnInit() {
  }
  changeRating($event) {
    console.log('rating has been changed' + $event);
  }
}
