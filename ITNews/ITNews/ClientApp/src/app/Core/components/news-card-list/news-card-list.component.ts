import { NewsQuery } from './../../../News/models/NewsQuery';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { NewsService } from 'src/app/News/services/news.service';
import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NewsCard } from 'src/app/Shared/models/news-card';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'news-card-list',
  templateUrl: './news-card-list.component.html',
  styleUrls: ['./news-card-list.component.css']
})
export class NewsCardListComponent implements OnChanges {
  @Input() class: string;
  @Input() page: number;
  @Input() pageSize: number;
  @Output() totalItem: EventEmitter<any> = new EventEmitter<any>();

  title: string;
  news: NewsCard[];
  queryObject: NewsQuery;

  constructor(
    private newsService: NewsService,
    private authService: AuthService
  ) {
    this.news = [];
    this.queryObject = <NewsQuery>{};
    this.queryObject.isSortAscending = false;
    this.loadData();
  }

  ngOnChanges(changes: SimpleChanges) {
    switch (this.class) {
      case 'latest':
      default:
        this.title = 'Latest News';
        this.queryObject.sortBy = 'ModifiedAt';
        break;
      case 'popular':
        this.title = 'Most Popular News';
        this.queryObject.sortBy = 'Rating';
        break;
      case 'mostViewed':
        this.title = 'Most Viewed News';
        this.queryObject.sortBy = 'VisitorCount';
        break;
    }
      this.queryObject.pageSize = this.pageSize;
      this.queryObject.page = this.page;
      this.loadData();
  }

  loadData() {
    this.newsService.getNews(this.queryObject)
      .subscribe(res => {
        console.log('News has been recieved');
        this.news = res.items;
        this.totalItem.emit(res.totalItems);
      }, error => console.log(error));
  }

}
