import { APP_BASE_HREF } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PaymentProviderDto } from '../../dto/paymentProviderDto';
import { CreatePaymentProviderDto } from '../../dto/createPaymentProviderDto';
import { UpdatePaymentProviderDto } from '../../dto/updatePaymentProviderDto';

@Injectable({
  providedIn: 'root',
})
export class PaymentProviderService {
  constructor(
    @Inject('API_URL') private apiUrl: string,
    private httpClient: HttpClient,
  ) { }

  public getPaymentProviders(): Observable<PaymentProviderDto[]> {
    return this.httpClient.get<PaymentProviderDto[]>(
      `${this.apiUrl}/PaymentProvider`,
    );
  }

  public getPaymentProviderById(id: string): Observable<PaymentProviderDto> {
    return this.httpClient.get<PaymentProviderDto>(
      `${this.apiUrl}/PaymentProvider/${id}`,
    );
  }

  public createPaymentProvider(
    provider: CreatePaymentProviderDto,
  ): Observable<PaymentProviderDto> {
    return this.httpClient.post<PaymentProviderDto>(
      `${this.apiUrl}/PaymentProvider`,
      provider,
    );
  }

  public updatePaymentProvider(
    provider: UpdatePaymentProviderDto,
  ): Observable<PaymentProviderDto> {
    return this.httpClient.put<PaymentProviderDto>(
      `${this.apiUrl}/PaymentProvider/${provider.id}`,
      provider,
    );
  }

  public deletePaymentProvider(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/PaymentProvider/${id}`);
  }
}
