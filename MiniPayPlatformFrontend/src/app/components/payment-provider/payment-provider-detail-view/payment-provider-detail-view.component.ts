import { Component } from '@angular/core';
import { PaymentProviderDto } from '../../../dto/paymentProviderDto';
import { PaymentProviderService } from '../../../services/payment-provider-service/payment-provider.service';
import { NotificationService } from '../../../services/notification-service/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-provider-detail-view',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payment-provider-detail-view.component.html',
  styleUrl: './payment-provider-detail-view.component.css',
})
export class PaymentProviderDetailViewComponent {
  id: string | null = null;
  paymentProvider: PaymentProviderDto | null = null;

  constructor(
    private paymentProviderService: PaymentProviderService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = params.get('id');

      if (!this.id) {
        this.notificationService.showError('Invalid Route');
        this.router.navigate(['/providers']);
      }

      this.loadPaymentProvider();
    });
  }

  editPaymentProvider() {
    if (!this.id) {
      this.notificationService.showError('Payment Provider ID is missing');
      return;
    }

    this.router.navigate(['/providers/edit', this.id]);
  }

  changeActiveStatus() {
    if (!this.paymentProvider || !this.id) {
      this.notificationService.showError('Payment Provider ID is missing');
      return;
    }

    this.paymentProvider.isActive = !this.paymentProvider.isActive;

    this.paymentProviderService.updatePaymentProvider(this.paymentProvider).subscribe({
      next: (response) => {
        this.paymentProvider = response;
        // this.notificationService.showSuccess('Payment Provider status updated successfully');
      },
      error: (error) => {
        console.error('Error updating Payment Provider status:', error);
        this.notificationService.showError(
          error.error || 'Failed to update payment provider status',
        );
      },
    });
  }

  deletePaymentProvider() {
    if (!this.id) {
      this.notificationService.showError('Payment Provider ID is missing');
      return;
    }

    this.paymentProviderService.deletePaymentProvider(this.id).subscribe({
      next: () => {
        // this.notificationService.showSuccess('Payment Provider deleted successfully');
        this.router.navigate(['/providers']);
      },
      error: (error) => {
        console.error('Error deleting Payment Provider:', error);
        this.notificationService.showError(
          error.error || 'Failed to delete payment provider',
        );
      },
    });
  }

  loadPaymentProvider() {
    if (!this.id) {
      this.notificationService.showError('Payment Provider ID is missing');
      this.router.navigate(['/providers']);
      return;
    }

    this.paymentProviderService.getPaymentProviderById(this.id).subscribe({
      next: (response) => {
        this.paymentProvider = response;
      },
      error: (error) => {
        console.error('Error loading Payment Provider:', error);
        this.notificationService.showError(
          error.error || 'Failed to load payment provider',
        );
        this.router.navigate(['/providers']);
      },
    });
  }
}
