import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { GenreFormComponent } from '../genre-form/genre-form.component';
import { Genre } from '../../models/genre.model';

@Component({
  selector: 'app-genre-modal',
  standalone: true,
  imports: [CommonModule, ModalComponent, GenreFormComponent],
  templateUrl: './genre-modal.component.html',
  styleUrl: './genre-modal.component.scss',
})
export class GenreModalComponent {
  @Input() isOpen: boolean = false;
  @Input() genre?: Genre;
  @Input() saving: boolean = false;

  @Output() modalClose = new EventEmitter<void>();
  @Output() genreSave = new EventEmitter<Genre>();

  get modalTitle(): string {
    return this.genre ? 'Editar Gênero' : 'Novo Gênero';
  }

  onClose(): void {
    this.modalClose.emit();
  }

  onGenreSave(genre: Genre): void {
    this.genreSave.emit(genre);
  }
}
