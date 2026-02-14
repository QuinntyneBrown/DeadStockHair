import { Component } from '@angular/core';
import { LucideAngularModule, Signal, Wifi } from 'lucide-angular';

@Component({
  selector: 'app-status-bar',
  imports: [LucideAngularModule],
  template: `
    <div class="status-bar">
      <span class="time">9:41</span>
      <div class="icons">
        <lucide-icon [img]="SignalIcon" [size]="16"></lucide-icon>
        <lucide-icon [img]="WifiIcon" [size]="16"></lucide-icon>
        <div class="battery">
          <div class="battery-fill"></div>
        </div>
      </div>
    </div>
  `,
  styles: `
    .status-bar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      height: 54px;
      padding: 14px 24px;
      width: 100%;
    }
    .time {
      font-family: var(--font-body);
      font-size: 16px;
      font-weight: 600;
      color: var(--text-primary);
    }
    .icons {
      display: flex;
      align-items: center;
      gap: 6px;
      color: var(--text-primary);
    }
    .battery {
      width: 27px;
      height: 13px;
      border-radius: 3px;
      border: 1px solid var(--text-primary);
      padding: 2px;
      display: flex;
      align-items: center;
    }
    .battery-fill {
      width: 16px;
      height: 100%;
      background: var(--text-primary);
      border-radius: 1px;
    }

    @media (min-width: 768px) {
      .status-bar {
        padding: 14px 32px;
      }
    }
  `,
})
export class StatusBarComponent {
  readonly SignalIcon = Signal;
  readonly WifiIcon = Wifi;
}
