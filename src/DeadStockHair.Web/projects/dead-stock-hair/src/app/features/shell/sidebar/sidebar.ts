import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LucideAngularModule, House, Search, Bookmark, Settings } from 'lucide-angular';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, LucideAngularModule],
  template: `
    <nav class="sidebar">
      <div class="logo">
        <h1 class="logo-text">DeadStock</h1>
        <span class="logo-subtitle">Hair Retailers</span>
      </div>

      <div class="nav-section">
        <span class="nav-label">MENU</span>
        <div class="nav-spacer"></div>
        @for (item of navItems; track item.route) {
          <a
            class="nav-item"
            [routerLink]="item.route"
            routerLinkActive="active"
          >
            <lucide-icon [img]="item.icon" [size]="20"></lucide-icon>
            <span>{{ item.label }}</span>
          </a>
        }
      </div>
    </nav>
  `,
  styles: `
    .sidebar {
      display: flex;
      flex-direction: column;
      gap: 40px;
      width: 260px;
      height: 100%;
      padding: 40px 28px;
      background: var(--bg-surface);
      border-right: 1px solid var(--border-divider);
    }
    .logo {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }
    .logo-text {
      font-family: var(--font-heading);
      font-size: 32px;
      font-weight: 300;
      line-height: 1;
      color: var(--text-primary);
    }
    .logo-subtitle {
      font-family: var(--font-body);
      font-size: 12px;
      font-weight: 500;
      letter-spacing: 3px;
      color: var(--accent-primary);
    }
    .nav-section {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }
    .nav-label {
      font-family: var(--font-body);
      font-size: 10px;
      font-weight: 500;
      letter-spacing: 3px;
      color: var(--text-tertiary);
    }
    .nav-spacer {
      height: 12px;
    }
    .nav-item {
      display: flex;
      align-items: center;
      gap: 14px;
      padding: 12px 16px;
      border-radius: 16px;
      text-decoration: none;
      color: var(--text-secondary);
      font-family: var(--font-body);
      font-size: 14px;
      transition: all 0.2s;
      cursor: pointer;

      &:hover {
        color: var(--text-primary);
        background: rgba(201, 169, 98, 0.05);
      }

      &.active {
        color: var(--accent-primary);
        background: rgba(201, 169, 98, 0.082);
        font-weight: 500;
      }
    }
  `,
})
export class SidebarComponent {
  readonly navItems = [
    { route: '/home', label: 'Home', icon: House },
    { route: '/scan', label: 'Scan', icon: Search },
    { route: '/saved', label: 'Saved', icon: Bookmark },
    { route: '/settings', label: 'Settings', icon: Settings },
  ];
}
