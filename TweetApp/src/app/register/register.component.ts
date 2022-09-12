import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { User } from '../models/User';
import { ApiService } from '../shared/api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  message : any;
  formValue !: FormGroup;
  screenSaver = true;
  user : User = new User();

  constructor(
    private builder : FormBuilder,
    private api : ApiService
  ) { }

  ngOnInit(): void {
    this.formValue = this.builder.group({
      firstName : [''],
      lastName : [''],
      email : [''],
      loginId : [''],
      contactNumber : [''],
      password : [''],
      confirmPassword : ['']
    });

    this.screenSaver = false;
  }


  validateEmail(email : any) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
  }


  validContactNumber(number : any) {
    const re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    return re.test(number);
  }


  saveUser() {

    this.user.firstName = this.formValue.value.firstName;
    this.user.lastName = this.formValue.value.lastName;
    this.user.email = this.formValue.value.email;
    this.user.loginId = this.formValue.value.loginId;
    this.user.contactNumber = this.formValue.value.contactNumber;
    this.user.password = this.formValue.value.password

    if (this.user.firstName.trim() == "" || this.user.firstName == null) {
      alert("Please Enter First Name");
      return;
    } else if (this.user.lastName.trim() == "" || this.user.lastName == null) {
      alert("Please Enter Last Name");
      return;
    } else if (this.user.email.trim() == "" || this.user.email == null) {
      alert("Please Enter Your Email");
      return;
    } else if (this.user.loginId.trim() == "" || this.user.loginId == null) {
      alert("Please Enter Login Id");
      return;
    } else if (this.user.contactNumber.trim() == "" || this.user.contactNumber == null) {
      alert("Please Enter Contact Number");
      return;
    } else if (this.user.password.trim() == "" || this.user.password == null) {
      alert("Please Enter Your Password");
      return;
    } else if (this.formValue.value.confirmPassword.trim() == "" || this.formValue.value.confirmPassword == null) {
      alert("Please Enter Confirm Password");
      return;
    } else if (this.formValue.value.password !== this.formValue.value.confirmPassword) {
      alert("Passwords do not match");
      return;
    } else if (!this.validateEmail(this.user.email)) {
      alert("Invalid Email");
      return;
    } else if (!this.validContactNumber(this.user.contactNumber)) {
      alert("Incorrect / Invalid Contact Number");
      return;
    } else {
      this.screenSaver = true;
      this.api.RegisterUser(this.user).subscribe((res : any) => {
        if(res) {
          this.message = res["message"];
          this.formValue.reset();
          this.screenSaver = false;
        }
      }, (error : any) => {
        console.log(error);
        this.message = error["error"]["message"];
        this.screenSaver = false;
      });
      
      console.log(this.user);
    }
    
  }


  removeMessage() {
    this.message = null;
  }

}
