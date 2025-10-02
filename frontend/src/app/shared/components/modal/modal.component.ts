import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss',
})
export class ModalComponent implements OnInit, OnDestroy {
  @ViewChild('modalContent') modalContentRef?: ElementRef;

  // Inputs para configuração
  @Input() isOpen: boolean = false;
  @Input() title?: string;
  @Input() titleIcon?: string;
  @Input() maxWidth: string = '480px';
  @Input() closeOnBackdrop: boolean = true;
  @Input() closeOnEscape: boolean = true;
  @Input() data?: any; // Para passar dados para templates

  // Templates customizados
  @Input() headerTemplate?: TemplateRef<any>;
  @Input() bodyTemplate?: TemplateRef<any>;
  @Input() footerTemplate?: TemplateRef<any>;

  // Footer padrão
  @Input() showDefaultFooter: boolean = false;
  @Input() confirmText: string = 'Confirmar';
  @Input() cancelText: string = 'Cancelar';
  @Input() confirmIcon: string = 'fas fa-check';
  @Input() confirmDisabled: boolean = false;

  // Outputs
  @Output() modalClose = new EventEmitter<void>();
  @Output() modalConfirm = new EventEmitter<any>();
  @Output() modalCancel = new EventEmitter<void>();
  @Output() backdropClick = new EventEmitter<void>();

  // Estado interno
  isClosing = false;
  private originalBodyOverflow?: string;

  ngOnInit(): void {
    if (this.isOpen) {
      this.onModalOpen();
    }
  }

  ngOnDestroy(): void {
    this.restoreBodyScroll();
    this.removeEventListeners();
  }

  ngOnChanges(): void {
    if (this.isOpen) {
      this.onModalOpen();
    } else {
      this.onModalClose();
    }
  }

  private onModalOpen(): void {
    this.preventBodyScroll();
    this.addEventListeners();

    // Focus no modal para acessibilidade
    setTimeout(() => {
      if (this.modalContentRef) {
        this.modalContentRef.nativeElement.focus();
      }
    }, 100);
  }

  private onModalClose(): void {
    this.isClosing = true;

    // Aguardar animação antes de remover completamente
    setTimeout(() => {
      this.isClosing = false;
      this.restoreBodyScroll();
      this.removeEventListeners();
    }, 300);
  }

  private preventBodyScroll(): void {
    this.originalBodyOverflow = document.body.style.overflow;
    document.body.style.overflow = 'hidden';
  }

  private restoreBodyScroll(): void {
    if (this.originalBodyOverflow !== undefined) {
      document.body.style.overflow = this.originalBodyOverflow;
    }
  }

  private addEventListeners(): void {
    if (this.closeOnEscape) {
      document.addEventListener('keydown', this.onEscapeKey);
    }
  }

  private removeEventListeners(): void {
    document.removeEventListener('keydown', this.onEscapeKey);
  }

  private onEscapeKey = (event: KeyboardEvent): void => {
    if (event.key === 'Escape' && this.isOpen) {
      this.close();
    }
  };

  onBackdropClick(event: MouseEvent): void {
    // Verificar se o clique foi no backdrop (não no conteúdo)
    if (event.target === event.currentTarget && this.closeOnBackdrop) {
      this.backdropClick.emit();
      this.close();
    }
  }

  close(): void {
    this.modalClose.emit();
    this.modalCancel.emit();
  }

  confirm(): void {
    this.modalConfirm.emit(this.data);
  }

  // Métodos públicos para controle externo
  open(): void {
    this.isOpen = true;
  }

  forceClose(): void {
    this.isOpen = false;
  }
}
