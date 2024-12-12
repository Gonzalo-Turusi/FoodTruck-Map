import { Component, OnInit } from '@angular/core';
import { FoodTruckService } from '../../services/food-truck.service';
import { AzureMapComponent } from '../azure-map/azure-map.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-food-trucks',
  imports: [CommonModule, AzureMapComponent],
  templateUrl: './food-trucks.component.html',
  styleUrls: ['./food-trucks.component.css']
})
export class FoodTrucksComponent implements OnInit {
  allFoodTruckLocations: { latitude: number; longitude: number }[] = [];
  visibleFoodTruckLocations: { latitude: number; longitude: number }[] = [];
  showReloadButton = false;

  constructor(private foodTruckService: FoodTruckService) {}

  ngOnInit(): void {
    // Cargar todos los food trucks al inicio
    this.foodTruckService.getFoodTrucks().subscribe({
      next: locations => {
        this.allFoodTruckLocations = locations;
        this.visibleFoodTruckLocations = locations; // Mostrar todos inicialmente
      },
      error: err => console.error('Failed to load food trucks', err)
    });
  }

  onBoundsChanged(bounds: { 
    minLatitude: number; 
    maxLatitude: number; 
    minLongitude: number; 
    maxLongitude: number; 
    centerLatitude: number; 
    centerLongitude: number; 
  }): void {
    // Filtrar las tiendas visibles según los límites del mapa
    const visibleFoodTrucks = this.allFoodTruckLocations.filter(location =>
      location.latitude >= bounds.minLatitude &&
      location.latitude <= bounds.maxLatitude &&
      location.longitude >= bounds.minLongitude &&
      location.longitude <= bounds.maxLongitude
    );
  
    // Ordenar las tiendas por distancia desde el centro del mapa
    const sortedByDistance = visibleFoodTrucks.sort((a, b) => {
      const distanceA = this.calculateDistance(bounds.centerLatitude, bounds.centerLongitude, a.latitude, a.longitude);
      const distanceB = this.calculateDistance(bounds.centerLatitude, bounds.centerLongitude, b.latitude, b.longitude);
      return distanceA - distanceB;
    });
  
    // Mostrar solo las 15 tiendas más cercanas
    this.visibleFoodTruckLocations = sortedByDistance.slice(0, 15);
  }
  
  // Método para calcular la distancia (Haversine formula)
  private calculateDistance(lat1: number, lon1: number, lat2: number, lon2: number): number {
    const R = 6371; // Radio de la Tierra en km
    const dLat = this.degToRad(lat2 - lat1);
    const dLon = this.degToRad(lon2 - lon1);
    const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
              Math.cos(this.degToRad(lat1)) * Math.cos(this.degToRad(lat2)) *
              Math.sin(dLon / 2) * Math.sin(dLon / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return R * c; // Distancia en km
  }
  
  private degToRad(deg: number): number {
    return deg * (Math.PI / 180);
  }
  
}
