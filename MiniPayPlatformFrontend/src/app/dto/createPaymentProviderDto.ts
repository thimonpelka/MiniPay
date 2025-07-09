export class CreatePaymentProviderDto {
  name: string;
  url: string;
  isActive: boolean;
  currency: string;
  description: string;

  constructor(
    name: string,
    url: string,
    isActive: boolean,
    currency: string,
    description: string,
  ) {
    this.name = name;
    this.url = url;
    this.isActive = isActive;
    this.currency = currency;
    this.description = description;
  }
}
