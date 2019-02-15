import { ArrayHelpers } from './../../../Shared/helpers/ArrayHelpers';
import { NewsQuery } from './../../../News/models/NewsQuery';
import { NewsCard } from 'src/app/Shared/models/news-card';
import { NewsService } from './../../../News/services/news.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  page;
  pageSize;
  totalItems;
  pageSizeArray;
  pageSizeForMostViewsd;
  mostViewedNews: NewsCard[];
  constructor(
    private newsService: NewsService
  ) {
    // by default
    this.page = 1;
    this.pageSize = 5;
    this.totalItems = 20;
    this.pageSizeForMostViewsd = 10;
    this.mostViewedNews = <NewsCard[]>{};
    this.pageSizeArray = ArrayHelpers.array_range(5, 11);
    console.log(this.pageSizeArray);
  }

  ngOnInit() {
    this.newsService.getNews(<NewsQuery>{
      sortBy: 'VisitorCount',
      pageSize: this.pageSizeForMostViewsd,
      page: this.page
    }).subscribe(res => {
      this.mostViewedNews = res.items;
      console.log('Most Viewed News was received');
    });
  }
  changeTotalItems($event) {
    this.totalItems = $event;
  }
  changePageSize(newPageSize) {
    this.pageSize = newPageSize;
  }

}
