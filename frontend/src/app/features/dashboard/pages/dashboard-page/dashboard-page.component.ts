import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavbarComponent } from '../../../../layout/navbar/navbar.component';
import { SidebarComponent } from '../../../../layout/sidebar/sidebar.component';
import { MainLayoutComponent } from '../../../../layout/main-layout/main-layout.component';
import { DashboardCardsComponent } from '../../components/dashboard-cards/dashboard-cards.component';
import { RecentActivityComponent } from '../../components/recent-activity/recent-activity.component';
import { BookService } from '../../../books/services/book.service';
import { AuthorService } from '../../../authors/services/author.service';
import { GenreService } from '../../../genres/services/genre.service';
import { PagedParameters } from '../../../../shared/interfaces/pagination/paged-parameters.interface';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';

export interface DashboardStats {
  totalBooks: number;
  totalAuthors: number;
  totalGenres: number;
  loading: boolean;
  error: string | null;
}

export interface RecentActivity {
  id: string;
  type: 'book' | 'author' | 'genre';
  action: 'created' | 'updated' | 'deleted';
  title: string;
  description: string;
  timestamp: Date;
  icon: string;
}

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
    SidebarComponent,
    MainLayoutComponent,
    DashboardCardsComponent,
    RecentActivityComponent,
  ],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss',
})
export class DashboardPageComponent implements OnInit, OnDestroy {
  dashboardStats: DashboardStats = {
    totalBooks: 0,
    totalAuthors: 0,
    totalGenres: 0,
    loading: true,
    error: null,
  };

  recentActivities: RecentActivity[] = [];

  private destroy$ = new Subject<void>();

  constructor(
    private bookService: BookService,
    private authorService: AuthorService,
    private genreService: GenreService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadDashboardData(): void {
    this.dashboardStats.loading = true;
    this.dashboardStats.error = null;

    const countParams: PagedParameters = { pageNumber: 1, pageSize: 1 };

    forkJoin({
      books: this.bookService.getAll(countParams),
      authors: this.authorService.getAll(countParams),
      genres: this.genreService.getAll(countParams),
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          this.dashboardStats = {
            totalBooks: result.books.totalCount,
            totalAuthors: result.authors.totalCount,
            totalGenres: result.genres.totalCount,
            loading: false,
            error: null,
          };

          this.generateRecentActivity(result);
        },
        error: (error) => {
          console.error('Error loading dashboard data:', error);
          this.dashboardStats = {
            ...this.dashboardStats,
            loading: false,
            error: 'Erro ao carregar dados do dashboard. Tente novamente.',
          };
        },
      });
  }

  private generateRecentActivity(data: any): void {
    const activities: RecentActivity[] = [];

    data.books.items.forEach((book: any, index: number) => {
      if (index < 3) {
        activities.push({
          id: `book-${book.id || book.title}`,
          type: 'book',
          action: 'created',
          title: book.title,
          description: `Livro "${book.title}" foi adicionado à biblioteca`,
          timestamp: book.publishedDate
            ? new Date(book.publishedDate)
            : new Date(),
          icon: 'fas fa-book',
        });
      }
    });

    data.authors.items.forEach((author: any, index: number) => {
      if (index < 2) {
        activities.push({
          id: `author-${author.id}`,
          type: 'author',
          action: 'created',
          title: `${author.firstName} ${author.lastName}`,
          description: `Autor "${author.firstName} ${author.lastName}" foi cadastrado`,
          timestamp: author.dateOfBirth
            ? new Date(author.dateOfBirth)
            : new Date(),
          icon: 'fas fa-user-edit',
        });
      }
    });

    data.genres.items.forEach((genre: any, index: number) => {
      if (index < 2) {
        activities.push({
          id: `genre-${genre.id}`,
          type: 'genre',
          action: 'created',
          title: genre.name,
          description: `Gênero "${genre.name}" foi criado`,
          timestamp: new Date(),
          icon: 'fas fa-tags',
        });
      }
    });

    this.recentActivities = activities
      .sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime())
      .slice(0, 10);
  }

  refreshDashboard(): void {
    this.loadDashboardData();
  }

  getDashboardStats(): DashboardStats {
    return this.dashboardStats;
  }

  getRecentActivities(): RecentActivity[] {
    return this.recentActivities;
  }
}
