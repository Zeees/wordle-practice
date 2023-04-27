import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LetterBoxComponent } from './letter-box.component';

describe('LetterBoxComponent', () => {
  let component: LetterBoxComponent;
  let fixture: ComponentFixture<LetterBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LetterBoxComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LetterBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
