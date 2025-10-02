import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';
import { AppState, initialState } from './state';

@Injectable({
  providedIn: 'root',
})
export class Store {
  private state$ = new BehaviorSubject<AppState>(initialState);

  get state(): AppState {
    return this.state$.getValue();
  }

  select<K>(selector: (state: AppState) => K): Observable<K> {
    return this.state$
      .asObservable()
      .pipe(map(selector), distinctUntilChanged());
  }

  update(reducer: (state: AppState) => AppState): void {
    const currentState = this.state;
    const newState = reducer(currentState);
    this.state$.next(newState);
  }

  dispatch(action: { type: string; payload?: any }): void {
    switch (action.type) {
      case '[Theme] Toggle':
        this.update((state) => ({
          ...state,
          theme: state.theme === 'dark' ? 'light' : 'dark',
        }));
        break;
    }
  }
}
