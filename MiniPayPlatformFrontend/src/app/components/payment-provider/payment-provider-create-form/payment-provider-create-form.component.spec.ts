import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentProviderCreateFormComponent } from './payment-provider-create-form.component';

describe('PaymentProviderCreateFormComponent', () => {
  let component: PaymentProviderCreateFormComponent;
  let fixture: ComponentFixture<PaymentProviderCreateFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentProviderCreateFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentProviderCreateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
