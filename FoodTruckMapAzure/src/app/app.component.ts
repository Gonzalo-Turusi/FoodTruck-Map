import { Component } from '@angular/core';
import { FoodTrucksComponent } from "./components/food-trucks/food-trucks.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FoodTrucksComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent{}
