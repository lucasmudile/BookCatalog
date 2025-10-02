import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SidebarService } from '../../core/services/sidebar.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss',
})
export class MainLayoutComponent implements OnInit, OnDestroy {
  isCollapsed = false;
  isMobile = false;
  private subscriptions = new Subscription();

  constructor(private sidebarService: SidebarService, private router: Router) {}

  ngOnInit() {
    this.subscriptions.add(
      this.sidebarService.isCollapsed$.subscribe(
        (collapsed) => (this.isCollapsed = collapsed)
      )
    );

    this.subscriptions.add(
      this.sidebarService.isMobile$.subscribe(
        (mobile) => (this.isMobile = mobile)
      )
    );
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  navigateTo(route: string) {
    this.router.navigate([route]);
  }

  isActiveRoute(route: string): boolean {
    if (route === '') {
      return this.router.url === '/' || this.router.url === '';
    }
    return this.router.url === route || this.router.url.startsWith(route + '/');
  }
}
