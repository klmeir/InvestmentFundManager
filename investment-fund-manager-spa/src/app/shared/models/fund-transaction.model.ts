export interface FundTransaction {
  id: string;
  user: string;
  fundId: string;
  fund: string;
  amount: number;
  type: 'SUBSCRIPTION' | 'CANCELLATION';
  status: 'COMPLETED' | 'CANCELED' | string;
  date: string;
  notificationChannel: 'EMAIL' | 'SMS' | string;
  recipient: string;
  category?: string | null;
  minimumAmount?: number | null;
  balanceAfterTransaction?: number;
}
