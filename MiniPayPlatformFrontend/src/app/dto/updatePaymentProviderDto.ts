export class UpdatePaymentProviderDto {
  id: number;
  name: string;
  url: string;
  isActive: boolean;
  currency: string;
  description: string;

  constructor(
    id: number,
    name: string,
    url: string,
    isActive: boolean,
    currency: string,
    description: string,
  ) {
    this.id = id;
    this.name = name;
    this.url = url;
    this.isActive = isActive;
    this.currency = currency;
    this.description = description;
  }
}
