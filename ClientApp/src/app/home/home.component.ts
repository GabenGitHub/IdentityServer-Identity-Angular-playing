import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  public isUserAuthenticated = false;

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    this.getUser();
  }

  public getUser = async () => {
    this.isUserAuthenticated = await this._authService.isAuthenticated();
    return this._authService.getUser();
  }
}
