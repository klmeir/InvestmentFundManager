import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FundManagerComponent } from './features/fund-manager/fund-manager.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FundManagerComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('InvestmentFundManager');
}
