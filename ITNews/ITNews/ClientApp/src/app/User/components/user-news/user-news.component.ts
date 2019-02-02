import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NewsService } from 'src/app/News/services/news.service';
import { NewsCard } from 'src/app/Shared/models/news-card';

@Component({
  selector: 'app-user-news',
  templateUrl: './user-news.component.html',
  styleUrls: ['./user-news.component.css']
})
export class UserNewsComponent implements OnInit {
  userNews: NewsCard[];
  constructor(
    private activatedRoute: ActivatedRoute,
    private newsServcie: NewsService) { }

  ngOnInit() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.newsServcie.getCardNewsByUserId(id)
        .subscribe(res => {
          this.userNews = res;
          console.log('user\'s news' + JSON.stringify(this.userNews));
        }, error => console.log(error));
    }
  }

}
