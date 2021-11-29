import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayWaitComponent } from './play-wait.component';

describe('PlayWaitComponent', () => {
  let component: PlayWaitComponent;
  let fixture: ComponentFixture<PlayWaitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayWaitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayWaitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
