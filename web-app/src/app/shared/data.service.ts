import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ConstantsService } from './constants.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient) { }

  /**
* Handle Http operation that failed.
* @param operation - name of the operation that failed
* @param result - optional value to return as the observable result
*/
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<any> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /**
  * Sends Http Post request to server
  * @param _this - this object
  * @param url - Url of API
  * @param data - optional data to be sent to server
  * @param token - Access token for resource API
  */
  private getApiResponse(_this, url, token, data = null) {
    const httpOptions = {
      headers: null
    };
    if (httpOptions.headers == null) {
      httpOptions.headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token
      });
    }
    if (data) {
      return _this.http.post(url, data, httpOptions)
        .pipe(
          catchError(_this.handleError('getApiResponse'))
        );
    } else {
      return _this.http.get(url, httpOptions)
        .pipe(
          catchError(_this.handleError('getApiResponse'))
        );
    }
  }

  /**
    * Sends Http Post request to server
    * @param url - Url of API
    * @param data - Data to be sent to server
    */
  post(url, data): Observable<any> {
    url = ConstantsService.URL.API + url;
    return this.getApiResponse(this, url, 'Anonymous', data);
  }

  /**
  * Sends Http Get request to server
  * @param url - Url of API
  */
  get(url): Observable<any> {
    const _this = this;
    let resource;
    url = ConstantsService.URL.API + url;
    return this.getApiResponse(this, url, 'Anonymous');
  }
}
