// src/app/components/azure-map/azure-map.component.ts
import { Component, Input, OnInit } from '@angular/core';
import * as atlas from 'azure-maps-control';

@Component({
  selector: 'app-azure-map',
  standalone: true,
  templateUrl: './azure-map.component.html',
  styleUrls: ['./azure-map.component.css']
})
export class AzureMapComponent implements OnInit {
  @Input() foodTruckLocations: { latitude: number; longitude: number }[] = [];
  
  private map!: atlas.Map;

  ngOnInit(): void {
    this.initializeMap();
  }

  private initializeMap(): void {
    this.map = new atlas.Map('azureMap', {
      center: [-122.4194, 37.7749], // Coordenadas iniciales (Seattle, por ejemplo)
      zoom: 12,
      language: 'en-US',
      authOptions: {
        authType: atlas.AuthenticationType.subscriptionKey,
        subscriptionKey: '6CfUS5E5IW13lHWVaBpp7uvuRhlFRkbP4T4zBOLagkzGbNAhGqYyJQQJ99ALACYeBjFqnTLfAAAgAZMP3jN7' // Reemplaza con tu clave de Azure Maps
      }
    });

    this.map.events.add('ready', () => {
      this.addFoodTruckMarkers();
    });
  }

  private addFoodTruckMarkers(): void {
    console.log(this.foodTruckLocations);
    this.foodTruckLocations.forEach(location => {
      const marker = new atlas.HtmlMarker({
        position: [location.longitude, location.latitude],
        color: 'red'
      });
      this.map.markers.add(marker);
    });

    console.log("terminé")
  }
}
