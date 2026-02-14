import { Component, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LucideAngularModule, Search } from 'lucide-angular';

@Component({
  selector: 'app-search-bar',
  imports: [FormsModule, LucideAngularModule],
  template: `
    <div class="search-bar">
      <lucide-icon [img]="SearchIcon" [size]="18" class="search-icon"></lucide-icon>
      <input
        type="text"
        placeholder="Search retailers..."
        class="search-input"
        (input)="onSearch($event)"
      />
    </div>
  `,
  styles: `
    .search-bar {
      display: flex;
      align-items: center;
      gap: 14px;
      height: 52px;
      border-radius: 26px;
      border: 1px solid var(--border-primary);
      padding: 0 20px;
      width: 100%;
      background: transparent;
      transition: border-color 0.2s;

      &:focus-within {
        border-color: var(--accent-primary);
      }
    }
    .search-icon {
      color: var(--text-secondary);
      flex-shrink: 0;
    }
    .search-input {
      background: transparent;
      border: none;
      outline: none;
      color: var(--text-primary);
      font-family: var(--font-body);
      font-size: 14px;
      width: 100%;

      &::placeholder {
        color: var(--text-secondary);
      }
    }
  `,
})
export class SearchBarComponent {
  readonly searchChange = output<string>();
  readonly SearchIcon = Search;

  onSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchChange.emit(value);
  }
}
