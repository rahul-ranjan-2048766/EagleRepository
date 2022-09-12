import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { UserCred } from '../models/UserCred';
import { ApiService } from '../shared/api.service';
import { LoginApiService } from '../signin/login-api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  /* GLOBAL VARIABLES */
  bunny = "assets/images/bunny.svg";
  butterfly = "assets/images/butterfly.png";
  message !: string;
  formValue !: FormGroup;
  screenSaver = true;
  userCred : UserCred = new UserCred();

  constructor(
    private builder : FormBuilder,
    private api : ApiService,
    private router : Router,
    private loginApi : LoginApiService
  ) { }

  ngOnInit(): void {
    this.formValue = this.builder.group({
      loginId : [''],
      password : ['']
    });

    this.screenSaver = false;
  }


  userLogin() {
    this.userCred.loginId = this.formValue.value.loginId;
    this.userCred.password = this.formValue.value.password;

    if (this.formValue.value.loginId.trim() === "" || this.formValue.value.loginId === null) {
      alert("Please Enter Login Id");
      return;
    } else if (this.formValue.value.password.trim() === "" || this.formValue.value.password === null) {
      alert("Please Enter Your Password");
      return;
    }
    
    this.screenSaver = true;
    this.api.AuthenticateUser(this.userCred).subscribe((res : any) => {
      if (res) {
        
        localStorage.setItem(this.loginApi.LOGGED_IN_LOGIN_ID, res["loginId"]);
        localStorage.setItem(this.loginApi.USER_TOKEN, res["token"]);
        localStorage.setItem(this.loginApi.VALID_FROM, res["validFrom"]);
        localStorage.setItem(this.loginApi.VALID_TO, res["validTo"]);
        this.screenSaver = false;
        this.router.navigate(["dashboard"]);
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
