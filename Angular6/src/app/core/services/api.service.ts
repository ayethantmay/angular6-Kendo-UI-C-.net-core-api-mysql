import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { toODataString } from '@progress/kendo-data-query';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
};

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public loading: boolean;
  constructor(private http: HttpClient) { }
  private formatErrors(error: any) {
    return  throwError(error.error);
  }

  fetchgrid(path: string, state: any): Observable<GridDataResult> {
    const queryStr = `${toODataString(state)}&$count=true`;
    this.loading = true;
    console.log(`${environment.api_url}${path}?${queryStr}`);
    return this.http.get(`${environment.api_url}${path}?${queryStr}`)
        .pipe(
            map(response => (<GridDataResult>{
                data: response['data'],
                total: parseInt(response['dataFoundRowsCount'], 10)
            })),
            tap(() => this.loading = false)
        );
  }

  fetchgrid_post(path: string, state: any): Observable<GridDataResult> {
    this.loading = true;
    return this.http.post(`${environment.api_url}${path}`, this.convert_post_form_parameter(state))
        .pipe(
            map(response => (<GridDataResult>{
                data: response['data'],
                total: parseInt(response['dataFoundRowsCount'], 10)
            })),
            tap(() => this.loading = false)
        );
  }
  fetch_get(path: string): Observable<any> {
    this.loading = true;
    return this.http.get(`${environment.api_url}${path}`)
        .pipe();
  }
  
  get(path: string): Observable<any> {
    return this.http.get(`${environment.api_url}${path}`)
      .pipe(catchError(this.formatErrors));
  }

  Get(path): Observable<any> {
    return this.http.get(
      `${environment.api_url}${path}`
    );
  }

  postAuth(path: string, body: Object = {}): Observable<any> {
    return this.http.post(
      `${environment.api_url}${path}`,
      JSON.stringify(body), httpOptions
    ).pipe(catchError(this.formatErrors));
  }

  post_twoparameter(path: string, param1: Object = {}, param2: Object = {}): Observable<any> {
    return this.http.post(
      `${environment.api_url}${path}`,
      this.convert_post_form_two_parameter(param1, param2)
    )
    .pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }

  post(path: string, body: Object = {}): Observable<any> {
    return this.http.post(
      `${environment.api_url}${path}`,
      this.convert_post_form_parameter(body)
    )
    .pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }

  post_Url_parameter(path, parameter): Observable<any> {
    const obj_post_param = new FormData();
    obj_post_param.append('objAdminMenuList', parameter);
    return this.http.post(
      `${environment.api_url}${path}`, obj_post_param
    ).pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }
  post_Url_two_parameter(path, param1, param2): Observable<any> {
    const obj_post_param = new FormData();
    obj_post_param.append('active', param1);
    obj_post_param.append('blocked', param2);
    return this.http.post(
      `${environment.api_url}${path}`, obj_post_param
    ).pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }

  delete(path): Observable<any> {
    return this.http.delete(
      `${environment.api_url}${path}`
    ).pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }

  protected convert_post_form_parameter(formDataObj: any): FormData {
    const obj_post_param = new FormData();
    obj_post_param.append('formDataObj', JSON.stringify(formDataObj));
    return obj_post_param;
  }
  protected convert_post_form_two_parameter(formDataObj1: any, formDataObj2: any): FormData {
    const obj_post_param = new FormData();
    obj_post_param.append('formDataObj1', JSON.stringify(formDataObj1));
    obj_post_param.append('formDataObj2', JSON.stringify(formDataObj2));
    return obj_post_param;
  }


  saveSocial_Url_parameter(path, parameter): Observable<any> {
    const obj_post_param = new FormData(); 
    obj_post_param.append('formDataObj', JSON.stringify(parameter));
    return this.http.post(
      `${environment.api_url}${path}`, obj_post_param
    ).pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }

  saveLoanOption_Url_parameter(path, parameter): Observable<any> {
    const obj_post_param = new FormData(); 
    obj_post_param.append('formDataObj', JSON.stringify(parameter));
    return this.http.post(
      `${environment.api_url}${path}`, obj_post_param
    ).pipe(
      map(response => response['data']),
      catchError(this.formatErrors));
  }
  

}
