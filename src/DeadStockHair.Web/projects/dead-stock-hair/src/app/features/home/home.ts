import { Component, inject, signal } from '@angular/core';
import { RetailerService } from '../../core/services/retailer.service';
import { LayoutService } from '../../core/services/layout.service';
import { SearchBarComponent } from './components/search-bar/search-bar';
import { StatsRowComponent } from './components/stats-row/stats-row';
import { RetailerListComponent } from './components/retailer-list/retailer-list';

@Component({
  selector: 'app-home',
  imports: [SearchBarComponent, StatsRowComponent, RetailerListComponent],
  template: `
    <div class="home-content">
      @if (!layout.isDesktop()) {
        <app-search-bar (searchChange)="onSearch($event)" />
      }
      <app-stats-row />
      <app-retailer-list [retailers]="filteredRetailers()" />
    </div>
  `,
  styles: `
    .home-content {
      display: flex;
      flex-direction: column;
      gap: 32px;
      width: 100%;
      padding: 0 28px 28px;
    }

    @media (min-width: 768px) {
      .home-content {
        gap: 40px;
        padding: 0 48px 48px;
      }
    }

    @media (min-width: 1440px) {
      .home-content {
        gap: 40px;
        padding: 0 60px 60px;
      }
    }
  `,
})
export class HomeComponent {
  readonly retailerService = inject(RetailerService);
  readonly layout = inject(LayoutService);

  private readonly searchQuery = signal('');

  readonly filteredRetailers = () => this.retailerService.filteredRetailers(this.searchQuery());

  onSearch(query: string): void {
    this.searchQuery.set(query);
  }
}
