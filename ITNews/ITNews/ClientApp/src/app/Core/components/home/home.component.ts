import { Category } from './../../../Shared/models/category';
import { CategoryService } from 'src/app/Shared/services/category.service';
import { ArrayHelpers } from './../../../Shared/helpers/ArrayHelpers';
import { NewsQuery } from './../../../News/models/NewsQuery';
import { NewsCard } from 'src/app/Shared/models/news-card';
import { NewsService } from './../../../News/services/news.service';
import { Component, OnInit } from '@angular/core';
import { CloudOptions, CloudData, ZoomOnHoverOptions } from 'angular-tag-cloud-module';
import { TagService } from 'src/app/Shared/services/tag.service';
import { text } from '@fortawesome/fontawesome-svg-core';
import { Tag } from 'src/app/Shared/models/tag';
import { forkJoin } from 'rxjs';

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
  pageSizeForMostViewed;
  mostViewedNews: NewsCard[];

  // tag cloud options and data
  cloudData: CloudData[] = [];
  options: CloudOptions = {
    // if width is between 0 and 1 it will be set to the size of the upper element multiplied by the value
    width : 1,
    height : 200,
    overflow: false,
  };
  zoomOnHoverOptions: ZoomOnHoverOptions = {
    scale: 1.3, // Elements will become 130 % of current zize on hover
    transitionTime: 0.2, // it will take 1.2 seconds until the zoom level defined in scale property has been reached
    delay: 0.5 // Zoom will take affect after 0.8 seconds
  };

  constructor(
    private newsService: NewsService,
    private tagService: TagService
  ) {
    // by default
    this.page = 1;
    this.pageSize = 5;
    this.totalItems = 20;
    this.pageSizeForMostViewed = 10;
    this.mostViewedNews = <NewsCard[]>{};
    this.pageSizeArray = ArrayHelpers.array_range(5, 11);
    console.log(this.pageSizeArray);
    this.tagService.getTagsForCloud()
    .subscribe(res => {
        res.forEach(t => {
        console.log(JSON.stringify(t));
        this.cloudData.push(<CloudData>{ text: t.name, weight: t.entryCount * 1.2, color: 'dark-blue' });
      });
    });
  }

  ngOnInit() {
    this.newsService.getNews(<NewsQuery>{
      sortBy: 'VisitorCount',
      pageSize: this.pageSizeForMostViewed,
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
  showTag($event) {
    console.log(JSON.stringify($event));
  }
}
