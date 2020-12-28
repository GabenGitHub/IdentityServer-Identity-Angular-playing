import { Component, OnInit } from '@angular/core';
import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app with oidc';
  public userAuthenticated = false;

  constructor(private _authService: AuthService) {
    this._authService.loginChanged
      .subscribe(userAuthenticated => {
        this.userAuthenticated = this.userAuthenticated;
      });
  }

  ngOnInit(): void {
    this._authService.isAuthenticated()
      .then(userAuthenticated => {
        this.userAuthenticated = this.userAuthenticated;
      });
  }
}
