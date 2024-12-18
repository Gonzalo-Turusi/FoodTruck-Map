import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { FoodTruck } from '../interfaces/food-truck';
import { FoodTruckShort } from '../interfaces/food-truck-short';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FoodTruckService {

  private apiUrlToken = environment.apiUrlToken;
  private apiUrlFoodtrucks = environment.apiUrlFoodtrucks;

  constructor(private http: HttpClient) { }

  private getToken(): Observable<string> {
    const credentials = { Username: 'admin', Password: 'password' }; // Static credentials
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });
    return this.http.post<{ token: string }>(this.apiUrlToken, credentials, { headers }).pipe(
      map(response => response.token)
    );
  }

  getFoodTrucks(): Observable<FoodTruckShort[]> {
    return this.getToken().pipe(
      switchMap(token => {
        const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Access-Control-Allow-Origin': '*'
        });
  
        return this.http.get<FoodTruck[]>(this.apiUrlFoodtrucks, { headers }).pipe(
          map(foodTrucks => {
            if (foodTrucks) {
              return foodTrucks.map(ft => ({
                latitude: parseFloat(ft.location.latitude),
                longitude: parseFloat(ft.location.longitude),
                applicant: ft.applicant,
                address: ft.address,
                fooditems: ft.fooditems
              }));
            }
            return [];
          })
        );
      }),
      catchError(error => {
        console.error('Error fetching food trucks:', error);
        throw error;
      })
    );
  }
  
  
}
