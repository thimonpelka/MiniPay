import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimulateTransactionComponent } from './simulate-transaction.component';

describe('SimulateTransactionComponent', () => {
  let component: SimulateTransactionComponent;
  let fixture: ComponentFixture<SimulateTransactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SimulateTransactionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SimulateTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
