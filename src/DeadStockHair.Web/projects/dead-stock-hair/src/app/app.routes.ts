import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: 'home',
    loadComponent: () => import('./features/home/home').then(m => m.HomeComponent),
  },
  {
    path: 'scan',
    loadComponent: () => import('./features/scan/scan').then(m => m.ScanComponent),
  },
  {
    path: 'saved',
    loadComponent: () => import('./features/saved/saved').then(m => m.SavedComponent),
  },
  {
    path: 'settings',
    loadComponent: () => import('./features/settings/settings').then(m => m.SettingsComponent),
  },
];
