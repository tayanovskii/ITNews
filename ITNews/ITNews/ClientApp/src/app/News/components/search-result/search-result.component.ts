import { SearchNewsQuery } from 'src/app/News/models/SearchNewsQuery';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NewsService } from '../../services/news.service';
import { NewsCard } from 'src/app/Shared/models/news-card';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent implements OnInit {
  news: NewsCard[];
  query: SearchNewsQuery;
  totalItems = 10;
  pageSizeArray = [5, 10, 15, 20 , 25]
  constructor(
    private activatedRoute: ActivatedRoute,
    private newsService: NewsService
  ) {
    this.query = <SearchNewsQuery> {
      page: 1,
      pageSize: 20
    };
  }

  ngOnInit() {
    this.query.query = this.activatedRoute.snapshot.paramMap.get('query');
    this.searchNews();
    console.log('Search page ' + this.query.query);
  }
  searchNews() {
    this.newsService.searchNews(this.query)
    .subscribe(res => {
      this.news = res;
      console.log(JSON.stringify(this.news));
    }, error => console.log(error));

  }
  updateUserList($event) {
    console.log($event);
    this.query.page = $event;
    this.searchNews();
  }
  changePageSize(newPageSize) {
    this.query.pageSize = newPageSize;
    this.searchNews();
  }
}
