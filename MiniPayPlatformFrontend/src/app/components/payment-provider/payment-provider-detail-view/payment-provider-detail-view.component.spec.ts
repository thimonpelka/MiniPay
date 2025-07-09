import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentProviderDetailViewComponent } from './payment-provider-detail-view.component';

describe('PaymentProviderDetailViewComponent', () => {
  let component: PaymentProviderDetailViewComponent;
  let fixture: ComponentFixture<PaymentProviderDetailViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentProviderDetailViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentProviderDetailViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
