import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { BookFormComponent } from '../book-form/book-form.component';
import { Book } from '../../models/book.model';
import { Author } from '../../../authors/models/author.model';
import { Genre } from '../../../genres/models/genre.model';

@Component({
  selector: 'app-book-modal',
  standalone: true,
  imports: [CommonModule, ModalComponent, BookFormComponent],
  templateUrl: './book-modal.component.html',
  styleUrl: './book-modal.component.scss',
})
export class BookModalComponent {
  @Input() isOpen: boolean = false;
  @Input() book?: Book;
  @Input() authors: Author[] = [];
  @Input() genres: Genre[] = [];
  @Input() saving: boolean = false;

  @Output() modalClose = new EventEmitter<void>();
  @Output() bookSave = new EventEmitter<Book>();

  get modalTitle(): string {
    return this.book ? 'Editar Livro' : 'Novo Livro';
  }

  onClose(): void {
    this.modalClose.emit();
  }

  onBookSave(book: Book): void {
    this.bookSave.emit(book);
  }
}
