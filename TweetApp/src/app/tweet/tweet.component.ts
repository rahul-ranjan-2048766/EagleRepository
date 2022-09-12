import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Comment } from '../models/Comment';
import { Tweet } from '../models/Tweet';
import { ApiService } from '../shared/api.service';
import { LoginApiService } from '../signin/login-api.service';

@Component({
  selector: 'app-tweet',
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent implements OnInit {

  /* GLOBAL VARIABLES */
  liberty = 'assets/images/liberty.svg';
  cars = 'assets/images/cars.jfif';
  forest = 'assets/images/cherry-640.png';
  windmill = 'assets/images/windmill.png';
  overlay : boolean = false;
  isClosed:boolean = false;
  data : any;
  inputToggle = false;
  formValue !: FormGroup;
  tweets : any;
  userPic = 'assets/images/user-icon.png';
  screenSaver = false;
  tagEvent : any;
  tagTweetId : any;
  displayUser : any;

  constructor(
    private loginApi: LoginApiService,
    private router : Router,
    private api : ApiService,
    private builder : FormBuilder
  ) { }

  ngOnInit(): void {
    if (!this.loginApi.CheckValidLoginTime()) {
      this.router.navigate(["index"]);
    }

    this.toggleClass();
    this.displayUser = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);

    this.formValue = this.builder.group({
      hashtag : [''],
      tweetMessage : ['']
    });

    this.getTweets();
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


  toggleInput() {
    if (this.inputToggle == false) {
      this.inputToggle = true;
      document.getElementById("tweet")?.classList.remove("tweet1");
      document.getElementById("tweet")?.classList.add("tweet2");
    }
    else {
      this.inputToggle = false;
      document.getElementById("tweet")?.classList.remove("tweet2");
      document.getElementById("tweet")?.classList.add("tweet1");
    }
    
  }


  saveTweet() {
    var loginid = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);
    const tweet = new Tweet();
    tweet.tag = this.formValue.value.hashtag;
    tweet.message = this.formValue.value.tweetMessage;
    tweet.sender = loginid ? loginid : "";
    
    this.screenSaver = true;
    this.api.SaveTweet(tweet).subscribe((res : any) => {
      if (res) {
        this.getTweets();
        this.screenSaver = false;
      }
    }, (error : any) => {
      alert(error["error"]["message"]);
      this.screenSaver = false;
    });
  }

  getTweets() {
    this.api.GetTweets().subscribe((res : any) => {
      if (res) {
        this.tweets = res;
        this.screenSaver = false;
      }
    }, (error : any) => {
      console.log(error);
      this.screenSaver = false;
    });
  }


  saveComment($event : any, tweetId : any) {
    const comment = new Comment();
    const loginId = localStorage.getItem(this.loginApi.LOGGED_IN_LOGIN_ID);

    if (tweetId == this.tagTweetId) {      
      comment.message = $event.target.value;
      comment.sender = loginId ? loginId : "";
      comment.tag = this.tagEvent;
      comment.tweetid = tweetId;
    }
    else {
      comment.message = $event.target.value;
      comment.sender = loginId ? loginId : "";
      comment.tweetid = tweetId;
    }
    
    $event.target.value = null;

    const tags = document.getElementsByClassName("input2") as HTMLCollectionOf<HTMLInputElement>;

    for(let i = 0; i < tags.length; i++) {
      tags[i].value = "";
    } 

    console.log(comment);

    this.screenSaver = true;

    this.api.SaveComment(comment).subscribe((res : any) => {
      if (res) {
        console.log(res["message"]);
        this.getTweets();
        this.screenSaver = false;
      }
    }, (error : any) => {
      console.log(error["error"]["message"]);
      this.screenSaver = false;
    });
  }


  saveTag($event : any, tweetId : any) {
    console.log($event.target.value);
    console.log(tweetId);

    this.tagEvent = $event.target.value;
    this.tagTweetId = tweetId;
  }

}
