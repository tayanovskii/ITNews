import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MiniNewsCardComponent } from './mini-news-card.component';

describe('MiniNewsCardComponent', () => {
  let component: MiniNewsCardComponent;
  let fixture: ComponentFixture<MiniNewsCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MiniNewsCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MiniNewsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
