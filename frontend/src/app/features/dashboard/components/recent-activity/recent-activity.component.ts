import { Component, Input } from '@angular/core';
import { ActivityItemComponent } from '../activity-item/activity-item.component';
import { NgFor, CommonModule } from '@angular/common';

export interface RecentActivity {
  id: string;
  type: 'book' | 'author' | 'genre';
  action: 'created' | 'updated' | 'deleted';
  title: string;
  description: string;
  timestamp: Date;
  icon: string;
}

@Component({
  selector: 'app-recent-activity',
  standalone: true,
  imports: [ActivityItemComponent, NgFor, CommonModule],
  templateUrl: './recent-activity.component.html',
  styleUrl: './recent-activity.component.scss',
})
export class RecentActivityComponent {
  @Input() activities: RecentActivity[] = [];

  trackByActivityId(index: number, activity: RecentActivity): string {
    return activity.id;
  }

  getIconColor(type: string): string {
    switch (type) {
      case 'book':
        return 'var(--info-color)';
      case 'author':
        return 'var(--success-color)';
      case 'genre':
        return 'var(--warning-color)';
      default:
        return 'var(--info-color)';
    }
  }

  formatTimestamp(timestamp: Date): string {
    const now = new Date();
    const diff = now.getTime() - timestamp.getTime();
    const hours = Math.floor(diff / (1000 * 60 * 60));
    const days = Math.floor(hours / 24);

    if (days > 0) {
      return `há ${days} dia${days > 1 ? 's' : ''}`;
    } else if (hours > 0) {
      return `há ${hours} hora${hours > 1 ? 's' : ''}`;
    } else {
      return 'há poucos minutos';
    }
  }
}
