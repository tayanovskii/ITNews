import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsStatComponent } from './news-stat.component';

describe('NewsStatComponent', () => {
  let component: NewsStatComponent;
  let fixture: ComponentFixture<NewsStatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsStatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsStatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
