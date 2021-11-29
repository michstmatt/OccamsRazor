import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayManagerComponent } from './play-manager.component';

describe('PlayManagerComponent', () => {
  let component: PlayManagerComponent;
  let fixture: ComponentFixture<PlayManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayManagerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
