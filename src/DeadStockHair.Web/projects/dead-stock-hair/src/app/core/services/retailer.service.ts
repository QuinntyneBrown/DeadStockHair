import { Injectable, computed, signal } from '@angular/core';
import { Retailer } from '../models/retailer.model';

@Injectable({ providedIn: 'root' })
export class RetailerService {
  private readonly retailers = signal<Retailer[]>([
    { id: 1, name: 'DROP DEAD Extensions', url: 'dropdeadextensions.com', inStock: true, isNew: false },
    { id: 2, name: 'Hair Overstock', url: 'hairoverstock.com', inStock: true, isNew: false },
    { id: 3, name: 'Viola Hair Extensions', url: 'violahairextensions.co', inStock: true, isNew: true },
    { id: 4, name: 'PureHair Canada', url: 'purehair.ca', inStock: true, isNew: false },
    { id: 5, name: 'Export Hair Africa', url: 'exporthairafrica.com', inStock: false, isNew: false },
    { id: 6, name: 'Golden Lush Extensions', url: 'goldenlushextensions.com', inStock: true, isNew: true },
    { id: 7, name: 'Bombay Hair', url: 'bombayhair.com', inStock: false, isNew: false },
    { id: 8, name: 'Modiva Hair', url: 'modivahair.com', inStock: true, isNew: true },
    { id: 9, name: 'Chiquel Hair', url: 'chiquel.ca', inStock: false, isNew: false },
    { id: 10, name: 'Chiquel Hair', url: 'chiquelhair.com', inStock: false, isNew: false },
  ]);

  readonly allRetailers = this.retailers.asReadonly();

  readonly totalCount = computed(() => this.retailers().length);
  readonly inStockCount = computed(() => this.retailers().filter(r => r.inStock).length);
  readonly newThisWeek = computed(() => this.retailers().filter(r => r.isNew).length);
  readonly lastScannedHours = signal(24);

  filteredRetailers(query: string): Retailer[] {
    const q = query.toLowerCase();
    if (!q) return this.retailers();
    return this.retailers().filter(
      r => r.name.toLowerCase().includes(q) || r.url.toLowerCase().includes(q)
    );
  }
}
