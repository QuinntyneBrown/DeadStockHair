import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LayoutService } from './core/services/layout.service';
import { StatusBarComponent } from './shared/components/status-bar/status-bar';
import { HeaderComponent } from './features/shell/header/header';
import { SidebarComponent } from './features/shell/sidebar/sidebar';
import { TabBarComponent } from './features/shell/tab-bar/tab-bar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, StatusBarComponent, HeaderComponent, SidebarComponent, TabBarComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  readonly layout = inject(LayoutService);
}
