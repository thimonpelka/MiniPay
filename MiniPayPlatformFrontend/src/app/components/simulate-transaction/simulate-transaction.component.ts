import { Component } from '@angular/core';
import { PaymentProviderService } from '../../services/payment-provider-service/payment-provider.service';
import { TransactionService } from '../../services/transaction-service/transaction.service';
import { PaymentProviderDto } from '../../dto/paymentProviderDto';
import { TransactionResponseDto } from '../../dto/transactionResponseDto';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TransactionRequestDto } from '../../dto/transactionRequestDto';
import { NotificationService } from '../../services/notification-service/notification.service';

@Component({
  selector: 'app-simulate-transaction',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './simulate-transaction.component.html',
  styleUrl: './simulate-transaction.component.css',
})
export class SimulateTransactionComponent {
  paymentProviders: PaymentProviderDto[] = [];
  response: TransactionResponseDto | null = null;
  transactionForm: FormGroup;
  isSubmitted = false;
  isLoading = false;
  failure = false;
  failureMessage = 'Transaction failed. Please try again later.';

  constructor(
    private transactionService: TransactionService,
    private paymentProviderService: PaymentProviderService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
  ) {
    this.transactionForm = this.fb.group({
      paymentProviderId: [-1, [Validators.required]],
      amount: [0, [Validators.required, Validators.min(0.1)]],
      referenceId: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.maxLength(200)]],
    });
  }

  ngOnInit() {
    this.loadPaymentProviders();
  }

  loadPaymentProviders() {
    this.paymentProviderService.getActivePaymentProviders().subscribe({
      next: (providers) => {
        this.paymentProviders = providers;
      },
      error: (err) => {
        console.error('Error loading payment providers', err);
        this.notificationService.showError('Failed to load payment providers');
      },
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.failure = false;

    if (this.transactionForm.invalid) return;

    this.isLoading = true;

    const newTransactionRequest: TransactionRequestDto = {
      ...this.transactionForm.value,
    };

    console.log(newTransactionRequest);

    this.transactionService
      .processTransaction(newTransactionRequest)
      .subscribe({
        next: (response) => {
          this.response = response;
          this.isSubmitted = false;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error processing transaction', err);
          this.failureMessage = err.error || 'Transaction failed. Please try again later.';
          // this.notificationService.showError(
          //   'Transaction failed: ' + err.message,
          // );
          this.failure = true;
          this.isSubmitted = false;
          this.isLoading = false;
          this.response = null;
        },
      });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.transactionForm.controls as {
      [key: string]: AbstractControl;
    };
  }
}
