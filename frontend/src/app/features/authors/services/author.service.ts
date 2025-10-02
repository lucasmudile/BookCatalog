import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../shared/components/pagination/pagination.component';
import { PagedParameters } from '../../../shared/interfaces/pagination/paged-parameters.interface';
import { Author } from '../models/author.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthorService {
  private readonly apiUrl = environment.apiUrl;
  private readonly baseUrl = this.apiUrl + '/authors';

  constructor(private http: HttpClient) {}

  getAll(parameters: PagedParameters): Observable<PagedResult<Author>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Author>>(this.baseUrl, {
      params,
    });
  }

  getById(id: string): Observable<Author> {
    return this.http.get<Author>(`${this.baseUrl}/${id}`);
  }

  create(createDto: Author): Observable<Author> {
    return this.http.post<Author>(this.baseUrl, createDto);
  }

  update(id: string, updateDto: Author): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, updateDto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  searchByName(
    name: string,
    parameters: PagedParameters
  ): Observable<PagedResult<Author>> {
    const params = new HttpParams()
      .set('name', name)
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Author>>(`${this.baseUrl}/search`, {
      params,
    });
  }

  getAllWithBooks(
    parameters: PagedParameters
  ): Observable<PagedResult<Author>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Author>>(`${this.baseUrl}/with-books`, {
      params,
    });
  }

  getByIdWithBooks(id: string): Observable<Author> {
    return this.http.get<Author>(`${this.baseUrl}/${id}/with-books`);
  }
}
