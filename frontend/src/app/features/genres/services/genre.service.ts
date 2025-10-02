import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../shared/components/pagination/pagination.component';
import { PagedParameters } from '../../../shared/interfaces/pagination/paged-parameters.interface';
import { Genre } from '../models/genre.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GenreService {
  private readonly apiUrl = environment.apiUrl;
  private readonly baseUrl = this.apiUrl + '/genres';

  constructor(private http: HttpClient) {}

  getAll(parameters: PagedParameters): Observable<PagedResult<Genre>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Genre>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Genre> {
    return this.http.get<Genre>(`${this.baseUrl}/${id}`);
  }

  create(createDto: Genre): Observable<Genre> {
    return this.http.post<Genre>(this.baseUrl, createDto);
  }

  update(id: string, updateDto: Genre): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, updateDto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  searchByName(
    name: string,
    parameters: PagedParameters
  ): Observable<PagedResult<Genre>> {
    const params = new HttpParams()
      .set('name', name)
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Genre>>(`${this.baseUrl}/search`, {
      params,
    });
  }

  getAllWithBooks(parameters: PagedParameters): Observable<PagedResult<Genre>> {
    const params = new HttpParams()
      .set('pageNumber', parameters.pageNumber.toString())
      .set('pageSize', parameters.pageSize.toString());

    return this.http.get<PagedResult<Genre>>(`${this.baseUrl}/with-books`, {
      params,
    });
  }
}
