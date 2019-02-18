import { ArrayHelpers } from './../../../Shared/helpers/ArrayHelpers';
import { NewsQuery } from './../../../News/models/NewsQuery';
import { NewsCard } from 'src/app/Shared/models/news-card';
import { NewsService } from './../../../News/services/news.service';
import { Component, OnInit } from '@angular/core';
import { CloudOptions, CloudData } from 'angular-tag-cloud-module';
import { TagService } from 'src/app/Shared/services/tag.service';

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
  // tag cloud options
  options: CloudOptions = {
    // if width is between 0 and 1 it will be set to the size of the upper element multiplied by the value
    width : 1,
    height : 400,
    overflow: true,
  };
  cloudData: CloudData[] = [
    { text: 'weight-5', weight: 8 , color: 'yellow'},
    { text: 'weight-7', weight: 7 },
    { text: 'weight-9', weight: 9 }
    // ...
  ];
  constructor(
    private newsService: NewsService,
    private tagService: TagService
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
