import { Component } from '@angular/core';
import { PaymentProviderDto } from '../../../dto/paymentProviderDto';
import { PaymentProviderService } from '../../../services/payment-provider.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

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
    private router: Router,
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
    this.paymentProviderService.deletePaymentProvider(provider.id.toString()).subscribe({
      next: () => {
        this.loadPaymentProviders(); // Reload the list after deletion
      },
      error: (err) => {
        console.error('Error deleting payment provider', err);
      }
    });
  }

  addNewPaymentProvider() {
    // Navigate to the create form for a new payment provider
    this.router.navigate(['/providers/create']);
  }
}
