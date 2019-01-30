import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsSuccessSavingComponent } from './success-news-saving.component';

describe('NewsSuccessSavingComponent', () => {
  let component: NewsSuccessSavingComponent;
  let fixture: ComponentFixture<NewsSuccessSavingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsSuccessSavingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsSuccessSavingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
