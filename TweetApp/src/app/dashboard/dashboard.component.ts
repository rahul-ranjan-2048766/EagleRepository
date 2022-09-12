import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginApiService } from '../signin/login-api.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  /* GLOBAL VARIABLES */
  liberty = 'assets/images/liberty.svg';
  cars = 'assets/images/cars.jfif';
  forest = 'assets/images/cherry-640.png';
  windmill = 'assets/images/windmill.png';
  overlay : boolean = false;
  isClosed:boolean = false;
  data : any;
  screenSaver = true;
  displayUser : any;

  constructor(
    private loginApi: LoginApiService,
    private router : Router
  ) { }

  ngOnInit(): void {
    if (!this.loginApi.CheckValidLoginTime()) {
      this.router.navigate(["index"]);
    }

    this.toggleClass();

    this.displayUser = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);
    this.screenSaver = false;
  }

  toggleClass(){
    document.getElementById("home")?.classList.toggle("toggled");
    
    if(this.isClosed == true) {
      document.getElementById("hamburger")?.classList.remove("is-open");
      document.getElementById("hamburger")?.classList.add("is-closed");
      this.isClosed = false;
    }else{
      document.getElementById("hamburger")?.classList.remove("is-closed");
      document.getElementById("hamburger")?.classList.add("is-open");
      this.isClosed = true;
    }
    
  }


  Signout() {
    this.loginApi.SignOut();

    this.router.navigate(["index"]);
  }

}
