import { Component, OnInit, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { faComments, faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-news-stat',
  templateUrl: './news-stat.component.html',
  styleUrls: ['./news-stat.component.css']
})
export class NewsStatComponent implements OnInit, OnChanges {
  @Input() newsStat: NewsStat;
  @Output() changeRating: EventEmitter<any> = new EventEmitter();
  commentsIcon = faComments;
  eyeIco = faEye;
  currentRate: number;
  constructor() {
   }
  ngOnInit() {
  }
  ngOnChanges() {
    this.currentRate = this.newsStat.rating;

  }
  setRating(newRating: number) {
     this.changeRating.emit(newRating);
  }
}
