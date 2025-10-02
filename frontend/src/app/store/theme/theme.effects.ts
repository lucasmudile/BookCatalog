import { Injectable } from '@angular/core';
import { Store } from '../store.service';
import { ThemeActions } from './theme.actions';

@Injectable({
  providedIn: 'root',
})
export class ThemeEffects {
  constructor(private store: Store) {}

  initializeTheme() {
    const savedTheme =
      (localStorage.getItem('theme') as 'light' | 'dark') || 'dark';
    this.store.dispatch(ThemeActions.set(savedTheme));
  }

  handleThemeChanges() {
    this.store
      .select((state) => state.theme)
      .subscribe((theme) => {
        document.body.classList.remove('light-theme', 'dark-theme');
        document.body.classList.add(
          theme === 'light' ? 'light-theme' : 'dark-theme'
        );
        localStorage.setItem('theme', theme);
      });
  }
}
