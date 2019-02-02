import { Component, OnInit, Input } from '@angular/core';
import { NewsCard } from '../../models/news-card';

@Component({
  selector: 'app-news-card',
  templateUrl: './news-card.component.html',
  styleUrls: ['./news-card.component.css']
})
export class NewsCardComponent implements OnInit {
  @Input() card: NewsCard;

  constructor() { }

  ngOnInit() {

  }

}
