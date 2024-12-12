// src/app/components/azure-map/azure-map.component.ts
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import * as atlas from 'azure-maps-control';

@Component({
  selector: 'app-azure-map',
  imports: [CommonModule],
  templateUrl: './azure-map.component.html',
  styleUrls: ['./azure-map.component.css']
})
export class AzureMapComponent implements OnInit {
  @Input() foodTruckLocations: { latitude: number; longitude: number }[] = [];
  @Output() boundsChanged = new EventEmitter<{ minLatitude: number; maxLatitude: number; minLongitude: number; maxLongitude: number, centerLatitude: number, centerLongitude: number }>();

  private map!: atlas.Map;

  ngOnInit(): void {
    this.initializeMap();
  }

  private initializeMap(): void {
    this.map = new atlas.Map('azureMap', {
      center: [-122.4194, 37.7749],
      zoom: 14,
      language: 'en-US',
      authOptions: {
        authType: atlas.AuthenticationType.subscriptionKey,
        subscriptionKey: '6CfUS5E5IW13lHWVaBpp7uvuRhlFRkbP4T4zBOLagkzGbNAhGqYyJQQJ99ALACYeBjFqnTLfAAAgAZMP3jN7'
      }
    });

    this.map.events.add('moveend', () => {
      const bounds = this.map.getCamera().bounds as [number, number, number, number]; // BoundingBox [west, south, east, north]
      const center = this.map.getCamera().center as [number, number]; // Centro actual del mapa
      this.boundsChanged.emit({
        minLongitude: bounds[0], // west
        minLatitude: bounds[1], // south
        maxLongitude: bounds[2], // east
        maxLatitude: bounds[3],  // north
        centerLatitude: center[1], // latitud del centro
        centerLongitude: center[0] // longitud del centro
      });
    });
  }

  ngOnChanges(): void {
    if (this.map) {
      this.map.markers.clear();
      this.foodTruckLocations.forEach(location => {
        const marker = new atlas.HtmlMarker({
          position: [location.longitude, location.latitude],
          color: 'blue'
        });
        this.map.markers.add(marker);
      });
    }
  }
}