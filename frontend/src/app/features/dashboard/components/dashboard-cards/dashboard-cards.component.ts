import { Component, Input } from '@angular/core';
import { DashboardCardComponent } from '../dashboard-card/dashboard-card.component';

@Component({
  selector: 'app-dashboard-cards',
  standalone: true,
  imports: [DashboardCardComponent],
  templateUrl: './dashboard-cards.component.html',
  styleUrl: './dashboard-cards.component.scss',
})
export class DashboardCardsComponent {
  @Input() totalBooks: number = 0;
  @Input() totalAuthors: number = 0;
  @Input() totalGenres: number = 0;
}
