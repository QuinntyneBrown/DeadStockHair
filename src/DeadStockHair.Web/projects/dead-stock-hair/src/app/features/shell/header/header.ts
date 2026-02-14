import { Component, inject } from '@angular/core';
import { LayoutService } from '../../../core/services/layout.service';
import { SearchBarComponent } from '../../home/components/search-bar/search-bar';
import { LucideAngularModule, Bell } from 'lucide-angular';

@Component({
  selector: 'app-header',
  imports: [LucideAngularModule, SearchBarComponent],
  template: `
    @if (layout.isDesktop()) {
      <!-- Desktop header: title + search + bell -->
      <div class="header desktop">
        <div class="header-left">
          <h1 class="title">Retailer Directory</h1>
          <p class="subtitle">Discover online retailers selling dead stock hair</p>
        </div>
        <div class="header-right">
          <div class="search-wrapper">
            <app-search-bar />
          </div>
          <button class="notification-btn">
            <lucide-icon [img]="BellIcon" [size]="20"></lucide-icon>
          </button>
        </div>
      </div>
    } @else {
      <!-- Mobile/Tablet header: brand + bell -->
      <div class="header mobile">
        <div class="brand">
          <h1 class="brand-text">DeadStock</h1>
          <span class="brand-subtitle">Hair Retailers</span>
        </div>
        <button class="notification-btn">
          <lucide-icon [img]="BellIcon" [size]="20"></lucide-icon>
        </button>
      </div>
    }
  `,
  styles: `
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }

    // Mobile/Tablet header
    .brand {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }
    .brand-text {
      font-family: var(--font-heading);
      font-size: 36px;
      font-weight: 300;
      line-height: 1;
      color: var(--text-primary);
    }
    .brand-subtitle {
      font-family: var(--font-body);
      font-size: 13px;
      font-weight: 500;
      letter-spacing: 3px;
      color: var(--accent-primary);
    }

    .notification-btn {
      width: 44px;
      height: 44px;
      border-radius: 22px;
      border: 1px solid var(--border-primary);
      background: transparent;
      color: var(--text-primary);
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      transition: border-color 0.2s;

      &:hover {
        border-color: var(--accent-primary);
      }
    }

    // Desktop header
    .header.desktop {
      .header-left {
        display: flex;
        flex-direction: column;
        gap: 6px;
      }
      .title {
        font-family: var(--font-heading);
        font-size: 36px;
        font-weight: 300;
        line-height: 1;
        color: var(--text-primary);
      }
      .subtitle {
        font-family: var(--font-body);
        font-size: 14px;
        color: var(--text-secondary);
      }
      .header-right {
        display: flex;
        align-items: center;
        gap: 12px;
      }
      .search-wrapper {
        width: 320px;
      }
      .notification-btn {
        width: 48px;
        height: 48px;
        border-radius: 24px;
      }
    }

    @media (min-width: 768px) {
      .brand-text {
        font-size: 42px;
      }
      .brand-subtitle {
        font-size: 14px;
      }
    }
  `,
})
export class HeaderComponent {
  readonly layout = inject(LayoutService);
  readonly BellIcon = Bell;
}
