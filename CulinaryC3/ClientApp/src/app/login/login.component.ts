import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../UserService';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [UserService]
})
/** Login component*/
export class LoginComponent {

  /** Login ctor */
  constructor(private userService: UserService, private router: Router) {
    console.log(localStorage.getItem('userEmail'));
  }

  signIn(form: NgForm) {
    this.userService.leaderboard().subscribe((result) => {
      for (var i = 0; i < result.length; i++) {
        if (result[i].loginId.toLowerCase() == form.form.value.email.toLowerCase() && result[i].password == form.form.value.password) {
          localStorage.setItem('userEmail', result[i].loginId);
          this.router.navigate(['/app-profile']).then(() => {
            window.location.reload();
          });
        }
      }
    })
  }
}
