import { CommonModule } from '@angular/common';
import { Component, Input, TemplateRef } from '@angular/core';

export interface CardAction {
  label: string;
  icon: string;
  cssClass: string;
  action: () => void;
}

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss',
})
export class CardComponent {
  @Input() title: string = '';
  @Input() subtitle?: string;
  @Input() description?: string;
  @Input() iconClass: string = 'fas fa-file';
  @Input() iconColor: string = 'var(--info-color)';
  @Input() actions: CardAction[] = [];
  @Input() data?: any;
  @Input() contentTemplate?: TemplateRef<any>;
}
