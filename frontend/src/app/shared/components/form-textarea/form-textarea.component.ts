import { CommonModule } from '@angular/common';
import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-form-textarea',
  standalone: true,
  imports: [CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormTextareaComponent),
      multi: true,
    },
  ],
  templateUrl: './form-textarea.component.html',
  styleUrl: './form-textarea.component.scss',
})
export class FormTextareaComponent implements ControlValueAccessor {
  @Input() label?: string;
  @Input() placeholder?: string;
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() rows: number = 3;
  @Input() hasError: boolean = false;
  @Input() errorMessage?: string;
  @Input() textareaId: string = `textarea-${Math.random()
    .toString(36)
    .substr(2, 9)}`;

  value: any = '';
  private onChange = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    this.value = value;
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
    const target = event.target as HTMLTextAreaElement;
    this.value = target.value;
    this.onChange(target.value);
  }

  onBlur(): void {
    this.onTouched();
  }
}
