import { Component, input } from '@angular/core';

@Component({
  selector: 'app-stat-card',
  template: `
    <div class="stat-card">
      <span class="value" [style.color]="valueColor()">{{ value() }}</span>
      <div class="label-row">
        <span class="label">{{ label() }}</span>
        @if (badge()) {
          <span class="badge">{{ badge() }}</span>
        }
      </div>
    </div>
  `,
  styles: `
    .stat-card {
      background: var(--bg-surface);
      border-radius: 20px;
      padding: 24px;
      display: flex;
      flex-direction: column;
      gap: 12px;
      flex: 1;
      min-width: 0;
    }
    .value {
      font-family: var(--font-heading);
      font-size: 48px;
      font-weight: 300;
      line-height: 0.85;
      color: var(--text-primary);
    }
    .label-row {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
    .label {
      font-family: var(--font-body);
      font-size: 12px;
      color: var(--text-secondary);
    }
    .badge {
      background: rgba(201, 169, 98, 0.125);
      color: var(--accent-primary);
      font-family: var(--font-body);
      font-size: 10px;
      font-weight: 500;
      padding: 4px 10px;
      border-radius: 20px;
    }

    @media (min-width: 1440px) {
      .value {
        font-size: 52px;
      }
      .label {
        font-size: 13px;
      }
      .stat-card {
        padding: 24px 28px;
      }
    }
  `,
})
export class StatCardComponent {
  readonly value = input.required<string | number>();
  readonly label = input.required<string>();
  readonly valueColor = input<string>('var(--text-primary)');
  readonly badge = input<string>('');
}
