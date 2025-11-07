import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Fund } from '../models/fund.model';
import { FundTransaction } from '../models/fund-transaction.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class InvestmentFundApi {

  private apiUrl = `${environment.apiUrl}/Funds`;;

  constructor(private http: HttpClient) { }

  /**
   * Get all available funds
   */
  getFunds(): Observable<Fund[]> {
    return this.http.get<Fund[]>(`${this.apiUrl}/`);
  }

  /**
   * Subscribe user to a fund
   * @param fundId Fund identifier
   */
  subscribe(fundId: string, notificationChannel: 'EMAIL' | 'SMS' | string = ''): Observable<any> {
    return this.http.post(`${this.apiUrl}/subscribe`, { fundId, notificationChannel });
  }

  /**
   * Cancel an active subscription (or transaction)
   * @param fundId Foud identifier
   */
  cancel(fundId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/cancel`, { fundId });
  }

  /**
   * Get all fund transactions for the current user
   */
  getTransactions(): Observable<FundTransaction[]> {
    return this.http.get<FundTransaction[]>(`${this.apiUrl}/transactions`);
  }
}
