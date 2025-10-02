export class ThemeActions {
  static readonly TOGGLE = '[Theme] Toggle';
  static readonly SET = '[Theme] Set';

  static toggle() {
    return { type: ThemeActions.TOGGLE };
  }

  static set(theme: 'light' | 'dark') {
    return { type: ThemeActions.SET, payload: theme };
  }
}
