import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;

  constructor() { }

  ngOnInit() {

  }
}
