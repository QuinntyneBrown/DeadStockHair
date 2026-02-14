import { Component, inject, input } from '@angular/core';
import { Retailer } from '../../../../core/models/retailer.model';
import { LayoutService } from '../../../../core/services/layout.service';
import { RetailerCardComponent } from '../retailer-card/retailer-card';
import { LucideAngularModule, Search, Store } from 'lucide-angular';

@Component({
  selector: 'app-retailer-list',
  imports: [RetailerCardComponent, LucideAngularModule],
  template: `
    <div class="retailer-list-section">
      <div class="section-header">
        <span class="section-label">RETAILERS</span>
        <button class="scan-button">
          <lucide-icon [img]="SearchIcon" [size]="14" class="scan-icon"></lucide-icon>
          <span>Scan</span>
        </button>
      </div>

      @if (layout.isMobile()) {
        <!-- Mobile: stacked list with dividers -->
        <div class="card-list-mobile">
          @for (retailer of retailers(); track retailer.id; let last = $last) {
            <app-retailer-card [retailer]="retailer" />
            @if (!last) {
              <div class="divider"></div>
            }
          }
        </div>
      } @else {
        <!-- Tablet/Desktop: CSS grid of cards -->
        <div class="card-grid" [class.desktop]="layout.isDesktop()">
          @for (retailer of retailers(); track retailer.id) {
            <app-retailer-card [retailer]="retailer" />
          }
        </div>
      }
    </div>
  `,
  styles: `
    .retailer-list-section {
      display: flex;
      flex-direction: column;
      gap: 20px;
      width: 100%;
    }
    .section-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }
    .section-label {
      font-family: var(--font-body);
      font-size: 11px;
      font-weight: 500;
      letter-spacing: 3px;
      color: var(--text-secondary);
    }
    .scan-button {
      display: flex;
      align-items: center;
      gap: 6px;
      padding: 8px 16px;
      border-radius: 20px;
      border: 1px solid var(--accent-primary);
      background: transparent;
      color: var(--accent-primary);
      font-family: var(--font-body);
      font-size: 12px;
      font-weight: 500;
      cursor: pointer;
      transition: background-color 0.2s;

      &:hover {
        background: rgba(201, 169, 98, 0.1);
      }
    }
    .scan-icon {
      color: var(--accent-primary);
    }

    // Mobile stacked list
    .card-list-mobile {
      background: var(--bg-surface);
      border-radius: 20px;
      display: flex;
      flex-direction: column;
      overflow: hidden;

      app-retailer-card {
        ::ng-deep .retailer-card {
          border-radius: 0;
          background: transparent;

          &:hover {
            background: var(--bg-elevated);
          }
        }
      }
    }
    .divider {
      height: 1px;
      background: var(--border-divider);
      width: 100%;
    }

    // Grid layout for tablet/desktop
    .card-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 16px;
    }
    .card-grid.desktop {
      grid-template-columns: repeat(3, 1fr);
      gap: 20px;
    }

    @media (min-width: 1440px) {
      .scan-button {
        padding: 10px 20px;
      }
      .retailer-list-section {
        gap: 24px;
      }
    }
  `,
})
export class RetailerListComponent {
  readonly retailers = input.required<Retailer[]>();
  readonly layout = inject(LayoutService);
  readonly SearchIcon = Search;
  readonly StoreIcon = Store;
}
