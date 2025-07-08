export class PaymentProviderDto {
  id: number;
  name: string;
  url: string;
  isActive: boolean;
  currency: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;

  constructor(
    id: number,
    name: string,
    url: string,
    isActive: boolean,
    currency: string,
    description: string,
    createdAt: Date,
    updatedAt: Date
  ) {
    this.id = id;
    this.name = name;
    this.url = url;
    this.isActive = isActive;
    this.currency = currency;
    this.description = description;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
  }
}
