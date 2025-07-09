import { Routes } from '@angular/router';
import { PaymentProviderListComponent } from './components/payment-provider/payment-provider-list/payment-provider-list.component';
import { SimulateTransactionComponent } from './components/simulate-transaction/simulate-transaction.component';
import { PaymentProviderCreateFormComponent } from './components/payment-provider/payment-provider-create-form/payment-provider-create-form.component';
import { PaymentProviderEditFormComponent } from './components/payment-provider/payment-provider-edit-form/payment-provider-edit-form.component';
import { PaymentProviderDetailViewComponent } from './components/payment-provider/payment-provider-detail-view/payment-provider-detail-view.component';

export const routes: Routes = [
  { path: 'providers', component: PaymentProviderListComponent },
  { path: 'providers/create', component: PaymentProviderCreateFormComponent },
  { path: 'providers/edit/:id', component: PaymentProviderEditFormComponent },
  { path: 'providers/:id', component: PaymentProviderDetailViewComponent },
  { path: 'simulate', component: SimulateTransactionComponent },
  { path: '', redirectTo: 'providers', pathMatch: 'full' },
];
