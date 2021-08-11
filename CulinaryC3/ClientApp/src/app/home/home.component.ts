import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { Recipe } from '../../Recipe';
import { RecipeofDay } from '../../RecipeofDay';
import { RecipeService } from '../../RecipeService';
import { UserService } from '../../UserService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [UserService, RecipeService]
})
export class HomeComponent {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  recipeString: string;
  recipeDay: RecipeofDay;
  recipe: Recipe;


  constructor(private userService: UserService, private recipeService: RecipeService) {
    let day = new Date();
    this.recipeDay = JSON.parse(localStorage.getItem('recipeDay'));
    console.log(this.recipeDay);
    if (this.recipeDay.dayofMonth === null || this.recipeDay.dayofMonth === day.getDate()) {
      this.userService.recipeday().subscribe((r) => {
        this.recipeDay = r
        console.log(this.recipeDay);
        localStorage.setItem('recipeDay', JSON.stringify(r));
      });
    } else if (this.recipeDay.dayofMonth === 31 && day.getDate() === 1 || this.recipeDay.dayofMonth === 32 && day.getDate() === 1 || this.recipeDay.dayofMonth === 29 && day.getDate() === 1) {
      this.userService.recipeday().subscribe((r) => {
        this.recipeDay = r
        console.log(this.recipeDay);
        localStorage.setItem('recipeDay', JSON.stringify(r));
      });
    }
    this.getRecipe();
  }

  ngOnInit() {

  }

  getRecipe() {
    this.recipeService.getRecipeById(this.recipeDay.recipeId).subscribe((result) => {
      this.recipe = result;
      console.log(this.recipe);
    })
  }
}
