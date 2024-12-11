import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AzureMapComponent } from './azure-map.component';

describe('AzureMapComponent', () => {
  let component: AzureMapComponent;
  let fixture: ComponentFixture<AzureMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AzureMapComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AzureMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
