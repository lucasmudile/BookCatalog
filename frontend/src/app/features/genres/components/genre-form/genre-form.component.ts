import { CommonModule } from '@angular/common';
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
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { FormInputComponent } from '../../../../shared/components/form-input/form-input.component';
import { FormTextareaComponent } from '../../../../shared/components/form-textarea/form-textarea.component';
import { Genre } from '../../models/genre.model';

@Component({
  selector: 'app-genre-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormInputComponent,
    FormTextareaComponent,
  ],
  templateUrl: './genre-form.component.html',
  styleUrl: './genre-form.component.scss',
})
export class GenreFormComponent implements OnInit, OnChanges {
  @Input() genre?: Genre;
  @Input() saving: boolean = false;

  @Output() formSubmit = new EventEmitter<Genre>();
  @Output() formCancel = new EventEmitter<void>();

  genreForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.genreForm = this.createForm();
  }

  ngOnInit(): void {
    if (this.genre) {
      this.populateForm();
    }
  }

  ngOnChanges(): void {
    if (this.genre && this.genreForm) {
      this.populateForm();
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
        ],
      ],
      description: ['', [this.descriptionValidator()]],
    });
  }

  private descriptionValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      if (control.value.length < 10) {
        return {
          minLength: {
            message: 'A descrição deve ter pelo menos 10 caracteres.',
          },
        };
      }

      if (control.value.length > 1000) {
        return {
          maxLength: {
            message:
              'A descrição do gênero não pode ter mais de 1000 caracteres.',
          },
        };
      }

      return null;
    };
  }

  private populateForm(): void {
    if (this.genre) {
      this.genreForm.patchValue({
        name: this.genre.name,
        description: this.genre.description || '',
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.genreForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldErrorMessage(fieldName: string): string {
    const field = this.genreForm.get(fieldName);

    if (!field || !field.errors || !field.touched) {
      return '';
    }

    const errors = field.errors;

    if (fieldName === 'name') {
      if (errors['required']) {
        return 'O nome do gênero é obrigatório.';
      }
      if (errors['minlength']) {
        return 'O nome do gênero deve ter pelo menos 2 caracteres.';
      }
      if (errors['maxlength']) {
        return 'O nome do gênero não pode ter mais de 100 caracteres.';
      }
    }

    if (fieldName === 'description') {
      if (errors['minLength']) {
        return 'A descrição deve ter pelo menos 10 caracteres.';
      }
      if (errors['maxLength']) {
        return 'A descrição do gênero não pode ter mais de 1000 caracteres.';
      }
    }

    return 'Campo inválido.';
  }

  onSubmit(): void {
    if (this.genreForm.valid && !this.saving) {
      const formValue = this.genreForm.value;

      const genreData: Genre = {
        ...formValue,
        id: this.genre?.id,
      };

      Object.keys(genreData).forEach((key) => {
        if (
          genreData[key as keyof Genre] === '' ||
          genreData[key as keyof Genre] === null
        ) {
          delete genreData[key as keyof Genre];
        }
      });

      this.formSubmit.emit(genreData);
    } else {
      this.markFormGroupTouched();
    }
  }

  onCancel(): void {
    this.genreForm.reset();
    this.formCancel.emit();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.genreForm.controls).forEach((key) => {
      const control = this.genreForm.get(key);
      control?.markAsTouched();
    });
  }

  resetForm(): void {
    this.genreForm.reset();
  }
}
