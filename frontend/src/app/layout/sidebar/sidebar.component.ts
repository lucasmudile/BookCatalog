import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { SidebarService } from '../../core/services/sidebar.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent implements OnInit, OnDestroy {
  isCollapsed = false;
  isMobile = false;

  constructor(private router: Router, private sidebarService: SidebarService) {}

  ngOnInit() {
    this.checkScreenSize();
  }

  ngOnDestroy() {}

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.checkScreenSize();
  }

  private checkScreenSize() {
    const width = window.innerWidth;
    this.isMobile = width < 768;
    this.sidebarService.setMobile(this.isMobile);

    this.sidebarService.setCollapsed(this.isCollapsed);
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
    this.sidebarService.setCollapsed(this.isCollapsed);
  }

  navigateTo(route: string) {
    this.router.navigate([route]);

    if (this.isMobile) {
      this.isCollapsed = true;
      this.sidebarService.setCollapsed(this.isCollapsed);
    }
  }

  isActiveRoute(route: string): boolean {
    if (route === '') {
      return this.router.url === '/' || this.router.url === '';
    }
    return this.router.url === route || this.router.url.startsWith(route + '/');
  }
}
