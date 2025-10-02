import { Book } from '../../books/models/book.model';

export interface Author {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth?: Date;
  dateOfDeath?: Date;
  biography?: string;
  books?: Book[];
}
