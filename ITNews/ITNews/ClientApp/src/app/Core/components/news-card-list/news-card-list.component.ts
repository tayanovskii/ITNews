import { AuthService } from 'src/app/Shared/services/auth.service';
import { NewsService } from 'src/app/News/services/news.service';
import { Component, OnInit, Input } from '@angular/core';
import { NewsCard } from 'src/app/Shared/models/news-card';

@Component({
  selector: 'app-news-card-list',
  templateUrl: './news-card-list.component.html',
  styleUrls: ['./news-card-list.component.css']
})
export class NewsCardListComponent implements OnInit {
  @Input() class: string;
  title: string;
  news: NewsCard[];
  constructor(
    private newsService: NewsService,
    private authService: AuthService
  ) {
    this.news = [];
  }

  ngOnInit() {
    // switch (class) {
    //   case:
    // }
    // this.newsService.
  }

}
