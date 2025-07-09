export class TransactionRequestDto {
  paymentProviderId: number;
  amount: number;
  description: string;
  referenceId: string;

  constructor(
    paymentProviderId: number,
    amount: number,
    description: string,
    referenceId: string
  ) {
    this.paymentProviderId = paymentProviderId;
    this.amount = amount;
    this.description = description;
    this.referenceId = referenceId;
  }
}
