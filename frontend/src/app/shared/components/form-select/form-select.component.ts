import { CommonModule } from '@angular/common';
import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface SelectOption {
  value: any;
  label: string;
}

@Component({
  selector: 'app-form-select',
  standalone: true,
  imports: [CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormSelectComponent),
      multi: true,
    },
  ],
  templateUrl: './form-select.component.html',
  styleUrl: './form-select.component.scss',
})
export class FormSelectComponent implements ControlValueAccessor {
  @Input() label?: string;
  @Input() placeholder?: string;
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() options: SelectOption[] = [];
  @Input() hasError: boolean = false;
  @Input() errorMessage?: string;
  @Input() selectId: string = `select-${Math.random()
    .toString(36)
    .substr(2, 9)}`;

  value: any = '';
  private onChangeCallback = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.value = target.value;
    this.onChangeCallback(target.value);
  }

  onBlur(): void {
    this.onTouched();
  }
}
