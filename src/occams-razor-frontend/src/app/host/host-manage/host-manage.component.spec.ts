import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostManageComponent } from './host-manage.component';

describe('HostManageComponent', () => {
  let component: HostManageComponent;
  let fixture: ComponentFixture<HostManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HostManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HostManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
