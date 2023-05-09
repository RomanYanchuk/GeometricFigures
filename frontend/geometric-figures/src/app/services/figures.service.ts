import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Figure } from './figure';
import { FiguresResponse } from './figures-response';

@Injectable({
  providedIn: 'root',
})
export class FiguresService {
  private serviceRoot = environment.serviceApi + 'figures';
  constructor(private http: HttpClient) {}

  public get(
    sortField: string,
    isAscending: boolean,
    pageSize: number,
    pageNumber: number,
    searchText: string
  ): Observable<FiguresResponse> {
    return this.http.get<FiguresResponse>(this.serviceRoot, {
      params: { sortField, pageSize, isAscending, pageNumber, searchText },
    });
  }

  public getOne(id: number): Observable<Figure> {
    return this.http.get<Figure>(this.serviceRoot + '/' + id);
  }

  public create(figure: Figure): Observable<object> {
    return this.http.post<object>(this.serviceRoot, figure);
  }

  public delete(figure: Figure): Observable<object> {
    return this.http.delete<object>(this.serviceRoot + '/' + figure.id);
  }

  public edit(figure: Figure): Observable<object> {
    return this.http.put<object>(this.serviceRoot + '/' + figure.id, figure);
  }
}
