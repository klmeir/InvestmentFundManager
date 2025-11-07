import { TestBed } from '@angular/core/testing';

import { InvestmentFundApi } from './investment-fund-service';

describe('InvestmentFundApi', () => {
  let service: InvestmentFundApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InvestmentFundApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
