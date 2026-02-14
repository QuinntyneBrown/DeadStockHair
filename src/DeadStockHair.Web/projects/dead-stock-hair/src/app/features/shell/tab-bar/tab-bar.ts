import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LayoutService } from '../../../core/services/layout.service';
import { LucideAngularModule, House, Search, Bookmark, Settings } from 'lucide-angular';

@Component({
  selector: 'app-tab-bar',
  imports: [RouterLink, RouterLinkActive, LucideAngularModule],
  template: `
    <div class="tab-bar-section">
      <nav class="tab-bar-pill">
        @for (tab of tabs; track tab.route) {
          <a
            class="tab"
            [routerLink]="tab.route"
            routerLinkActive="active"
          >
            <lucide-icon [img]="tab.icon" [size]="22"></lucide-icon>
            <span class="tab-label">{{ tab.label }}</span>
          </a>
        }
      </nav>
    </div>
  `,
  styles: `
    .tab-bar-section {
      padding: 16px 21px 24px;
      width: 100%;
      background: linear-gradient(to bottom, transparent 0%, #1A1A1C 30%);
    }
    .tab-bar-pill {
      display: flex;
      justify-content: space-around;
      align-items: center;
      height: 62px;
      border-radius: 34px;
      background: var(--bg-surface);
      border: 1px solid var(--border-primary);
      padding: 4px;
    }
    .tab {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      gap: 4px;
      flex: 1;
      height: 100%;
      text-decoration: none;
      color: var(--text-secondary);
      transition: color 0.2s;

      &.active {
        color: var(--accent-primary);
        .tab-label {
          font-weight: 500;
        }
      }
    }
    .tab-label {
      font-family: var(--font-body);
      font-size: 10px;
    }

    @media (min-width: 768px) {
      .tab-bar-section {
        padding: 16px 48px 24px;
      }
      .tab {
        flex-direction: row;
        gap: 8px;
      }
      .tab-label {
        font-size: 12px;
      }
    }
  `,
})
export class TabBarComponent {
  readonly layout = inject(LayoutService);

  readonly tabs = [
    { route: '/home', label: 'Home', icon: House },
    { route: '/scan', label: 'Scan', icon: Search },
    { route: '/saved', label: 'Saved', icon: Bookmark },
    { route: '/settings', label: 'Settings', icon: Settings },
  ];
}
