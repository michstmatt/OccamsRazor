import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostQuestionEditorComponent } from './host-question-editor.component';

describe('HostQuestionEditorComponent', () => {
  let component: HostQuestionEditorComponent;
  let fixture: ComponentFixture<HostQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HostQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HostQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
