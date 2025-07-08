import { Routes } from '@angular/router';
import { PaymentProviderListComponent } from './components/payment-provider/payment-provider-list/payment-provider-list.component';
import { SimulateTransactionComponent } from './components/simulate-transaction/simulate-transaction.component';

export const routes: Routes = [
  { path: 'providers', component: PaymentProviderListComponent },
  { path: 'simulate', component: SimulateTransactionComponent },
  { path: '', redirectTo: 'providers', pathMatch: 'full' },
];
