import { Component, input } from '@angular/core';
import { Retailer } from '../../../../core/models/retailer.model';
import { LucideAngularModule, Store, ExternalLink } from 'lucide-angular';

@Component({
  selector: 'app-retailer-card',
  imports: [LucideAngularModule],
  template: `
    <a class="retailer-card" [href]="'https://' + retailer().url" target="_blank" rel="noopener">
      <div class="icon-wrapper">
        <lucide-icon [img]="StoreIcon" [size]="22" class="store-icon"></lucide-icon>
      </div>
      <div class="content">
        <span class="name">{{ retailer().name }}</span>
        <span class="url">{{ retailer().url }}</span>
      </div>
      <lucide-icon [img]="ExternalLinkIcon" [size]="18" class="link-icon"></lucide-icon>
    </a>
  `,
  styles: `
    .retailer-card {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 20px 24px;
      background: var(--bg-surface);
      border-radius: 20px;
      text-decoration: none;
      transition: background-color 0.2s;
      cursor: pointer;
      width: 100%;

      &:hover {
        background: var(--bg-elevated);
      }
    }
    .icon-wrapper {
      width: 44px;
      height: 44px;
      min-width: 44px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
      background: linear-gradient(135deg, var(--accent-primary), #8B7845);
    }
    .store-icon {
      color: #1A1A1C;
    }
    .content {
      display: flex;
      flex-direction: column;
      gap: 4px;
      flex: 1;
      min-width: 0;
    }
    .name {
      font-family: var(--font-heading);
      font-size: 18px;
      font-weight: 500;
      color: var(--text-primary);
    }
    .url {
      font-family: var(--font-body);
      font-size: 12px;
      color: var(--text-secondary);
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
    .link-icon {
      color: var(--text-tertiary);
      flex-shrink: 0;
    }
  `,
})
export class RetailerCardComponent {
  readonly retailer = input.required<Retailer>();
  readonly StoreIcon = Store;
  readonly ExternalLinkIcon = ExternalLink;
}
