import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayAnswerComponent } from './play-answer.component';

describe('PlayAnswerComponent', () => {
  let component: PlayAnswerComponent;
  let fixture: ComponentFixture<PlayAnswerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayAnswerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayAnswerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
