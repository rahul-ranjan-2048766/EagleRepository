import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../models/User';
import { ApiService } from '../shared/api.service';
import { LoginApiService } from '../signin/login-api.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  /* GLOBAL VARIABLES */
  liberty = 'assets/images/liberty.svg';
  cars = 'assets/images/cars.jfif';
  forest = 'assets/images/cherry-640.png';
  windmill = 'assets/images/windmill.png';
  overlay : boolean = false;
  isClosed:boolean = false;
  data : any;
  formValue !: FormGroup;
  user : User = new User();
  message : any;
  screenSaver = true;
  displayUser : any;

  constructor(
    private loginApi: LoginApiService,
    private router : Router,
    private builder : FormBuilder,
    private api : ApiService
  ) { }

  ngOnInit(): void {
    if (!this.loginApi.CheckValidLoginTime()) {
      this.router.navigate(["index"]);
    }

    this.toggleClass();
    this.displayUser = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);

    this.formValue = this.builder.group({
      firstName : [''],
      lastName : [''],
      email : [''],
      contactNumber : [''],
      password : ['']
    });

    const loginId = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);
    this.api.GetUserByLoginId(loginId).subscribe((res : any) => {
      if (res) {

        this.formValue = new FormGroup({
          firstName : new FormControl(res["firstName"]),
          lastName : new FormControl(res["lastName"]),
          email : new FormControl(res["email"]),
          contactNumber : new FormControl(res["contactNumber"]),
          password : new FormControl(res["password"])
        });

        this.user.id = res["id"];
        this.user.firstName = res["firstName"];
        this.user.lastName = res["lastName"];
        this.user.loginId = res["loginId"]
        this.user.email = res["email"];
        this.user.contactNumber = res["contactNumber"];
        this.user.password = res["password"];
        this.user.dateTime = res["dateTime"];

        console.log(this.user);

      }
    });

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


  validateEmail(email : any) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
  }


  validContactNumber(number : any) {
    const re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    return re.test(number);
  }


  updateProfile() {
    this.user.firstName = this.formValue.value.firstName;
    this.user.lastName = this.formValue.value.lastName;
    this.user.email = this.formValue.value.email;
    this.user.contactNumber = this.formValue.value.contactNumber;
    this.user.password = this.formValue.value.password;

    this.screenSaver = true;

    this.api.UpdateUser(this.user).subscribe((res : any) => {
      if (res) {
        this.message = res["message"];
        this.screenSaver = false;
      }
    }, (error : any) => {
      this.message = error["error"]["message"];
      this.screenSaver = false;
    });

  }


  removeMessage() {
    this.message = "";
  }

}
