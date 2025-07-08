import { APP_BASE_HREF } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { of } from 'rxjs';
import { PaymentProviderDto } from '../dto/paymentProviderDto';

@Injectable({
  providedIn: 'root'
})
export class PaymentProviderService {
  constructor(@Inject("API_URL") private apiUrl: string, @Optional() private httpClient?: HttpClient) { }

  public getPaymentProviders() {
    if (this.httpClient) {
      return this.httpClient.get<PaymentProviderDto[]>(`${this.apiUrl}/PaymentProvider`);
    } else {
      return of([]);
    }
  }

}
