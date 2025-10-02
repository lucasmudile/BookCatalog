import {
  HttpInterceptorFn,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, tap, throwError } from 'rxjs';

export const toastInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);

  return next(req).pipe(
    tap((event) => {
      if (event instanceof HttpResponse) {
        const status = event.status;
        if (status === 201) {
          console.log('Recurso criado com sucesso, interceptor');
          toastr.success('Recurso criado com sucesso');
        }

        if (status === 204) {
          toastr.success('Operação realizada com sucesso');
        }
      }
    }),
    catchError((error: HttpErrorResponse) => {
      let message = 'Erro inesperado.';

      switch (error.status) {
        case 0:
          message = 'Sem conexão com o servidor.';
          break;
        case 400:
          message = 'Requisição inválida.';
          break;
        case 401:
          message = 'Não autorizado.';
          break;
        case 403:
          message = 'Acesso negado.';
          break;
        case 404:
          message = 'Recurso não encontrado.';
          break;
        case 500:
          message = 'Erro interno do servidor.';
          break;
        default:
          message = `Erro ${error.status}: ${error.statusText}`;
      }

      toastr.error(message);
      return throwError(() => error);
    })
  );
};
