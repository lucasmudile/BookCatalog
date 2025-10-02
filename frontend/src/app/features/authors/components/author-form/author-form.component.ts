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
import { Author } from '../../models/author.model';

@Component({
  selector: 'app-author-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormInputComponent,
    FormTextareaComponent,
  ],
  templateUrl: './author-form.component.html',
  styleUrl: './author-form.component.scss',
})
export class AuthorFormComponent implements OnInit, OnChanges {
  @Input() author?: Author;
  @Input() saving: boolean = false;

  @Output() formSubmit = new EventEmitter<Author>();
  @Output() formCancel = new EventEmitter<void>();

  authorForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.authorForm = this.createForm();
  }

  ngOnChanges(): void {
    if (this.author && this.authorForm) {
      this.populateForm();
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      firstName: [
        '',
        [Validators.required, Validators.maxLength(100), this.nameValidator()],
      ],
      lastName: [
        '',
        [Validators.required, Validators.maxLength(100), this.nameValidator()],
      ],
      dateOfBirth: ['', [this.dateOfBirthValidator()]],
      dateOfDeath: ['', [this.dateOfDeathValidator()]],
      biography: ['', [Validators.maxLength(2000)]],
    });
  }

  private nameValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const namePattern = /^[a-zA-ZÀ-ÿ\s'-]+$/;
      const valid = namePattern.test(control.value);

      return valid
        ? null
        : {
            invalidName: {
              message:
                'Deve conter apenas letras, espaços, hífens e apostrofes.',
            },
          };
    };
  }

  private dateOfBirthValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const birthDate = new Date(control.value);
      const now = new Date();
      const minDate = new Date(1800, 0, 1);

      if (isNaN(birthDate.getTime())) {
        return { invalidDate: { message: 'Data inválida.' } };
      }

      if (birthDate >= now) {
        return {
          futureDate: {
            message: 'A data de nascimento deve ser anterior à data atual.',
          },
        };
      }

      if (birthDate <= minDate) {
        return {
          tooOldDate: {
            message: 'A data de nascimento deve ser posterior ao ano 1800.',
          },
        };
      }

      return null;
    };
  }

  private dateOfDeathValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const deathDate = new Date(control.value);
      const now = new Date();

      if (isNaN(deathDate.getTime())) {
        return { invalidDate: { message: 'Data inválida.' } };
      }

      if (deathDate > now) {
        return {
          futureDate: {
            message: 'A data de falecimento não pode ser no futuro.',
          },
        };
      }

      const birthDateControl = control.parent?.get('dateOfBirth');
      if (birthDateControl?.value) {
        const birthDate = new Date(birthDateControl.value);
        if (!isNaN(birthDate.getTime()) && deathDate <= birthDate) {
          return {
            beforeBirthDate: {
              message:
                'A data de falecimento deve ser posterior à data de nascimento.',
            },
          };
        }
      }

      return null;
    };
  }

  private populateForm(): void {
    if (this.author) {
      this.authorForm.patchValue({
        firstName: this.author.firstName,
        lastName: this.author.lastName,
        dateOfBirth: this.author.dateOfBirth || '',
        dateOfDeath: this.author.dateOfDeath || '',
        biography: this.author.biography || '',
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.authorForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldErrorMessage(fieldName: string): string {
    const field = this.authorForm.get(fieldName);

    if (!field || !field.errors || !field.touched) {
      return '';
    }

    const errors = field.errors;

    if (fieldName === 'firstName' || fieldName === 'lastName') {
      const fieldLabel =
        fieldName === 'firstName' ? 'primeiro nome' : 'sobrenome';

      if (errors['required']) {
        return `O ${fieldLabel} é obrigatório.`;
      }
      if (errors['maxlength']) {
        return `O ${fieldLabel} não pode ter mais de 100 caracteres.`;
      }
      if (errors['invalidName']) {
        return `O ${fieldLabel} deve conter apenas letras, espaços, hífens e apostrofes.`;
      }
    }

    if (fieldName === 'dateOfBirth') {
      if (errors['invalidDate']) {
        return 'Data de nascimento inválida.';
      }
      if (errors['futureDate']) {
        return 'A data de nascimento deve ser anterior à data atual.';
      }
      if (errors['tooOldDate']) {
        return 'A data de nascimento deve ser posterior ao ano 1800.';
      }
    }

    if (fieldName === 'dateOfDeath') {
      if (errors['invalidDate']) {
        return 'Data de falecimento inválida.';
      }
      if (errors['futureDate']) {
        return 'A data de falecimento não pode ser no futuro.';
      }
      if (errors['beforeBirthDate']) {
        return 'A data de falecimento deve ser posterior à data de nascimento.';
      }
    }

    if (fieldName === 'biography') {
      if (errors['maxlength']) {
        return 'A biografia não pode ter mais de 2000 caracteres.';
      }
    }

    return 'Campo inválido.';
  }

  onSubmit(): void {
    if (this.authorForm.valid && !this.saving) {
      const formValue = this.authorForm.value;

      const authorData: Author = {
        ...formValue,
        id: this.author?.id,
      };

      Object.keys(authorData).forEach((key) => {
        if (
          authorData[key as keyof Author] === '' ||
          authorData[key as keyof Author] === null
        ) {
          delete authorData[key as keyof Author];
        }
      });

      this.formSubmit.emit(authorData);
    } else {
      this.markFormGroupTouched();
    }
  }

  onCancel(): void {
    this.authorForm.reset();
    this.formCancel.emit();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.authorForm.controls).forEach((key) => {
      const control = this.authorForm.get(key);
      control?.markAsTouched();
    });
  }

  resetForm(): void {
    this.authorForm.reset();
  }

  ngOnInit(): void {
    if (this.author) {
      this.populateForm();
    }

    this.authorForm.get('dateOfBirth')?.valueChanges.subscribe(() => {
      this.authorForm.get('dateOfDeath')?.updateValueAndValidity();
    });
  }
}
