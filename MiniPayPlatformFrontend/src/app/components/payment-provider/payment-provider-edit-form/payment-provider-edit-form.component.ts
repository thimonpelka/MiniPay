import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { PaymentProviderService } from '../../../services/payment-provider-service/payment-provider.service';
import { NotificationService } from '../../../services/notification-service/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UpdatePaymentProviderDto } from '../../../dto/updatePaymentProviderDto';

@Component({
  selector: 'app-payment-provider-edit-form',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './payment-provider-edit-form.component.html',
  styleUrl: './payment-provider-edit-form.component.css',
})
export class PaymentProviderEditFormComponent {
  id: string | null = null;
  paymentProviderForm: FormGroup;
  isSubmitted = false;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private paymentProviderService: PaymentProviderService,
    private notificationService: NotificationService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.paymentProviderForm = this.fb.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100),
        ],
      ],
      url: [
        '',
        [
          Validators.required,
          Validators.pattern('https?://.+'),
          Validators.maxLength(200),
        ],
      ],
      isActive: [true],
      currency: [
        '',
        [Validators.required, Validators.minLength(3), Validators.maxLength(3)],
      ],
      description: ['', [Validators.maxLength(500)]],
    });
  }

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

  loadPaymentProvider() {
    if (!this.id) {
      this.notificationService.showError('Payment Provider ID is missing');
      this.router.navigate(['/providers']);
      return;
    }

    this.paymentProviderService.getPaymentProviderById(this.id).subscribe({
      next: (response) => {
        this.paymentProviderForm.patchValue({
          name: response.name,
          url: response.url,
          isActive: response.isActive,
          currency: response.currency,
          description: response.description || '',
        });
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

  onSubmit() {
    this.isSubmitted = true;

    if (this.paymentProviderForm.invalid) return;

    this.isLoading = true;

    const newPaymentProvider: UpdatePaymentProviderDto = {
      ...this.paymentProviderForm.value,
      id: this.id,
    };

    this.paymentProviderService
      .updatePaymentProvider(newPaymentProvider)
      .subscribe({
        next: (response) => {
          console.log('Payment Provider updated successfully:', response);
          this.router.navigate(['/providers']);
        },
        error: (error) => {
          console.error('Error update Payment Provider:', error);
          this.notificationService.showError(
            error.error || 'Failed to update payment provider',
          );
          this.isLoading = false;
          this.isSubmitted = false;
        },
      });
  }

  cancel() {
    this.router.navigate(['/providers']);
  }

  get f(): { [key: string]: AbstractControl } {
    return this.paymentProviderForm.controls as {
      [key: string]: AbstractControl;
    };
  }
}
