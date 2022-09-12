import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginApiService {

  /* GLOBAL VARIABLES */
  LOGGED_IN_LOGIN_ID = "LOGGED_IN_LOGIN_ID";
  USER_TOKEN = "USER_TOKEN";
  VALID_FROM = "VALID_FROM";
  VALID_TO = "VALID_TO";

  constructor() { }

  CheckValidLoginTime() : boolean {
    const validTo = localStorage.getItem(this.VALID_TO);

    if (validTo === null)
    return false;

    var currentTime = new Date();
    var toTime = new Date(validTo);

    console.log(currentTime);
    console.log(toTime);

    if (toTime > currentTime)
      return true;

    return false;
  }

  SignOut() {
    localStorage.removeItem(this.LOGGED_IN_LOGIN_ID);
    localStorage.removeItem(this.USER_TOKEN);
    localStorage.removeItem(this.VALID_FROM);
    localStorage.removeItem(this.VALID_TO);

    localStorage.clear();
  }

}
