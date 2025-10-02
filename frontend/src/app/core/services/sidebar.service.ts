import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SidebarService {
  private isCollapsedSubject = new BehaviorSubject<boolean>(false);
  private isMobileSubject = new BehaviorSubject<boolean>(false);

  isCollapsed$: Observable<boolean> = this.isCollapsedSubject.asObservable();
  isMobile$: Observable<boolean> = this.isMobileSubject.asObservable();

  get isCollapsed(): boolean {
    return this.isCollapsedSubject.value;
  }

  get isMobile(): boolean {
    return this.isMobileSubject.value;
  }

  setCollapsed(collapsed: boolean): void {
    this.isCollapsedSubject.next(collapsed);
  }

  setMobile(mobile: boolean): void {
    this.isMobileSubject.next(mobile);
  }

  toggle(): void {
    this.setCollapsed(!this.isCollapsed);
  }
}
