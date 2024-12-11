import { Component, OnInit } from '@angular/core';
import { AzureMapComponent } from './components/azure-map/azure-map.component';
import { FoodTruckService } from './services/food-truck.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [CommonModule, AzureMapComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  foodTruckLocations: { latitude: number; longitude: number }[] = [];

  constructor(private foodTruckService: FoodTruckService) {}

  ngOnInit(): void {
    this.foodTruckService.getFoodTrucks().subscribe({
      next: locations => {
        this.foodTruckLocations = locations;
        console.log(this.foodTruckLocations);
      },
      error: err => console.error('Failed to load food trucks', err)
    });
  }
}
