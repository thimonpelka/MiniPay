import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentProviderListComponent } from './payment-provider-list.component';

describe('PaymentProviderListComponent', () => {
  let component: PaymentProviderListComponent;
  let fixture: ComponentFixture<PaymentProviderListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentProviderListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentProviderListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
