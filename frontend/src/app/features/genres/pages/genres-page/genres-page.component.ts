import { CommonModule } from '@angular/common';
import {
  Component,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { MainLayoutComponent } from '../../../../layout/main-layout/main-layout.component';
import { NavbarComponent } from '../../../../layout/navbar/navbar.component';
import { SidebarComponent } from '../../../../layout/sidebar/sidebar.component';
import {
  CardAction,
  CardComponent,
} from '../../../../shared/components/card/card.component';
import {
  PagedResult,
  PaginationComponent,
} from '../../../../shared/components/pagination/pagination.component';
import { TagComponent } from '../../../../shared/components/tag/tag.component';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { GenreModalComponent } from '../../components/genre-modal/genre-modal.component';
import { GenreService } from '../../services/genre.service';
import { Genre } from '../../models/genre.model';
import { PagedParameters } from '../../../../shared/interfaces/pagination/paged-parameters.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-genres-page',
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
    GenreModalComponent,
  ],
  templateUrl: './genres-page.component.html',
  styleUrl: './genres-page.component.scss',
})
export class GenresPageComponent implements OnInit, OnDestroy {
  @ViewChild('genreCardContent', { static: true })
  genreCardContent!: TemplateRef<any>;

  genres: PagedResult<Genre> = {
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

  isGenreModalOpen = false;
  selectedGenre?: Genre;
  savingGenre = false;

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(private genreService: GenreService, private router: Router) {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntil(this.destroy$))
      .subscribe((searchTerm) => {
        this.loadGenres(1);
      });
  }

  ngOnInit(): void {
    this.loadGenres();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadGenres(page: number = 1): void {
    this.loading = true;
    this.error = null;

    const parameters: PagedParameters = {
      pageNumber: page,
      pageSize: this.genres.pageSize,
    };

    const serviceCall = this.searchTerm
      ? this.genreService.searchByName(this.searchTerm, parameters)
      : this.genreService.getAllWithBooks(parameters);

    serviceCall.subscribe({
      next: (result) => {
        this.genres = result;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading genres:', error);
        this.error = 'Erro ao carregar gêneros. Tente novamente.';
        this.loading = false;
      },
    });
  }

  onSearchGenres(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value;
    this.searchSubject.next(this.searchTerm);
  }

  onPageChanged(page: number): void {
    this.loadGenres(page);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.loadGenres(1);
  }

  trackByGenreId(index: number, genre: Genre): string {
    return genre.id;
  }

  trackByBookTitle(index: number, book: any): string {
    return book.id || book.title;
  }

  getGenreActions(genre: Genre): CardAction[] {
    const actions: CardAction[] = [
      {
        label: 'Editar',
        icon: 'fas fa-edit',
        cssClass: 'btn-secondary',
        action: () => this.editGenre(genre),
      },
      {
        label: 'Excluir',
        icon: 'fas fa-trash',
        cssClass: 'btn-danger',
        action: () => this.deleteGenre(genre),
      },
    ];
    return actions;
  }

  openGenreModal(genre?: Genre): void {
    this.selectedGenre = genre;
    this.isGenreModalOpen = true;
  }

  onGenreModalClose(): void {
    this.isGenreModalOpen = false;
    this.selectedGenre = undefined;
    this.savingGenre = false;
  }

  onGenreSave(genreData: Genre): void {
    this.savingGenre = true;

    if (genreData.id) {
      this.genreService.update(genreData.id, genreData).subscribe({
        next: () => {
          console.log('Genre updated:', genreData);
          this.savingGenre = false;
          this.isGenreModalOpen = false;
          this.selectedGenre = undefined;
          this.loadGenres(this.genres.page);
        },
        error: (error) => {
          console.error('Error updating genre:', error);
          this.error = 'Erro ao atualizar gênero. Tente novamente.';
          this.savingGenre = false;
        },
      });
    } else {
      this.genreService.create(genreData).subscribe({
        next: (newGenre: Genre) => {
          console.log('Genre created:', newGenre);
          this.savingGenre = false;
          this.isGenreModalOpen = false;
          this.selectedGenre = undefined;
          this.loadGenres(this.genres.page);
        },
        error: (error) => {
          console.error('Error creating genre:', error);
          this.error = 'Erro ao criar gênero. Tente novamente.';
          this.savingGenre = false;
        },
      });
    }
  }

  editGenre(genre: Genre): void {
    this.openGenreModal(genre);
  }

  deleteGenre(genre: Genre): void {
    if (genre.books && genre.books.length > 0) {
      alert(
        'Não é possível excluir este gênero pois existem livros associados a ele.'
      );
      return;
    }

    if (confirm(`Tem certeza que deseja excluir o gênero "${genre.name}"?`)) {
      this.genreService.delete(genre.id).subscribe({
        next: () => {
          console.log('Genre deleted:', genre.name);
          this.loadGenres(this.genres.page);
        },
        error: (error) => {
          console.error('Error deleting genre:', error);
          this.error = 'Erro ao excluir gênero. Tente novamente.';
        },
      });
    }
  }
}
