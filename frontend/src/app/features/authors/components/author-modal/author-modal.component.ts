import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AuthorFormComponent } from '../author-form/author-form.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { Author } from '../../models/author.model';

@Component({
  selector: 'app-author-modal',
  standalone: true,
  imports: [ModalComponent, AuthorFormComponent],
  templateUrl: './author-modal.component.html',
  styleUrl: './author-modal.component.scss',
})
export class AuthorModalComponent {
  @Input() isOpen: boolean = false;
  @Input() author?: Author;
  @Input() saving: boolean = false;

  @Output() modalClose = new EventEmitter<void>();
  @Output() authorSave = new EventEmitter<Author>();

  get modalTitle(): string {
    return this.author ? 'Editar Autor' : 'Novo Autor';
  }

  onClose(): void {
    this.modalClose.emit();
  }

  onAuthorSave(author: Author): void {
    this.authorSave.emit(author);
  }
}
