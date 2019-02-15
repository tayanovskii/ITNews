import { Component, OnInit, Input } from '@angular/core';
import { NewsCard } from 'src/app/Shared/models/news-card';
import { faComments, faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'mini-news-card',
  templateUrl: './mini-news-card.component.html',
  styleUrls: ['./mini-news-card.component.css']
})
export class MiniNewsCardComponent implements OnInit {
  @Input() news: NewsCard;
  commentsIcon = faComments;
  eyeIco = faEye;

  constructor() { }

  ngOnInit() {
  }

}
