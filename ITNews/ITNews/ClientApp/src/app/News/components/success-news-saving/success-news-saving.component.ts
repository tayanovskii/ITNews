import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'success-news-saving',
  templateUrl: './success-news-saving.component.html',
  styleUrls: ['./success-news-saving.component.css']
})
export class NewsSuccessSavingComponent implements OnInit {
  newsId: string;
  action: string;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.newsId = this.activatedRoute.snapshot.queryParamMap.get('id');
    this.action = this.activatedRoute.snapshot.url[1].path.includes('edit')
    ? 'modifed'
    : 'created';
  }

}
