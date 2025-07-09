import { TestBed } from '@angular/core/testing';

import { PaymentProviderService } from './payment-provider-service/payment-provider.servicee';

describe('PaymentProviderService', () => {
  let service: PaymentProviderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaymentProviderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
