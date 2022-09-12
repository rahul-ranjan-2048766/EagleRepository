import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../shared/api.service';
import { LoginApiService } from '../signin/login-api.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {

  /* GLOBAL VARIABLES */
  liberty = 'assets/images/liberty.svg';
  cars = 'assets/images/cars.jfif';
  forest = 'assets/images/cherry-640.png';
  windmill = 'assets/images/windmill.png';
  overlay : boolean = false;
  isClosed:boolean = false;
  data : any;
  friends : any;
  userPic = 'assets/images/user-icon.png';
  butterfly = 'assets/images/butterfly.png';
  loginId : any;
  screenSaver = true;
  displayUser : any;

  constructor(
    private loginApi: LoginApiService,
    private router : Router,
    private api : ApiService
  ) { }

  ngOnInit(): void {
    if (!this.loginApi.CheckValidLoginTime()) {
      this.router.navigate(["index"]);
    }

    this.loginId = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);
    this.displayUser = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);

    this.toggleClass();

    this.api.GetUsers().subscribe((res : any) => {
      if (res) {
        this.friends = res;        
        this.screenSaver = false;
      }
    }, (error : any) => {
      console.log(error);
      this.screenSaver = false;
    });
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


  searchUsers($event : any) {
    if ($event.target.value.trim() === "" || $event.target.value === null) {
      this.api.GetUsers().subscribe((res : any) => {
        if (res) {
          this.friends = res;
        }
      }, (error : any) => {
        console.log(error);
      });
    }

    this.api.SearchUsers($event.target.value).subscribe((res : any) => {
      if (res) {
        this.friends = res;
      }
    }, (error : any) => {
      console.log(error);
    });
  }

}
