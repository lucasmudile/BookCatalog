import { Component } from '@angular/core';
import { Store } from '../../store/store.service';
import { ThemeActions } from '../../store/theme/theme.actions';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  currentTheme$ = this.store.select((state) => state.theme);

  constructor(private store: Store) {}

  toggleTheme() {
    this.store.dispatch(ThemeActions.toggle());
  }
}
