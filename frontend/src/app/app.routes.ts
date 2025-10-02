import { Routes } from '@angular/router';
import { DashboardPageComponent } from './features/dashboard/pages/dashboard-page/dashboard-page.component';
import { BooksPageComponent } from './features/books/pages/books-page/books-page.component';
import { AuthorsPageComponent } from './features/authors/pages/authors-page/authors-page.component';
import { GenresPageComponent } from './features/genres/pages/genres-page/genres-page.component';

export const routes: Routes = [
  {
    path: '',
    component: DashboardPageComponent,
  },
  {
    path: 'books',
    component: BooksPageComponent,
  },
  {
    path: 'authors',
    component: AuthorsPageComponent,
  },
  {
    path: 'genres',
    component: GenresPageComponent,
  },
];
