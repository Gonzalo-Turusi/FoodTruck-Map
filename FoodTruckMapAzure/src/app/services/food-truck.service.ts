import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { FoodTruck } from '../interfaces/food-truck';

@Injectable({
  providedIn: 'root'
})
export class FoodTruckService {

  private apiUrl = 'http://localhost:7071/foodtrucks'; // URL de tu Azure Function local
  private tokenUrl = 'http://localhost:7071/token'; // URL del endpoint para obtener el token

  constructor(private http: HttpClient) { }

  private getToken(): Observable<string> {
    const credentials = { Username: 'admin', Password: 'password' }; // Static credentials
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });
    return this.http.post<{ token: string }>(this.tokenUrl, credentials, { headers }).pipe(
      map(response => response.token)
    );
  }

  getFoodTrucks(): Observable<{ latitude: number; longitude: number }[]> {
    return this.getToken().pipe(
      switchMap(token => {
        const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Access-Control-Allow-Origin': '*'
        });
  
        // Transformar los datos para devolver solo coordenadas
        return this.http.get<FoodTruck[]>(this.apiUrl, { headers }).pipe(
          map(foodTrucks => 
            foodTrucks.map(ft => ({
              latitude: parseFloat(ft.location.latitude),
              longitude: parseFloat(ft.location.longitude)
            }))
          )
        );
      }),
      catchError(error => {
        console.error('Error fetching food trucks:', error);
        throw error;
      })
    );
  }
  
}
