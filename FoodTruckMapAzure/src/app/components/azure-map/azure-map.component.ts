// src/app/components/azure-map/azure-map.component.ts
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, OnInit, SimpleChanges } from '@angular/core';
import * as atlas from 'azure-maps-control';
import { FoodTruckShort } from '../../interfaces/food-truck-short';

@Component({
  selector: 'app-azure-map',
  imports: [CommonModule],
  templateUrl: './azure-map.component.html',
  styleUrls: ['./azure-map.component.css']
})
export class AzureMapComponent implements OnInit {
  @Input() foodTruckLocations: FoodTruckShort[] = [];
  @Input() focusLocation!: { latitude: number; longitude: number };
  @Output() boundsChanged = new EventEmitter<{ minLatitude: number; maxLatitude: number; minLongitude: number; maxLongitude: number; centerLatitude: number; centerLongitude: number }>();

  private map!: atlas.Map;
  private popup = new atlas.Popup();

  ngOnInit(): void {
    this.initializeMap();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.map) {
      if (changes['foodTruckLocations']) {
        this.updateMarkers();
      }

      if (changes['focusLocation'] && this.focusLocation) {
        this.setMapFocus(this.focusLocation.latitude, this.focusLocation.longitude);
      }
    }
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
      const bounds = this.map.getCamera().bounds as [number, number, number, number];
      const center = this.map.getCamera().center as [number, number];

      this.boundsChanged.emit({
        minLongitude: bounds[0],
        minLatitude: bounds[1],
        maxLongitude: bounds[2],
        maxLatitude: bounds[3],
        centerLatitude: center[1],
        centerLongitude: center[0]
      });
    });
  }

  private updateMarkers(): void {
    this.map.markers.clear();
  
    this.foodTruckLocations.forEach(location => {
      const marker = new atlas.HtmlMarker({
        position: [location.longitude, location.latitude],
        color: 'red'
      });
  
      this.map.markers.add(marker);
  
      let isMouseOver = false;
      let closePopupTimeout: any;
  
      this.map.events.add('mouseover', marker, () => {
        clearTimeout(closePopupTimeout);
        isMouseOver = true;
  
        // Crear el contenido del popup
        const popupContent = `
          <div style="max-width: 200px; max-height: 100px; overflow: hidden; text-overflow: ellipsis;">
            <strong>${location.applicant}</strong><br>
            ${location.address}<br>
            <em>${location.fooditems}</em>
          </div>
        `;
  
        // Configurar el popup
        this.popup.setOptions({
          content: popupContent,
          position: [location.longitude, location.latitude],
          pixelOffset: [0, -20], // Ajustar posición para que no quede centrado exactamente en el marcador
          closeButton: true, // Mostrar botón de cierre
          closeOnMouseOut: false // El popup no se cierra automáticamente al salir del mouse
        });
  
        // Abrir el popup
        this.popup.open(this.map);
      });
  
      // Configurar cierre diferido al salir del mouse
      this.map.events.add('mouseout', marker, () => {
        isMouseOver = false;
        closePopupTimeout = setTimeout(() => {
          if (!isMouseOver) {
            this.popup.close();
          }
        }, 200);
      });
    });
  }

  private setMapFocus(latitude: number, longitude: number): void {
    this.map.setCamera({
      center: [longitude, latitude],
      zoom: 16 // Zoom más cercano para enfocar la ubicación
    });
  }
  
}