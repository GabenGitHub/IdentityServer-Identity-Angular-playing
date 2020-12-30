import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Constants } from '../constants';
import { Subject } from 'rxjs';
import * as Oidc from 'oidc-client';

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
      client_secret: 'secret',
      redirect_uri: `${Constants.clisentRoot}/signin-callback`,
      scope: 'openid profile RoleScope',
      response_type: 'code',
      post_logout_redirect_uri: `${Constants.clisentRoot}/signout-callback`,

      // Persist access token, id token, claims after closing the browser
      userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
      // TODO: check this:
      loadUserInfo: true,
    };
  }

  constructor() {
    this._userManager = new UserManager(this.idsSettings);
  }

  public login = () => this._userManager.signinRedirect();
  public logout = () => this._userManager.signoutRedirect();

  public getUser = () => this._userManager.getUser().then(user => {
    console.log('user:', user);
  })

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

  private checkUser = (user: User): boolean => !!user && !user.expired;

  public finishLogin = (): Promise<User> => {
    return this._userManager.signinRedirectCallback()
      .then(user => {
        this._user = user;
        this._loginChangedSubject.next(this.checkUser(user));
        return user;
      });
  }

  public finishLogout = () => {
    this._user = null;
    return this._userManager.signoutRedirectCallback();
  }
}
