import { Injectable, Signal, computed, signal } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LayoutService {
  private readonly tabletBreakpoint = '(min-width: 768px)';
  private readonly desktopBreakpoint = '(min-width: 1440px)';

  private readonly isTabletUp: Signal<boolean>;
  private readonly isDesktopUp: Signal<boolean>;

  readonly isMobile: Signal<boolean>;
  readonly isTablet: Signal<boolean>;
  readonly isDesktop: Signal<boolean>;

  constructor(private breakpointObserver: BreakpointObserver) {
    this.isTabletUp = toSignal(
      this.breakpointObserver
        .observe(this.tabletBreakpoint)
        .pipe(map(result => result.matches)),
      { initialValue: false }
    );

    this.isDesktopUp = toSignal(
      this.breakpointObserver
        .observe(this.desktopBreakpoint)
        .pipe(map(result => result.matches)),
      { initialValue: false }
    );

    this.isDesktop = this.isDesktopUp;
    this.isTablet = computed(() => this.isTabletUp() && !this.isDesktopUp());
    this.isMobile = computed(() => !this.isTabletUp());
  }
}
