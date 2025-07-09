import { Component } from '@angular/core';
import { PaymentProviderDto } from '../../../dto/paymentProviderDto';
import { PaymentProviderService } from '../../../services/payment-provider-service/payment-provider.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NotificationService } from '../../../services/notification-service/notification.service';

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
    private notificationService: NotificationService,
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

  changeActiveStatus(provider: PaymentProviderDto) {
    // Toggle the active status of the payment provider
    provider.isActive = !provider.isActive;

    this.paymentProviderService.updatePaymentProvider(provider).subscribe({
      next: (updatedProvider) => {
        // Update the local list with the updated provider
        this.loadPaymentProviders();
      },
      error: (err) => {
        console.error('Error updating payment provider status', err);
        this.notificationService.showError(err);
      }
    });
  }

  selectPaymentProvider(provider: PaymentProviderDto) {
    // Logic to handle payment provider selection
    this.router.navigate(['/providers', provider.id]);
  }

  editPaymentProvider(provider: PaymentProviderDto) {
    // Navigate to the edit form for the selected payment provider
    this.router.navigate(['/providers/edit', provider.id]);
  }

  deletePaymentProvider(provider: PaymentProviderDto) {
    this.paymentProviderService.deletePaymentProvider(provider.id.toString()).subscribe({
      next: () => {
        this.loadPaymentProviders(); // Reload the list after deletion
      },
      error: (err) => {
        console.error('Error deleting payment provider', err);
        this.notificationService.showError(err);
      }
    });
  }

  addNewPaymentProvider() {
    // Navigate to the create form for a new payment provider
    this.router.navigate(['/providers/create']);
  }
}
