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
  constructor() {
    // by default
    this.page = 1;
    this.pageSize = 5;
    this.totalItems = 20;
   }

  ngOnInit() {

  }
  changeTotalItems($event) {
    this.totalItems = $event;
  }
}
