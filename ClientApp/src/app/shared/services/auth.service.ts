import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Constants } from '../constants';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _userManager: UserManager;
  private _user: User;
  private _loginChangedSubject = new Subject<boolean>();

  public loginChanged = this._loginChangedSubject.asObservable();

  private get idsSettings(): UserManagerSettings {
    return {
      authority: Constants.idsAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clisentRoot}/signin-callback`,
      scope: 'openid profile',
      response_type: 'code',
      post_logout_redirect_uri: `${Constants.clisentRoot}/signout-callback`
    };
  }

  constructor() {
    this._userManager = new UserManager(this.idsSettings);
  }

  public login = () => {
    return this._userManager.signinRedirect();
  }

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser()
      .then(user => {
        if (this._user !== user) {
          this._loginChangedSubject.next(this.checkUser(user));
        }
        this._user = user;

        return this.checkUser(user);
      });
  }

  private checkUser = (user: User): boolean => {
    return !!user && !user.expired; // ! check this
  }
}
