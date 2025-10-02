import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  firstItemIndex: number;
  lastItemIndex: number;
}

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
})
export class PaginationComponent implements OnChanges {
  @Input() pagedData?: PagedResult<any>;
  @Output() pageChanged = new EventEmitter<number>();

  private maxVisiblePages = 5;

  ngOnChanges(): void {}

  changePage(page: number): void {
    if (
      this.pagedData &&
      page >= 1 &&
      page <= this.pagedData.totalPages &&
      page !== this.pagedData.page
    ) {
      this.pageChanged.emit(page);
    }
  }

  getPageNumbers(): number[] {
    if (!this.pagedData) return [];

    const totalPages = this.pagedData.totalPages;
    const currentPage = this.pagedData.page;
    const maxVisible = this.maxVisiblePages;

    if (totalPages <= maxVisible) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }

    const halfVisible = Math.floor(maxVisible / 2);
    let startPage = Math.max(1, currentPage - halfVisible);
    let endPage = Math.min(totalPages, startPage + maxVisible - 1);

    if (endPage - startPage + 1 < maxVisible) {
      startPage = Math.max(1, endPage - maxVisible + 1);
    }

    return Array.from(
      { length: endPage - startPage + 1 },
      (_, i) => startPage + i
    );
  }
}
