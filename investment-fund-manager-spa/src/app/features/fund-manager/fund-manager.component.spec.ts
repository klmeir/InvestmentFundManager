import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FundManagerComponent } from './fund-manager.component';

describe('FundManagerComponent', () => {
  let component: FundManagerComponent;
  let fixture: ComponentFixture<FundManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FundManagerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FundManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
