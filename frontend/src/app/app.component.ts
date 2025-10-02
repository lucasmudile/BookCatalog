import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from './store/store.service';
import { ThemeEffects } from './store/theme/theme.effects';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  theme$ = this.store.select((state) => state.theme);

  constructor(private store: Store, private themeEffects: ThemeEffects) {}

  ngOnInit() {
    this.themeEffects.initializeTheme();
    this.themeEffects.handleThemeChanges();
  }
}
