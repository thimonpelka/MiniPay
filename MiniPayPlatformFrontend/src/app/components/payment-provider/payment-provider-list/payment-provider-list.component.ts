import { Component } from '@angular/core';
import { PaymentProviderDto } from '../../../dto/paymentProviderDto';
import { PaymentProviderService } from '../../../services/payment-provider.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-provider-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payment-provider-list.component.html',
  styleUrl: './payment-provider-list.component.css'
})
export class PaymentProviderListComponent {
  paymentProviders: PaymentProviderDto[] = [];

  constructor(
    private paymentProviderService: PaymentProviderService,
  ) {}

  ngOnInit() {
    this.loadPaymentProviders();
  }

  loadPaymentProviders() {
    this.paymentProviderService.getPaymentProviders().subscribe({
      next: (providers) => {
        this.paymentProviders = providers;
      },
      error: (err) => {
        console.error('Error loading payment providers', err);
      }
    });
  }

  selectPaymentProvider(provider: PaymentProviderDto) {
    // Logic to handle payment provider selection
    console.log('Selected Payment Provider:', provider);
  }

  editPaymentProvider(provider: PaymentProviderDto) {
    // Logic to handle editing a payment provider
    console.log('Edit Payment Provider:', provider);
  }

  deletePaymentProvider(provider: PaymentProviderDto) {
    // Logic to handle deleting a payment provider
    console.log('Delete Payment Provider:', provider);
  }

  addNewPaymentProvider() {
    // Logic to handle adding a new payment provider
    console.log('Add New Payment Provider');
  }
}
