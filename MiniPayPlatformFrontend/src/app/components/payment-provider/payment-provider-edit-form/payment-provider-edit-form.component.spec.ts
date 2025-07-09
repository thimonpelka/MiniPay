import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentProviderEditFormComponent } from './payment-provider-edit-form.component';

describe('PaymentProviderEditFormComponent', () => {
  let component: PaymentProviderEditFormComponent;
  let fixture: ComponentFixture<PaymentProviderEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentProviderEditFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentProviderEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
