import {
  Component,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import {
  CardAction,
  CardComponent,
} from '../../../../shared/components/card/card.component';
import {
  PagedResult,
  PaginationComponent,
} from '../../../../shared/components/pagination/pagination.component';
import { CommonModule } from '@angular/common';
import { MainLayoutComponent } from '../../../../layout/main-layout/main-layout.component';
import { NavbarComponent } from '../../../../layout/navbar/navbar.component';
import { SidebarComponent } from '../../../../layout/sidebar/sidebar.component';
import { TagComponent } from '../../../../shared/components/tag/tag.component';
import { AuthorModalComponent } from '../../components/author-modal/author-modal.component';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { AuthorService } from '../../services/author.service';
import { Author } from '../../models/author.model';
import { PagedParameters } from '../../../../shared/interfaces/pagination/paged-parameters.interface';

@Component({
  selector: 'app-authors-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NavbarComponent,
    SidebarComponent,
    MainLayoutComponent,
    CardComponent,
    PaginationComponent,
    TagComponent,
    AuthorModalComponent,
  ],
  templateUrl: './authors-page.component.html',
  styleUrl: './authors-page.component.scss',
})
export class AuthorsPageComponent implements OnInit, OnDestroy {
  @ViewChild('authorCardContent', { static: true })
  authorCardContent!: TemplateRef<any>;

  authors: PagedResult<Author> = {
    items: [],
    page: 1,
    pageSize: 6,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false,
    hasNextPage: false,
    firstItemIndex: 0,
    lastItemIndex: 0,
  };

  loading = false;
  error: string | null = null;
  searchTerm = '';

  isAuthorModalOpen = false;
  selectedAuthor?: Author;
  savingAuthor = false;

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(private authorService: AuthorService) {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntil(this.destroy$))
      .subscribe((searchTerm) => {
        this.loadAuthors(1);
      });
  }

  ngOnInit(): void {
    this.loadAuthors();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadAuthors(page: number = 1): void {
    this.loading = true;
    this.error = null;

    const parameters: PagedParameters = {
      pageNumber: page,
      pageSize: this.authors.pageSize,
    };

    const serviceCall = this.searchTerm
      ? this.authorService.searchByName(this.searchTerm, parameters)
      : this.authorService.getAllWithBooks(parameters);

    serviceCall.subscribe({
      next: (result) => {
        this.authors = result;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading authors:', error);
        this.error = 'Erro ao carregar autores. Tente novamente.';
        this.loading = false;
      },
    });
  }

  onSearchAuthors(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value;
    this.searchSubject.next(this.searchTerm);
  }

  onPageChanged(page: number): void {
    this.loadAuthors(page);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.loadAuthors(1);
  }

  trackByAuthorId(index: number, author: Author): string {
    return author.id;
  }

  trackByBookTitle(index: number, book: any): string {
    return book.id || book.title;
  }

  getAuthorActions(author: Author): CardAction[] {
    return [
      {
        label: 'Editar',
        icon: 'fas fa-edit',
        cssClass: 'btn-secondary',
        action: () => this.editAuthor(author),
      },
      {
        label: 'Excluir',
        icon: 'fas fa-trash',
        cssClass: 'btn-danger',
        action: () => this.deleteAuthor(author),
      },
    ];
  }

  formatDate(date?: Date): string {
    if (!date) return '';
    return new Date(date).toLocaleDateString('pt-BR');
  }

  openAuthorModal(author?: Author): void {
    this.selectedAuthor = author;
    this.isAuthorModalOpen = true;
  }

  onAuthorModalClose(): void {
    this.isAuthorModalOpen = false;
    this.selectedAuthor = undefined;
    this.savingAuthor = false;
  }

  onAuthorSave(authorData: Author): void {
    this.savingAuthor = true;

    if (authorData.id) {
      this.authorService.update(authorData.id, authorData).subscribe({
        next: () => {
          console.log('Author updated:', authorData);
          this.savingAuthor = false;
          this.isAuthorModalOpen = false;
          this.selectedAuthor = undefined;
          this.loadAuthors(this.authors.page);
        },
        error: (error) => {
          console.error('Error updating author:', error);
          this.error = 'Erro ao atualizar autor. Tente novamente.';
          this.savingAuthor = false;
        },
      });
    } else {
      this.authorService.create(authorData).subscribe({
        next: (newAuthor: Author) => {
          console.log('Author created:', newAuthor);
          this.savingAuthor = false;
          this.isAuthorModalOpen = false;
          this.selectedAuthor = undefined;
          this.loadAuthors(this.authors.page);
        },
        error: (error) => {
          console.error('Error creating author:', error);
          this.error = 'Erro ao criar autor. Tente novamente.';
          this.savingAuthor = false;
        },
      });
    }
  }

  editAuthor(author: Author): void {
    this.openAuthorModal(author);
  }

  deleteAuthor(author: Author): void {
    if (author.books && author.books.length > 0) {
      alert(
        'Não é possível excluir este autor pois existem livros associados a ele.'
      );
      return;
    }

    if (
      confirm(
        `Tem certeza que deseja excluir o autor "${author.firstName} ${author.lastName}"?`
      )
    ) {
      this.authorService.delete(author.id).subscribe({
        next: () => {
          console.log(
            'Author deleted:',
            author.firstName + ' ' + author.lastName
          );
          this.loadAuthors(this.authors.page);
        },
        error: (error) => {
          console.error('Error deleting author:', error);
          this.error = 'Erro ao excluir autor. Tente novamente.';
        },
      });
    }
  }
}
