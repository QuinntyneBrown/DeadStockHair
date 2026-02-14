import { Component, inject } from '@angular/core';
import { RetailerService } from '../../../../core/services/retailer.service';
import { LayoutService } from '../../../../core/services/layout.service';
import { StatCardComponent } from '../stat-card/stat-card';

@Component({
  selector: 'app-stats-row',
  imports: [StatCardComponent],
  template: `
    <div class="stats-row">
      <app-stat-card
        [value]="retailerService.totalCount()"
        [label]="layout.isDesktop() || layout.isTablet() ? 'Retailers Found' : 'Retailers'"
      />
      <app-stat-card
        [value]="retailerService.inStockCount()"
        label="In Stock"
        valueColor="var(--accent-primary)"
        badge="Live"
      />
      @if (layout.isTablet() || layout.isDesktop()) {
        <app-stat-card
          [value]="retailerService.newThisWeek()"
          label="New This Week"
          valueColor="var(--accent-secondary)"
        />
      }
      @if (layout.isDesktop()) {
        <app-stat-card
          [value]="retailerService.lastScannedHours()"
          label="Last Scanned (hrs)"
        />
      }
    </div>
  `,
  styles: `
    .stats-row {
      display: flex;
      gap: 16px;
      width: 100%;
    }

    @media (min-width: 1440px) {
      .stats-row {
        gap: 20px;
      }
    }
  `,
})
export class StatsRowComponent {
  readonly retailerService = inject(RetailerService);
  readonly layout = inject(LayoutService);
}
