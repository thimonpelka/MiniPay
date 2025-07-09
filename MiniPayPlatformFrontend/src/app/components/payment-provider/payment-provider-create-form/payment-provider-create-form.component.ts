import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { PaymentProviderService } from '../../../services/payment-provider.service';
import { CreatePaymentProviderDto } from '../../../dto/createPaymentProviderDto';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-provider-create-form',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './payment-provider-create-form.component.html',
  styleUrl: './payment-provider-create-form.component.css',
})
export class PaymentProviderCreateFormComponent {
  paymentProviderForm: FormGroup;
  isSubmitted = false;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private paymentProviderService: PaymentProviderService,
    private router: Router,
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

  onSubmit() {
    this.isSubmitted = true;

    if (this.paymentProviderForm.invalid) return;

    this.isLoading = true;

    const newPaymentProvider: CreatePaymentProviderDto = {
      ...this.paymentProviderForm.value
    }

    this.paymentProviderService.createPaymentProvider(newPaymentProvider).subscribe({
      next: (response) => {
        console.log('Payment Provider created successfully:', response);
        this.router.navigate(['/providers']);
      },
      error: (error) => {
        console.error('Error creating Payment Provider:', error);
        this.isLoading = false;
        this.isSubmitted = false;
      }
    })
  }

  cancel() {
    this.router.navigate(['/providers']);
  }

  get f(): { [key: string]: AbstractControl } {
    return this.paymentProviderForm.controls as { [key: string]: AbstractControl };
  }
}
