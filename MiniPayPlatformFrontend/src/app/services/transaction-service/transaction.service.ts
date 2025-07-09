import { Injectable, Inject } from '@angular/core';
import { TransactionRequestDto } from '../../dto/transactionRequestDto';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TransactionResponseDto } from '../../dto/transactionResponseDto';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  constructor(
    @Inject('API_URL') private apiUrl: string,
    private httpClient: HttpClient,
  ) { }

  public processTransaction(transactionRequest: TransactionRequestDto): Observable<TransactionResponseDto> {
    return this.httpClient.post<TransactionResponseDto>(
      `${this.apiUrl}/Transaction`,
      transactionRequest
    );
  }
}
