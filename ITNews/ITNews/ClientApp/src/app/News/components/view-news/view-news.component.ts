import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NewsService } from '../../services/news.service';
import { News } from '../../models/News';

@Component({
  selector: 'app-view-news',
  templateUrl: './view-news.component.html',
  styleUrls: ['./view-news.component.css']
})
export class ViewNewsComponent implements OnInit {
  news: News = <News>{
    tags: [],
    categories: []
  };
  constructor(
    private activatedRoute: ActivatedRoute,
    private newsService: NewsService
  ) {
    // this.news = <News>{};
    // this.news.tags = [];
    // this.news.categories = [];
    const id = +this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.newsService.getNewsById(id)
        .subscribe(res => {
          this.news = res;
          console.log(this.news.userMiniCardDto);
          console.log('Full news for view-news component -> ' + JSON.stringify(res));
        }, error => console.log(error));
    }
  }

  ngOnInit() {
    // const id = +this.activatedRoute.snapshot.paramMap.get('id');
    // if (id) {
    //   this.newsService.getNewsById(id)
    //     .subscribe(res => {
    //       this.news = res;
    //       console.log('Full news for view-news component -> ' + JSON.stringify(res));
    //     }, error => console.log(error));

    // }
  }

}
