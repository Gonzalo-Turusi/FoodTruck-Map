import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FoodTrucksComponent } from './food-trucks.component';

describe('FoodTrucksComponent', () => {
  let component: FoodTrucksComponent;
  let fixture: ComponentFixture<FoodTrucksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FoodTrucksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FoodTrucksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
