import { Component } from '@angular/core';

@Component({
  selector: 'app-scan',
  template: `
    <div class="placeholder-page">
      <h2>Scan</h2>
      <p>Scan retailers for dead stock hair products.</p>
    </div>
  `,
  styles: `
    .placeholder-page {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      gap: 16px;
      height: 100%;
      padding: 60px;

      h2 {
        font-family: var(--font-heading);
        font-size: 36px;
        font-weight: 300;
        color: var(--text-primary);
      }
      p {
        font-family: var(--font-body);
        font-size: 14px;
        color: var(--text-secondary);
      }
    }
  `,
})
export class ScanComponent {}
