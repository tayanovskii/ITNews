import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-news',
  templateUrl: './create-news.component.html',
  styleUrls: ['./create-news.component.css']
})
export class CreateNewsComponent implements OnInit {
  news: SaveNews;
  constructor() {
      this.news = <SaveNews> {};
   }

  ngOnInit() {
  }

}
