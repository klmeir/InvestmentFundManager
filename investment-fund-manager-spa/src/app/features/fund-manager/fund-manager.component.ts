import { Component, OnInit } from '@angular/core';
import { InvestmentFundApi } from '../../shared/services/investment-fund-service';
import { Fund } from '../../shared/models/fund.model';
import { Observable } from 'rxjs';
import { FundTransaction } from '../../shared/models/fund-transaction.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-fund-manager',
  imports: [CommonModule, FormsModule],
  templateUrl: './fund-manager.component.html',
  styleUrl: './fund-manager.component.css',
})
export class FundManagerComponent implements OnInit {

  funds$!: Observable<Fund[]>;                     // Todos los fondos
  transactions$!: Observable<FundTransaction[]>;  // Todas las transacciones
  selectedFundId: string = '';
  notificationChannel: 'EMAIL' | 'SMS' | string = '';
  message: string = '';
  isLoading: boolean = false;

  constructor(private fundApi: InvestmentFundApi) { }

  ngOnInit(): void {
    this.funds$ = this.fundApi.getFunds();
    this.loadTransactions();
  }

  subscribe(): void {
    if (!this.selectedFundId) {
      this.message = 'Please select a fund to subscribe.';
      return;
    }

    if (!this.selectedFundId) {
      this.message = 'Please select a notification type to subscribe.';
      return;
    }

    this.fundApi.subscribe(this.selectedFundId, this.notificationChannel).subscribe({
      next: () => {
        this.message = '✅ Subscription successful!';
        this.loadTransactions();
      },
      error: err => {
        if (err.error && err.error.ErrorMessage) {
          this.message = `❌ Subscription failed: ${err.error.ErrorMessage}`;
        } else {
          this.message = `❌ Subscription failed: ${err.message || 'Unknown error'}`;
        }
        console.error('Subscription failed', err);
      }
    });
  }

  cancel(fundId: string): void {
    this.message = '';
    this.isLoading = true;
    this.fundApi.cancel(fundId).subscribe({
      next: () => {
        this.isLoading = false;
        this.message = `✅ Subscription for fund was successfully canceled.`;
        this.loadTransactions();
      },
      error: err => {
        if (err.error && err.error.ErrorMessage) {
          this.message = `❌ Cancel failed: ${err.error.ErrorMessage}`;
        } else {
          this.message = `❌ Cancel failed: ${err.message || 'Unknown error'}`;
        }
        console.error('Cancel failed', err);
      }
    });
  }

  loadTransactions(): void {
    this.transactions$ = this.fundApi.getTransactions(); // Endpoint que devuelve transacciones del usuario
  }
}
