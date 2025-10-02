import { CommonModule } from '@angular/common';
import { Component, forwardRef, Input } from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-form-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormInputComponent),
      multi: true,
    },
  ],
  templateUrl: './form-input.component.html',
  styleUrl: './form-input.component.scss',
})
export class FormInputComponent implements ControlValueAccessor {
  @Input() label?: string;
  @Input() type: string = 'text';
  @Input() placeholder?: string;
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() hasError: boolean = false;
  @Input() errorMessage?: string;
  @Input() inputId: string = `input-${Math.random().toString(36).substr(2, 9)}`;

  private _value: string = '';

  get value(): string {
    return this._value;
  }

  set value(val: string) {
    this._value = val;
    this.onChange(
      this.type === 'number' ? (val === '' ? null : parseFloat(val)) : val
    );
  }

  private onChange = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    if (value === null) {
      this._value = '';
    } else {
      this._value =
        this.type === 'number' && typeof value === 'number'
          ? value.toString()
          : value;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
  }

  onBlur(): void {
    this.onTouched();
  }
}
