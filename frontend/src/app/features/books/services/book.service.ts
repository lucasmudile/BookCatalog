import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../shared/components/pagination/pagination.component';
import { PagedParameters } from '../../../shared/interfaces/pagination/paged-parameters.interface';
import { Book } from '../models/book.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private readonly apiUrl = environment.apiUrl;
  private readonly baseUrl = this.apiUrl + '/books';

  constructor(private http: HttpClient) {}

  getAll(parameters: PagedParameters): Observable<PagedResult<Book>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Book>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Book> {
    return this.http.get<Book>(`${this.baseUrl}/${id}`);
  }

  create(createDto: Book): Observable<Book> {
    return this.http.post<Book>(this.baseUrl, createDto);
  }

  update(id: string, updateDto: Book): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, updateDto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  searchByName(
    name: string,
    parameters: PagedParameters
  ): Observable<PagedResult<Book>> {
    const params = new HttpParams()
      .set('name', name)
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Book>>(`${this.baseUrl}/search`, {
      params,
    });
  }

  getByAuthorId(
    authorId: string,
    parameters: PagedParameters
  ): Observable<PagedResult<Book>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Book>>(
      `${this.baseUrl}/author/${authorId}`,
      { params }
    );
  }

  getByGenreId(
    genreId: string,
    parameters: PagedParameters
  ): Observable<PagedResult<Book>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Book>>(
      `${this.baseUrl}/genre/${genreId}`,
      { params }
    );
  }
}
