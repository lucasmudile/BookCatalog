import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
} from '@angular/core';
import {
  AbstractControl,
  FormGroup,
  FormBuilder,
  ValidationErrors,
  ValidatorFn,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  FormSelectComponent,
  SelectOption,
} from '../../../../shared/components/form-select/form-select.component';
import { CommonModule } from '@angular/common';
import { FormInputComponent } from '../../../../shared/components/form-input/form-input.component';
import { FormTextareaComponent } from '../../../../shared/components/form-textarea/form-textarea.component';
import { Book } from '../../models/book.model';
import { Author } from '../../../authors/models/author.model';
import { Genre } from '../../../genres/models/genre.model';

@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormInputComponent,
    FormTextareaComponent,
    FormSelectComponent,
  ],
  templateUrl: './book-form.component.html',
  styleUrl: './book-form.component.scss',
})
export class BookFormComponent implements OnInit, OnChanges {
  @Input() book?: Book;
  @Input() authors: Author[] = [];
  @Input() genres: Genre[] = [];
  @Input() saving: boolean = false;

  @Output() formSubmit = new EventEmitter<Book>();
  @Output() formCancel = new EventEmitter<void>();

  bookForm: FormGroup;
  authorOptions: SelectOption[] = [];
  genreOptions: SelectOption[] = [];

  constructor(private fb: FormBuilder) {
    this.bookForm = this.createForm();
  }

  ngOnInit(): void {
    this.updateOptions();
    if (this.book) {
      this.populateForm();
    }
  }

  ngOnChanges(): void {
    this.updateOptions();
    if (this.book && this.bookForm) {
      this.populateForm();
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      title: [
        '',
        [
          Validators.required,
          Validators.minLength(1),
          Validators.maxLength(200),
        ],
      ],
      subtitle: ['', [Validators.maxLength(300)]],
      description: ['', [Validators.maxLength(5000)]],
      authorId: ['', [Validators.required, this.guidValidator()]],
      genreId: ['', [Validators.required, this.guidValidator()]],
      isbn: ['', [this.isbnValidator()]],
      pageCount: ['', [this.pageCountValidator()]],
      publisher: ['', [this.publisherValidator()]],
      publishedDate: ['', [this.publishedDateValidator()]],
    });
  }

  private guidValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const guidPattern =
        /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
      const valid = guidPattern.test(control.value);

      return valid
        ? null
        : {
            invalidGuid: {
              message: 'Deve ser um ID válido.',
            },
          };
    };
  }

  private isbnValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const isbn = control.value.replace(/[-\s]/g, '');

      if (isbn.length === 10) {
        return this.validateISBN10(isbn)
          ? null
          : {
              invalidIsbn: {
                message: 'ISBN-10 inválido.',
              },
            };
      } else if (isbn.length === 13) {
        return this.validateISBN13(isbn)
          ? null
          : {
              invalidIsbn: {
                message: 'ISBN-13 inválido.',
              },
            };
      } else {
        return {
          invalidIsbn: {
            message: 'ISBN deve ter 10 ou 13 dígitos.',
          },
        };
      }
    };
  }

  private validateISBN10(isbn: string): boolean {
    if (!/^\d{9}[\dX]$/.test(isbn)) {
      return false;
    }

    let sum = 0;
    for (let i = 0; i < 9; i++) {
      sum += parseInt(isbn[i]) * (10 - i);
    }

    const checkDigit = isbn[9] === 'X' ? 10 : parseInt(isbn[9]);
    return (sum + checkDigit) % 11 === 0;
  }

  private validateISBN13(isbn: string): boolean {
    if (!/^\d{13}$/.test(isbn)) {
      return false;
    }

    let sum = 0;
    for (let i = 0; i < 12; i++) {
      sum += parseInt(isbn[i]) * (i % 2 === 0 ? 1 : 3);
    }

    const checkDigit = (10 - (sum % 10)) % 10;
    return checkDigit === parseInt(isbn[12]);
  }

  private pageCountValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const pageCount = parseInt(control.value);

      if (isNaN(pageCount)) {
        return {
          invalidNumber: {
            message: 'Deve ser um número válido.',
          },
        };
      }

      if (pageCount <= 0) {
        return {
          minValue: {
            message: 'O número de páginas deve ser maior que zero.',
          },
        };
      }

      if (pageCount >= 50000) {
        return {
          maxValue: {
            message: 'O número de páginas deve ser menor que 50.000.',
          },
        };
      }

      return null;
    };
  }

  private publisherValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      if (control.value.length < 1) {
        return {
          minLength: {
            message: 'O nome da editora deve ter pelo menos 1 caractere.',
          },
        };
      }

      if (control.value.length > 200) {
        return {
          maxLength: {
            message: 'O nome da editora não pode ter mais de 200 caracteres.',
          },
        };
      }

      return null;
    };
  }

  private publishedDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const publishedDate = new Date(control.value);
      const now = new Date();
      const minDate = new Date(1450, 0, 1);

      if (isNaN(publishedDate.getTime())) {
        return {
          invalidDate: {
            message: 'Data inválida.',
          },
        };
      }

      if (publishedDate > now) {
        return {
          futureDate: {
            message: 'A data de publicação não pode ser no futuro.',
          },
        };
      }

      if (publishedDate <= minDate) {
        return {
          tooOldDate: {
            message:
              'A data de publicação deve ser posterior ao ano 1450 (invenção da imprensa).',
          },
        };
      }

      return null;
    };
  }

  private updateOptions(): void {
    this.authorOptions = this.authors.map((author) => ({
      value: author.id,
      label: `${author.firstName} ${author.lastName}`,
    }));

    this.genreOptions = this.genres.map((genre) => ({
      value: genre.id,
      label: genre.name,
    }));
  }

  private populateForm(): void {
    if (this.book) {
      this.bookForm.patchValue({
        title: this.book.title,
        subtitle: this.book.subtitle || '',
        description: this.book.description || '',
        authorId: this.book.authorId,
        genreId: this.book.genreId,
        isbn: this.book.isbn || '',
        pageCount: this.book.pageCount || '',
        publisher: this.book.publisher || '',
        publishedDate: this.book.publishedDate || '',
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.bookForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldErrorMessage(fieldName: string): string {
    const field = this.bookForm.get(fieldName);

    if (!field || !field.errors || !field.touched) {
      return '';
    }

    const errors = field.errors;

    if (fieldName === 'title') {
      if (errors['required']) {
        return 'O título do livro é obrigatório.';
      }
      if (errors['minlength']) {
        return 'O título deve ter pelo menos 1 caractere.';
      }
      if (errors['maxlength']) {
        return 'O título não pode ter mais de 200 caracteres.';
      }
    }

    if (fieldName === 'subtitle') {
      if (errors['maxlength']) {
        return 'O subtítulo não pode ter mais de 300 caracteres.';
      }
    }

    if (fieldName === 'description') {
      if (errors['maxlength']) {
        return 'A descrição não pode ter mais de 5000 caracteres.';
      }
    }

    if (fieldName === 'authorId') {
      if (errors['required']) {
        return 'O ID do autor é obrigatório.';
      }
      if (errors['invalidGuid']) {
        return 'O ID do autor deve ser um GUID válido.';
      }
    }

    if (fieldName === 'genreId') {
      if (errors['required']) {
        return 'O ID do gênero é obrigatório.';
      }
      if (errors['invalidGuid']) {
        return 'O ID do gênero deve ser um GUID válido.';
      }
    }

    if (fieldName === 'isbn') {
      if (errors['invalidIsbn']) {
        return errors['invalidIsbn'].message;
      }
    }

    if (fieldName === 'pageCount') {
      if (errors['invalidNumber']) {
        return 'Deve ser um número válido.';
      }
      if (errors['minValue']) {
        return 'O número de páginas deve ser maior que zero.';
      }
      if (errors['maxValue']) {
        return 'O número de páginas deve ser menor que 50.000.';
      }
    }

    if (fieldName === 'publisher') {
      if (errors['minLength']) {
        return 'O nome da editora deve ter pelo menos 1 caractere.';
      }
      if (errors['maxLength']) {
        return 'O nome da editora não pode ter mais de 200 caracteres.';
      }
    }

    if (fieldName === 'publishedDate') {
      if (errors['invalidDate']) {
        return 'Data inválida.';
      }
      if (errors['futureDate']) {
        return 'A data de publicação não pode ser no futuro.';
      }
      if (errors['tooOldDate']) {
        return 'A data de publicação deve ser posterior ao ano 1450 (invenção da imprensa).';
      }
    }

    return 'Campo inválido.';
  }

  onSubmit(): void {
    if (this.bookForm.valid && !this.saving) {
      const formValue = this.bookForm.value;

      const bookData: Book = {
        ...formValue,
        id: this.book?.id,
        pageCount: formValue.pageCount
          ? parseInt(formValue.pageCount)
          : undefined,
      };

      Object.keys(bookData).forEach((key) => {
        if (
          bookData[key as keyof Book] === '' ||
          bookData[key as keyof Book] === null
        ) {
          delete bookData[key as keyof Book];
        }
      });

      this.formSubmit.emit(bookData);
    } else {
      this.markFormGroupTouched();
    }
  }

  onCancel(): void {
    this.bookForm.reset();
    this.formCancel.emit();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.bookForm.controls).forEach((key) => {
      const control = this.bookForm.get(key);
      control?.markAsTouched();
    });
  }

  resetForm(): void {
    this.bookForm.reset();
  }
}
