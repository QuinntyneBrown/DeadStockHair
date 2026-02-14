import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Retailer, RetailerStats, RetailerStatus } from '../models/retailer.model';

const API_BASE = 'http://localhost:5128/api';

@Injectable({ providedIn: 'root' })
export class RetailerService {
  private readonly http = inject(HttpClient);

  private readonly retailers = signal<Retailer[]>([]);
  private readonly stats = signal<RetailerStats>({ totalRetailers: 0, inStock: 0, newThisWeek: 0 });
  private readonly loading = signal(true);

  readonly allRetailers = this.retailers.asReadonly();
  readonly isLoading = this.loading.asReadonly();

  readonly totalCount = computed(() => this.stats().totalRetailers);
  readonly inStockCount = computed(() => this.stats().inStock);
  readonly newThisWeek = computed(() => this.stats().newThisWeek);
  readonly lastScannedHours = signal(24);

  constructor() {
    this.loadRetailers();
    this.loadStats();
  }

  private loadRetailers(): void {
    this.http.get<Retailer[]>(`${API_BASE}/retailers`).subscribe({
      next: (data) => {
        this.retailers.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Failed to load retailers:', err);
        this.loading.set(false);
      },
    });
  }

  private loadStats(): void {
    this.http.get<RetailerStats>(`${API_BASE}/retailers/stats`).subscribe({
      next: (data) => this.stats.set(data),
      error: (err) => console.error('Failed to load stats:', err),
    });
  }

  filteredRetailers(query: string): Retailer[] {
    const q = query.toLowerCase();
    if (!q) return this.retailers();
    return this.retailers().filter(
      r => r.name.toLowerCase().includes(q) || r.url.toLowerCase().includes(q)
    );
  }
}
