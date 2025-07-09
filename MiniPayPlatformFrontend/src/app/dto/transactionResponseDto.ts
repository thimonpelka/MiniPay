export class TransactionResponseDto {
  status: string;
  transactionId: string;
  timestamp: Date;
  message: string;
  referenceId: string;

  constructor(
    status: string,
    transactionId: string,
    timestamp: Date,
    message: string,
    referenceId: string
  ) {
    this.status = status;
    this.transactionId = transactionId;
    this.timestamp = timestamp;
    this.message = message;
    this.referenceId = referenceId;
  }
}
