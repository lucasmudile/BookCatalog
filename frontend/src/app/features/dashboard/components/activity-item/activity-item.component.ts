import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-activity-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './activity-item.component.html',
  styleUrl: './activity-item.component.scss',
})
export class ActivityItemComponent {
  @Input() title: string = '';
  @Input() description: string = '';
  @Input() timestamp: string = '';
  @Input() icon: string = 'fas fa-book';
  @Input() iconColor: string = 'var(--info-color)';
  @Input() activityType: 'book' | 'author' | 'genre' = 'book';
}
