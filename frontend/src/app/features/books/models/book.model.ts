import { Author } from '../../authors/models/author.model';
import { Genre } from '../../genres/models/genre.model';

export interface Book {
  id: string;
  title: string;
  subtitle?: string;
  description?: string;
  publishedDate?: Date;
  isbn?: string;
  pageCount?: number;
  publisher?: string;
  authorId: string;
  author?: Author;
  genreId: string;
  genre?: Genre;
}
