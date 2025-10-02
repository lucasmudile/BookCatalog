import {
  Component,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { NavbarComponent } from '../../../../layout/navbar/navbar.component';
import { SidebarComponent } from '../../../../layout/sidebar/sidebar.component';
import { MainLayoutComponent } from '../../../../layout/main-layout/main-layout.component';
import {
  CardAction,
  CardComponent,
} from '../../../../shared/components/card/card.component';
import {
  PagedResult,
  PaginationComponent,
} from '../../../../shared/components/pagination/pagination.component';
import { CommonModule } from '@angular/common';
import {
  Subject,
  debounceTime,
  distinctUntilChanged,
  takeUntil,
  forkJoin,
} from 'rxjs';
import { FormsModule } from '@angular/forms';
import { BookModalComponent } from '../../components/book-modal/book-modal.component';
import { BookService } from '../../services/book.service';
import { AuthorService } from '../../../authors/services/author.service';
import { GenreService } from '../../../genres/services/genre.service';
import { Book } from '../../models/book.model';
import { Author } from '../../../authors/models/author.model';
import { Genre } from '../../../genres/models/genre.model';
import { PagedParameters } from '../../../../shared/interfaces/pagination/paged-parameters.interface';

@Component({
  selector: 'app-books-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NavbarComponent,
    SidebarComponent,
    MainLayoutComponent,
    CardComponent,
    PaginationComponent,
    BookModalComponent,
  ],
  templateUrl: './books-page.component.html',
  styleUrl: './books-page.component.scss',
})
export class BooksPageComponent implements OnInit, OnDestroy {
  @ViewChild('bookCardContent', { static: true })
  bookCardContent!: TemplateRef<any>;

  books: PagedResult<Book> = {
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

  authors: Author[] = [];
  genres: Genre[] = [];

  loading = false;
  error: string | null = null;
  searchTerm = '';

  isBookModalOpen = false;
  selectedBook?: Book;
  savingBook = false;

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(
    private bookService: BookService,
    private authorService: AuthorService,
    private genreService: GenreService
  ) {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntil(this.destroy$))
      .subscribe((searchTerm) => {
        this.loadBooks(1);
      });
  }

  ngOnInit(): void {
    this.loadReferenceData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadReferenceData(): void {
    const authorsParams: PagedParameters = { pageNumber: 1, pageSize: 1000 };
    const genresParams: PagedParameters = { pageNumber: 1, pageSize: 1000 };

    forkJoin({
      authors: this.authorService.getAll(authorsParams),
      genres: this.genreService.getAll(genresParams),
    }).subscribe({
      next: (result) => {
        this.authors = result.authors.items;
        this.genres = result.genres.items;
        this.loadBooks();
      },
      error: () => {
        this.error = 'Erro ao carregar dados de referência. Tente novamente.';
      },
    });
  }

  loadBooks(page: number = 1): void {
    this.loading = true;
    this.error = null;

    const parameters: PagedParameters = {
      pageNumber: page,
      pageSize: this.books.pageSize,
    };

    const serviceCall = this.searchTerm
      ? this.bookService.searchByName(this.searchTerm, parameters)
      : this.bookService.getAll(parameters);

    serviceCall.subscribe({
      next: (result) => {
        this.books = result;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading books:', error);
        this.error = 'Erro ao carregar livros. Tente novamente.';
        this.loading = false;
      },
    });
  }

  onSearchBooks(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value;
    this.searchSubject.next(this.searchTerm);
  }

  onPageChanged(page: number): void {
    this.loadBooks(page);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.loadBooks(1);
  }

  trackByBookId(index: number, book: Book): string {
    return book.id;
  }

  getBookActions(book: Book): CardAction[] {
    return [
      {
        label: 'Editar',
        icon: 'fas fa-edit',
        cssClass: 'btn-secondary',
        action: () => this.editBook(book),
      },
      {
        label: 'Excluir',
        icon: 'fas fa-trash',
        cssClass: 'btn-danger',
        action: () => this.deleteBook(book),
      },
    ];
  }

  getAuthorName(authorId: string): string {
    const author = this.authors.find((a) => a.id === authorId);
    return author ? `${author.firstName} ${author.lastName}` : 'N/A';
  }

  getGenreName(genreId: string): string {
    const genre = this.genres.find((g) => g.id === genreId);
    return genre ? genre.name : 'N/A';
  }

  formatDate(date?: Date): string {
    if (!date) return '';
    return new Date(date).toLocaleDateString('pt-BR');
  }

  openBookModal(book?: Book): void {
    this.selectedBook = book;
    this.isBookModalOpen = true;
  }

  onBookModalClose(): void {
    this.isBookModalOpen = false;
    this.selectedBook = undefined;
    this.savingBook = false;
  }

  onBookSave(bookData: Book): void {
    this.savingBook = true;

    if (this.selectedBook && this.selectedBook.id) {
      this.bookService.update(this.selectedBook.id, bookData).subscribe({
        next: () => {
          console.log('Book updated:', bookData);
          this.savingBook = false;
          this.isBookModalOpen = false;
          this.selectedBook = undefined;
          this.loadBooks(this.books.page);
        },
        error: (error) => {
          console.error('Error updating book:', error);
          this.error = 'Erro ao atualizar livro. Tente novamente.';
          this.savingBook = false;
        },
      });
    } else {
      this.bookService.create(bookData).subscribe({
        next: (newBook: Book) => {
          console.log('Book created:', newBook);
          this.savingBook = false;
          this.isBookModalOpen = false;
          this.selectedBook = undefined;
          this.loadBooks(this.books.page);
        },
        error: (error) => {
          console.error('Error creating book:', error);
          this.error = 'Erro ao criar livro. Tente novamente.';
          this.savingBook = false;
        },
      });
    }
  }

  editBook(book: Book): void {
    this.openBookModal(book);
  }

  deleteBook(book: Book): void {
    if (confirm(`Tem certeza que deseja excluir o livro "${book.title}"?`)) {
      this.bookService.delete(book.id).subscribe({
        next: () => {
          console.log('Book deleted:', book.title);
          this.loadBooks(this.books.page);
        },
        error: (error) => {
          console.error('Error deleting book:', error);
          this.error = 'Erro ao excluir livro. Tente novamente.';
        },
      });
    }
  }
}
